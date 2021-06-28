using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace AccountingUI.Wpf.Dialogs.PrintingDataGrid
{
    public class PrintDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "Ispis";

        public PrintDialogViewModel()
        {
            OkPrintCommand = new DelegateCommand<string>(SetPrintOk);
        }

        public DelegateCommand<string> OkPrintCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        private FixedDocumentSequence _printDocument;
        public FixedDocumentSequence PrintDocument
        {
            get { return _printDocument; }
            set { SetProperty(ref _printDocument, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var visual = parameters.GetValue<Visual>("datagrid");
            OpenPrintDialog(visual);
        }

        private void OpenPrintDialog(Visual v)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
            printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            string documentTitle = "Bilanca";
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

            CustomDataGridDocumentPaginator paginator = new(v as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20), true);
            CreateDocument(printDialog, paginator);
        }

        private void CreateDocument(PrintDialog printDialog, CustomDataGridDocumentPaginator paginator)
        {
            string tempFileName = Path.GetTempFileName();
            File.Delete(tempFileName);
            using (XpsDocument xpsDocument = new XpsDocument(tempFileName, FileAccess.ReadWrite))
            {
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                writer.Write(paginator);
                ShowPreview(xpsDocument.GetFixedDocumentSequence());
            }
        }

        private void ShowPreview(FixedDocumentSequence fixedDocumentSequence)
        {
            PrintDocument = fixedDocumentSequence;
        }

        private void SetPrintOk(string obj)
        {
            if (obj == "1")
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintDocument(PrintDocument.DocumentPaginator, "Print document");
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                }
            }
            else
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            }
        }
    }
}
