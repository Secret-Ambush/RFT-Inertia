using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace WpfApp1
{
    public class DocumentConverter
    {
        public byte[] ConvertToPdf(MyDocumentPaginator paginator, Size pageSize)
        {
            using (var mem = new MemoryStream())
            using (var pkg = Package.Open(mem, FileMode.Create, FileAccess.ReadWrite))
            using (XpsDocument xpsDoc = new XpsDocument(pkg))
            {
                XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);

                FixedDocument fixedDoc = new FixedDocument();

                double marginSize = 25;
                double contentMargin = 10;
                double subheadingHeight = 30;
                double logoHeight = 50;
                double logoWidth = 100;

                string heading = "Inertia Calculator Report";

                string[] subheadings = new string[]
                {
                    "1) User Input: Section Dimensions",
                    "2) User Input: Rebar Details",
                    "3) Calculated Rebar Properties",
                    "4) Calculated Section Properties",
                    "5) Section Illustration"
                };

                int visualsPerPage = 3;

                // Define styles
                var titleStyle = new Style(typeof(TextBlock))
                {
                    Setters = {
                        new Setter(TextBlock.FontFamilyProperty, new FontFamily("Courier New")),
                        new Setter(TextBlock.FontSizeProperty, 16.0),
                        new Setter(TextBlock.FontWeightProperty, FontWeights.Bold),
                        new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center),
                        new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                        new Setter(TextBlock.TextDecorationsProperty, TextDecorations.Underline)
                    }
                };

                var subheadingStyle = new Style(typeof(TextBlock))
                {
                    Setters = {
                        new Setter(TextBlock.FontFamilyProperty, new FontFamily("Courier New")),
                        new Setter(TextBlock.FontSizeProperty, 14.0),
                        new Setter(TextBlock.FontWeightProperty, FontWeights.Bold),
                        new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                        new Setter(TextBlock.MarginProperty, new Thickness(5, 5, 5, 5)),
                        new Setter(TextBlock.TextDecorationsProperty, TextDecorations.Underline)
                    }
                };

                var contentStyle = new Style(typeof(TextBlock))
                {
                    Setters = {
                        new Setter(TextBlock.FontFamilyProperty, new FontFamily("Courier New")),
                        new Setter(TextBlock.FontSizeProperty, 10.0),
                        new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                        new Setter(TextBlock.MarginProperty, new Thickness(5, 2, 5, 2))
                    }
                };

                var borderStyle = new Style(typeof(Rectangle))
                {
                    Setters = {
                        new Setter(Rectangle.StrokeProperty, Brushes.Black),
                        new Setter(Rectangle.StrokeThicknessProperty, 1.0)
                    }
                };

                for (int pageIndex = 0; pageIndex < paginator.PageCount; pageIndex += visualsPerPage)
                {
                    PageContent pageContent = new PageContent();
                    FixedPage fixedPage = new FixedPage
                    {
                        Width = pageSize.Width,
                        Height = pageSize.Height
                    };

                    // Page Border
                    Rectangle pageBorder = new Rectangle
                    {
                        Width = pageSize.Width - 2 * marginSize,
                        Height = pageSize.Height - 2 * marginSize,
                        Margin = new Thickness(marginSize),
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };
                    fixedPage.Children.Add(pageBorder);

                    if (pageIndex == 0)
                    {
                        var logoImage = new Image
                        {
                            Width = logoWidth + 4,
                            Height = logoHeight,
                            Margin = new Thickness(marginSize + 10, marginSize + 4, 0, 0)
                        };

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri("./View/Resources/Jacobs.png", UriKind.RelativeOrAbsolute);
                        bitmap.EndInit();
                        logoImage.Source = bitmap;

                        fixedPage.Children.Add(logoImage);

                        var leftmargin = (pageSize.Width - (marginSize + logoHeight)) / 2;

                        // Heading
                        TextBlock titleBlock = new TextBlock
                        {
                            Text = heading,
                            Style = titleStyle,
                            Width = pageSize.Width - 2 * marginSize,
                            Height = logoHeight + 4,
                            Margin = new Thickness(marginSize, marginSize + logoHeight/2, marginSize, 0),
                            TextAlignment = TextAlignment.Center
                        };

                        fixedPage.Children.Add(titleBlock);
                    }

                    for (int i = 0; i < visualsPerPage && pageIndex + i < paginator.PageCount; i++)
                    {
                        double availableHeight = pageSize.Height - 2 * marginSize - (subheadingHeight + contentMargin + logoHeight) * (visualsPerPage + 1);
                        double visualHeight = availableHeight / visualsPerPage;

                        var page = paginator.GetPage(pageIndex + i);

                        if (page == null) continue;

                        // Subheading
                        TextBlock subheadingBlock = new TextBlock
                        {
                            Text = subheadings[pageIndex + i],
                            Style = subheadingStyle,
                            Width = pageSize.Width - 2 * marginSize,
                            Height = subheadingHeight,
                            Margin = new Thickness(marginSize + 4, 
                                                    marginSize + i * (visualHeight + subheadingHeight + contentMargin) + subheadingHeight + logoHeight, 
                                                    marginSize, 
                                                    0),
                            VerticalAlignment = VerticalAlignment.Top,
                            TextAlignment = TextAlignment.Left
                        };

                        fixedPage.Children.Add(subheadingBlock);

                        // Visual
                        VisualBrush vb = new VisualBrush(page.Visual)
                        {
                            Stretch = Stretch.Uniform
                        };

                        Rectangle visualRect = new Rectangle
                        {
                            Width = pageSize.Width - 2 * marginSize - 2 * contentMargin,
                            Height = visualHeight,
                            Margin = new Thickness(marginSize + contentMargin, 
                                                    marginSize + subheadingHeight + subheadingBlock.Height + logoHeight + i * (visualHeight + subheadingHeight + contentMargin), 
                                                    marginSize + contentMargin, 
                                                    0),
                            Fill = vb
                        };

                        fixedPage.Children.Add(visualRect);
                    }

                    ((IAddChild)pageContent).AddChild(fixedPage);
                    fixedDoc.Pages.Add(pageContent);
                }

                xpsWriter.Write(fixedDoc);
                xpsDoc.Close();
                pkg.Close();
                mem.Seek(0, SeekOrigin.Begin);
                return mem.ToArray();
            }
        }
    }
}