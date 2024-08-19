//using PdfSharp.Drawing;
//using PdfSharp.Pdf;
//using PdfSharp;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.IO.Packaging;
//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Xps;
//using System.Windows.Xps.Packaging;
//using System.Windows.Input;
//using System.Drawing.Printing;
//using System.Windows.Documents;

//namespace WpfApp1
//{
//    public class ReportViewModel : BaseViewModel
//    {
//        public ICommand WPFReportCommand { get; set; }

//        #region Report Generation
//        public void HelloWorld()
//        {
//            // Create a new PDF document
//            PdfDocument document = new PdfDocument();
//            document.Info.Title = "Created with PDFsharp";

//            // Create an empty page
//            PdfPage page = document.AddPage();

//            // Get an XGraphics object for drawing
//            XGraphics gfx = XGraphics.FromPdfPage(page);

//            // Create a font
//            XFont font = new XFont("Verdana", 20, XFontStyleEx.BoldItalic);

//            // Draw the text
//            gfx.DrawString("Hello, World!", font, XBrushes.Black,
//                new XRect(0, 0, page.Width, page.Height),
//                XStringFormats.Center);

//            // Save the document...
//            const string filename = "HelloWorld.pdf";
//            document.Save(filename);

//        }
        
//        //public void SampleReport()
//        //{
//        //    XRect rect;
//        //    XPen pen;

//        //    PdfDocument document = new PdfDocument();
//        //    document.Info.Title = "Created with PDFsharp";

//        //    PdfPage page = document.AddPage();

//        //    XGraphics gfx = XGraphics.FromPdfPage(page);

//        //    double x = 50, y = 100;
//        //    XFont fontH1 = new XFont("Times", 18, XFontStyleEx.Bold);
//        //    XFont font = new XFont("Times", 12);
//        //    XFont fontItalic = new XFont("Times", 12, XFontStyleEx.BoldItalic);
//        //    double ls = font.GetHeight();

//        //    gfx.DrawString("Create PDF on the fly with PDFsharp",
//        //        fontH1, XBrushes.Black, x, x);
//        //    gfx.DrawString("With PDFsharp you can use the same code to draw graphic, " +
//        //        "text and images on different targets.", font, XBrushes.Black, x, y);
//        //    y += ls;
//        //    gfx.DrawString("The object used for drawing is the XGraphics object.",
//        //        font, XBrushes.Black, x, y);
//        //    y += 2 * ls;

//        //    // Draw an arc
//        //    pen = new XPen(XColors.Red, 4);
//        //    pen.DashStyle = XDashStyle.Dash;
//        //    gfx.DrawArc(pen, x + 20, y, 100, 60, 150, 120);

//        //    // Draw a star
//        //    XGraphicsState gs = gfx.Save();
//        //    gfx.TranslateTransform(x + 140, y + 30);
//        //    for (int idx = 0; idx < 360; idx += 10)
//        //    {
//        //        gfx.RotateTransform(10);
//        //        gfx.DrawLine(XPens.DarkGreen, 0, 0, 30, 0);
//        //    }
//        //    gfx.Restore(gs);

//        //    // Draw a rounded rectangle
//        //    rect = new XRect(x + 230, y, 100, 60);
//        //    pen = new XPen(XColors.DarkBlue, 2.5);
//        //    XColor color1 = XColor.FromKnownColor(XKnownColor.DarkBlue);
//        //    XColor color2 = XColors.Red;
//        //    XLinearGradientBrush lbrush = new XLinearGradientBrush(rect, color1, color2,
//        //      XLinearGradientMode.Vertical);
//        //    gfx.DrawRoundedRectangle(pen, lbrush, rect, new XSize(10, 10));

//        //    // Draw a pie
//        //    pen = new XPen(XColors.DarkOrange, 1.5);
//        //    pen.DashStyle = XDashStyle.Dot;
//        //    gfx.DrawPie(pen, XBrushes.Blue, x + 360, y, 100, 60, -130, 135);

