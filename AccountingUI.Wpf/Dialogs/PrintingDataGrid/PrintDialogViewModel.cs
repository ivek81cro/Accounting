using AccountingUI.Core.Services;
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
        private readonly ICompanyEndpoint _companyEndpoint;

        public string Title => "Ispis";

        public PrintDialogViewModel(ICompanyEndpoint companyEndpoint)
        {
            OkPrintCommand = new DelegateCommand<string>(SetPrintOk);
            _companyEndpoint = companyEndpoint;
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
            string title = parameters.GetValue<string>("title");
            OpenPrintDialog(visual, title);
        }

        private async void OpenPrintDialog(Visual v, string title)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
            printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            string documentTitle = title;
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

            var company = await _companyEndpoint.Get();

            CustomDataGridDocumentPaginator paginator = new(v as DataGrid, documentTitle, company, pageSize, new Thickness(30, 20, 30, 20), true);
            CreateDocument(paginator);
        }

        private void CreateDocument(CustomDataGridDocumentPaginator paginator)
        {
            string tempFileName = Path.GetTempFileName();
            File.Delete(tempFileName);
            using (XpsDocument xpsDocument = new XpsDocument(tempFileName, FileAccess.ReadWrite))
            {
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                writer.Write(paginator);
                PrintDocument = xpsDocument.GetFixedDocumentSequence();
            }
        }

        private void SetPrintOk(string obj)
        {
            if (obj == "1")
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                    printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
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
