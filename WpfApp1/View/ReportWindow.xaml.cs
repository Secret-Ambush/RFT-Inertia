using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PdfiumViewer;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public partial class ReportWindow : MetroWindow
    {
        public DocumentConverter _documentConverter;

        List<Visual> visual = null;
        byte[] pdfData = null;

        public ReportWindow(List<Visual> MainWindowVisuals)
        {
            InitializeComponent();
            _documentConverter = new DocumentConverter();

            visual = MainWindowVisuals;
            var pageSize = new Size(800, 1000);


            var paginator = new MyDocumentPaginator(visual, new Size(800, 1000), false);
            pdfData = _documentConverter.ConvertToPdf(paginator, new Size(800, 1000));
            //ShowPdfPreview(pdfData);

            // string htmlPreview = GenerateHtmlPreview(visual, pageSize);
            //WebBrowserPreview.NavigateToString(htmlPreview);

        }

        private string GenerateHtmlPreview(List<Visual> visuals, Size pageSize)
        {
            var htmlBuilder = new System.Text.StringBuilder();

            htmlBuilder.Append("<html><body>");

            foreach (var visual in visuals)
            {
                if (visual is FrameworkElement element)
                {
                    string htmlElement = ConvertVisualToHtml(element);
                    htmlBuilder.Append(htmlElement);
                }
            }

            htmlBuilder.Append("</body></html>");

            return htmlBuilder.ToString();
        }
        private string ConvertVisualToHtml(FrameworkElement element)
        {
            if (element is Grid grid)
            {
                return $"<div style='width:{grid.ActualWidth}px;height:{grid.ActualHeight}px;background-color:lightgray;'>Grid content here</div>";
            }
            else if (element is Canvas canvas)
            {
                return $"<div style='width:{canvas.ActualWidth}px;height:{canvas.ActualHeight}px;background-color:lightblue;'>Canvas content here</div>";
            }

            return $"<div>Unsupported element type: {element.GetType().Name}</div>";

        }
        private void ShowPdfPreview(byte[] pdfBytes)
        {
            using (Stream stream = new MemoryStream(pdfBytes))
            using (var document = PdfDocument.Load(stream))
            {
                var image = document.Render(0, 300, 300, true);
                string tempImagePath = Path.Combine(Path.GetTempPath(), "pdf_preview.png");
                DisplayImage(tempImagePath);
            }
        }
        private void DisplayImage(string imagePath)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            ImagePreview.Source = bitmap;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Save the PDF data
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                DefaultExt = "pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, pdfData);
            }
        }


    }
}