//        //    // Draw some more text
//        //    y += 60 + 2 * ls;
//        //    gfx.DrawString("With XGraphics you can draw on a PDF page as well as " +
//        //        "on any System.Drawing.Graphics object.", font, XBrushes.Black, x, y);
//        //    y += ls * 1.1;
//        //    gfx.DrawString("Use the same code to", font, XBrushes.Black, x, y);
//        //    x += 10;
//        //    y += ls * 1.1;
//        //    gfx.DrawString("• draw on a newly created PDF page", font, XBrushes.Black, x, y);
//        //    y += ls;
//        //    gfx.DrawString("• draw above or beneath of the content of an existing PDF page",
//        //        font, XBrushes.Black, x, y);
//        //    y += ls;
//        //    gfx.DrawString("• draw in a window", font, XBrushes.Black, x, y);
//        //    y += ls;
//        //    gfx.DrawString("• draw on a printer", font, XBrushes.Black, x, y);
//        //    y += ls;
//        //    gfx.DrawString("• draw in a bitmap image", font, XBrushes.Black, x, y);
//        //    x -= 10;
//        //    y += ls * 1.1;
//        //    gfx.DrawString("You can also import an existing PDF page and use it like " +
//        //        "an image, e.g. draw it on another PDF page.", font, XBrushes.Black, x, y);
//        //    y += ls * 1.1 * 2;
//        //    gfx.DrawString("Imported PDF pages are neither drawn nor printed; create a " +
//        //        "PDF file to see or print them!", fontItalic, XBrushes.Firebrick, x, y);
//        //    y += ls * 1.1;
//        //    gfx.DrawString("Below this text is a PDF form that will be visible when " +
//        //        "viewed or printed with a PDF viewer.", fontItalic, XBrushes.Firebrick, x, y);
//        //    y += ls * 1.1;
//        //    XGraphicsState state = gfx.Save();
//        //    XRect rcImage = new XRect(100, y, 100, 100 * Math.Sqrt(2));
//        //    gfx.DrawRectangle(XBrushes.Snow, rcImage);

//        //    const string filename = "HelloWorld.pdf";
//        //    document.Save(filename);
//        //}
//        //public void SampleReport2()
//        //{
//        //    // Render the grid to a RenderTargetBitmap
//        //    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
//        //        (int)InertiaCalculator.ActualWidth,
//        //        (int)InertiaCalculator.ActualHeight,
//        //        96,
//        //        96,
//        //        PixelFormats.Pbgra32);
//        //    renderTargetBitmap.Render(InertiaCalculator);

//        //    // Encode the RenderTargetBitmap to a PNG
//        //    PngBitmapEncoder pngImage = new PngBitmapEncoder();
//        //    pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

//        //    using (MemoryStream pngStream = new MemoryStream())
//        //    {
//        //        pngImage.Save(pngStream);
//        //        pngStream.Seek(0, SeekOrigin.Begin);

//        //        // Create a new PDF document
//        //        PdfDocument pdfDoc = new PdfDocument();
//        //        PdfPage pdfPage = pdfDoc.AddPage();
//        //        XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

//        //        // Load the PNG image into a PDFsharp XImage
//        //        XImage xImage = XImage.FromStream(pngStream);

//        //        // Draw the image on the PDF page
//        //        gfx.DrawImage(xImage, 0, 0, pdfPage.Width, pdfPage.Height);

//        //        // Save the PDF document to a file
//        //        pdfDoc.Save(d.FileName);
//        //    }
//        //}

//        //private void ExportToPdf()
//        //{
//        //    // Get the window content as a Visual
//        //    var window = Application.Current.MainWindow;
//        //    var visual = window.Content as FrameworkElement;
//        //    if (visual != null)
//        //    {
//        //        // Render the visual to a bitmap
//        //        var renderBitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Default);
//        //        renderBitmap.Render(visual);
//        //        // Convert the RenderTargetBitmap to a bitmap image
//        //        var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
//        //        using (var stream = new System.IO.MemoryStream())
//        //        {
//        //            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
//        //            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(renderBitmap));
//        //            encoder.Save(stream);
//        //            stream.Position = 0;
//        //            bitmapImage.BeginInit();
//        //            bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
//        //            bitmapImage.StreamSource = stream;
//        //            bitmapImage.EndInit();
//        //        }
//        //        // Save the bitmap to a PDF using PDFsharp
//        //        using (var document = new PdfDocument())
//        //        {
//        //            var page = document.AddPage();
//        //            page.Width = XUnit.FromPoint(visual.ActualWidth);
//        //            page.Height = XUnit.FromPoint(visual.ActualHeight);
//        //            using (var gfx = XGraphics.FromPdfPage(page))
//        //            {
//        //                var xImage = XImage.FromGdiPlusImage(bitmapImage);
//        //                gfx.DrawImage(xImage, 0, 0);
//        //            }
//        //            string pdfFilename = "Output.pdf";
//        //            document.Save(pdfFilename);
//        //            MessageBox.Show($"PDF saved to {pdfFilename}", "PDF Export", MessageBoxButton.OK, MessageBoxImage.Information);
//        //        }
//        //    }
//        //}

