using MahApps.Metro.Controls;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Xps.Packaging;

namespace WpfApp1
{
    public partial class ReportWindow : MetroWindow
    {
        public DocumentConverter _documentConverter;
        private string tempFilePath;
        List<Visual> visualsList = null;
        byte[] pdfData = null;

        public ReportWindow(List<Visual> MainWindowVisuals)
        {
            InitializeComponent();

            _documentConverter = new DocumentConverter();
            visualsList = MainWindowVisuals;
            var pageSize = new Size(800, 1000);
            var paginator = new MyDocumentPaginator(visualsList, pageSize, false);
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

