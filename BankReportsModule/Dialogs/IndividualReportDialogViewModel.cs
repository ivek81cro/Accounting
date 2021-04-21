using AccountingUI.Core.Models;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace BankkStatementsModule.Dialogs
{
    class IndividualReportDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "Izvod";

        public event Action<IDialogResult> RequestClose;

        private BankReportModel _reportHeader;
        public BankReportModel ReportHeader
        {
            get { return _reportHeader; }
            set { SetProperty(ref _reportHeader, value); }
        }

        private List<BankReportItemModel> _reportItems = new();
        public List<BankReportItemModel> ReportItems
        {
            get { return _reportItems; }
            set { SetProperty(ref _reportItems, value); }
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
            ReportHeader = parameters.GetValue<BankReportModel>("header");
            ReportItems = parameters.GetValue<List<BankReportItemModel>>("itemsList");
        }
    }
}