//        public ReportViewModel()
//        {
//            WPFReportCommand = new RelayCommand(() => WPFReportCommand_Execute());
//        }

//        public void WPFReportCommand_Execute()
//        {
//            using (var mem = new MemoryStream())
//            using (var pkg = Package.Open(mem, FileMode.Create, FileAccess.ReadWrite))
//            using (XpsDocument xpsDoc = new XpsDocument(pkg))
//            {
//                XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
//                MyDocumentPaginator paginator = new MyDocumentPaginator(listVisual, pageSize, false);
//                paginator.ComputePageCount();
//                for (int i = 0; i & lt; paginator.PageCount; i++)
//            {
//                    var p = paginator.GetPage(i);
//                    if (p == null)
//                        continue;
//                    var x = p.Visual;
//                    if (x is Canvas)
//                    {
//                        var canvas = x as Canvas;
//                        canvas.Width = pageSize.Width;
//                        canvas.Height = pageSize.Height;
//                        canvas.UpdateLayout();
//                    }
//                }
//                xpsWriter.Write(paginator);
//                xpsDoc.Close();
//                pkg.Close();
//                mem.Seek(0, SeekOrigin.Begin);
//                return mem.ToArray();
//            }
//        }

//        private void GeneratePdf()
//        {
//            Window window = Application.Current.MainWindow;

//            // Render the window content to a bitmap
//            var renderTarget = new RenderTargetBitmap(
//                (int)window.ActualWidth, (int)window.ActualHeight, 96, 96, PixelFormats.Pbgra32);
//            renderTarget.Render(window);

//            // Convert RenderTargetBitmap to BitmapImage
//            BitmapImage bitmapImage = BitmapFromRenderTarget(renderTarget);

//            // Convert BitmapImage to System.Drawing.Bitmap
//            Bitmap bitmap = BitmapFromBitmapImage(bitmapImage);

//            // Save the bitmap to a PDF using PDFsharp
//            using (var document = new PdfDocument())
//            {
//                var page = document.AddPage();
//                page.Width = XUnit.FromPoint(window.ActualWidth);
//                page.Height = XUnit.FromPoint(window.ActualHeight);
//                using (var gfx = XGraphics.FromPdfPage(page))
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        // Save the bitmap to a memory stream in PNG format
//                        bitmap.Save(memoryStream, ImageFormat.Png);
//                        memoryStream.Seek(0, SeekOrigin.Begin);
//                        // Create an XImage from the memory stream
//                        var xImage = XImage.FromStream(memoryStream);
//                        // Draw the image to the PDF page
//                        gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);
//                    }
//                }

//                string pdfFilename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Output.pdf");
//                document.Save(pdfFilename);
//                MessageBox.Show($"PDF saved to {pdfFilename}", "PDF Export", MessageBoxButton.OK, MessageBoxImage.Information);
//            }
//        }
//        private Bitmap BitmapFromBitmapImage(BitmapImage bitmapImage)
//        {
//            using (MemoryStream outStream = new MemoryStream())
//            {
//                BitmapEncoder encoder = new BmpBitmapEncoder();
//                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
//                encoder.Save(outStream);
//                Bitmap bitmap = new Bitmap(outStream);
//                return new Bitmap(bitmap);
//            }
//        }
//        private BitmapImage BitmapFromRenderTarget(RenderTargetBitmap renderTarget)
//        {
//            BitmapImage bitmapImage = new BitmapImage();
//            using (MemoryStream stream = new MemoryStream())
//            {
//                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
//                encoder.Frames.Add(BitmapFrame.Create(renderTarget));
//                encoder.Save(stream);
//                stream.Seek(0, SeekOrigin.Begin);
//                bitmapImage.BeginInit();
//                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
//                bitmapImage.StreamSource = stream;
//                bitmapImage.EndInit();
//            }
//            return bitmapImage;
//        }

//        #endregion

//    }
//}
