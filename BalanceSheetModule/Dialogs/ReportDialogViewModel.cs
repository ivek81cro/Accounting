using AccountingUI.Core.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BalanceSheetModule.Dialogs
{
    public class ReportDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "Print";

        public ReportDialogViewModel()
        {
            OkPrintCommand = new DelegateCommand<string>(SetPrintOk);
        }

        public DelegateCommand<FlowDocument> PrintCommand
        {
            get
            {
                return new DelegateCommand<FlowDocument>(v =>
                {
                    PrintDialog printDialog = new PrintDialog();

                    if (printDialog.ShowDialog() == true)
                    {
                        printDialog.PrintDocument(((IDocumentPaginatorSource)v).DocumentPaginator, "Flow Document Print Job");
                    }
                });
            }
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
            PrintDocument = parameters.GetValue<FixedDocumentSequence>("document");
        }

        private void SetPrintOk(string obj)
        {
            if (obj == "1")
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            }
        }
    }
}
