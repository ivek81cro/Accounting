using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankkStatementsModule.Dialogs
{
    class IndividualReportDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService _showDialog;
        private readonly IAccountPairsEndpoint _accoutPairsEndpoint;

        private readonly string _bookName="Izvodi";

        public IndividualReportDialogViewModel(IDialogService showDialog,
                                               IAccountPairsEndpoint accoutPairsEndpoint)
        {
            _showDialog = showDialog;
            _accoutPairsEndpoint = accoutPairsEndpoint;

            AccountsLinkCommand = new DelegateCommand(AddNewPair, CanAddPair);
        }

        public DelegateCommand AccountsLinkCommand { get; private set; }

        public string Title => "Pojedinačni izvod";

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

        private BankReportItemModel _selectedEntry;
        public BankReportItemModel SelectedEntry
        {
            get { return _selectedEntry; }
            set 
            { 
                SetProperty(ref _selectedEntry, value);
                AccountsLinkCommand.RaiseCanExecuteChanged();
            }
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
            LoadAccountPairs();
        }

        private async void LoadAccountPairs()
        {
            var pairs = await _accoutPairsEndpoint.GetByBookName(_bookName);

            foreach (var reportEntry in ReportItems)
            {
                FindPair(pairs, reportEntry);
            }
        }

        private void FindPair(List<AccountPairModel> pairs, BankReportItemModel reportEntry)
        {
            List<AccountPairModel> result = new();
            if (pairs.Count != 0)
            {
                result = pairs.Where(
                    p => p.Name == reportEntry.Naziv)
                    .DefaultIfEmpty(new AccountPairModel()).ToList();
            }

            if (result.Count > 1)
            {
                foreach (var item in result)
                {
                    if (reportEntry.Opis.Contains(item.Description))
                    {
                        reportEntry.Konto = item.Account;
                        return;
                    }
                }
            }
            else
            {
                reportEntry.Konto = result.First().Account;
            }
        }

        private bool CanAddPair()
        {
            return SelectedEntry != null;
        }

        private void AddNewPair()
        {
            var param = new DialogParameters();
            param.Add("bankReport", SelectedEntry);
            _showDialog.ShowDialog("AccountPairDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedEntry.Konto = result.Parameters.GetValue<string>("bankReport");
                }
            });
        }
    }
}
