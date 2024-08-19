using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps.Packaging;

namespace WpfApp1
{
    public partial class ReportWindow : MetroWindow
    {
        public DocumentConverter _documentConverter;
        private string tempFilePath;
        List<Visual> visual = null;
        byte[] pdfData = null;

        public ReportWindow(List<Visual> MainWindowVisuals)
        {
            InitializeComponent();

            _documentConverter = new DocumentConverter();
            visual = MainWindowVisuals;
            var pageSize = new Size(800, 1000);
            var paginator = new MyDocumentPaginator(visual, pageSize, false);
            pdfData = _documentConverter.ConvertToPdf(paginator, pageSize);

            if (pdfData != null)
            {
                DisplayXpsDocument(pdfData);
            }
            else
            {
                MessageBox.Show("Failed to load byte array.");
            }

        }

        private FixedDocumentSequence GetFixedDocumentSequence(byte[] xpsBytes)
        {
            Uri packageUri;
            XpsDocument xpsDocument = null;

            using (MemoryStream xpsStream = new MemoryStream(xpsBytes))
            {
                using (Package package = Package.Open(xpsStream))
                {
                    packageUri = new Uri("xpsStream://myXps.xps");
                    PackageStore.AddPackage(packageUri, package);

                    try
                    {
                        xpsDocument = new XpsDocument(package, CompressionOption.Maximum, packageUri.AbsoluteUri);
                        return xpsDocument.GetFixedDocumentSequence();
                    }
                    finally
                    {
                        xpsDocument?.Close();
                        PackageStore.RemovePackage(packageUri);
                    }
                }
            }
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
        private void DisplayXpsDocument(byte[] byteArray)
        {
            // Create a temporary file to store the XPS document
            tempFilePath = Path.GetTempFileName();
            File.WriteAllBytes(tempFilePath, byteArray);
            XpsDocument xpsDocument = new XpsDocument(tempFilePath, FileAccess.Read);
            documentViewer.Document = xpsDocument.GetFixedDocumentSequence();

            // NOTE: Do not close the XpsDocument immediately, it must remain open for the DocumentViewer to function
            _xpsDocument = xpsDocument;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Clean up the temporary file and resources
            if (_xpsDocument != null)
            {
                _xpsDocument.Close();
                _xpsDocument = null;
            }

            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        private XpsDocument _xpsDocument;
    }
}

