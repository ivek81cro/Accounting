using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AccountingUI.Wpf.Dialogs.AccountBalanceCard
{
    public class BalanceCardDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;
        private readonly IAccountingJournalEndpoint _accountingJournalEndpoint;

        public BalanceCardDialogViewModel(IBookAccountsEndpoint bookAccountsEndpoint,
                                          IAccountingJournalEndpoint accountingJournalEndpoint)
        {
            _bookAccountsEndpoint = bookAccountsEndpoint;
            _accountingJournalEndpoint = accountingJournalEndpoint;
        }

        public string Title => "Kartica konta";

        public event Action<IDialogResult> RequestClose;

        private ObservableCollection<AccountBalanceModel> _accountCard;
        public ObservableCollection<AccountBalanceModel> AccountCard
        {
            get { return _accountCard; }
            set { SetProperty(ref _accountCard, value); }
        }

        private string _accountNumber;
        public string AccountNumber
        {
            get { return _accountNumber; }
            set { SetProperty(ref _accountNumber, value); }
        }

        private string _accountName;
        public string AccountName
        {
            get { return _accountName; }
            set { SetProperty(ref _accountName, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            AccountNumber = parameters.GetValue<string>("accountNumber");
            var result = await _bookAccountsEndpoint.GetByNumber(AccountNumber);
            AccountName = result.Opis;
            var list = await _accountingJournalEndpoint.LoadAccountCard(AccountNumber);
            CalculateCurrentSum(list);
            AccountCard = new ObservableCollection<AccountBalanceModel>(list);
        }

        private void CalculateCurrentSum(List<AccountBalanceModel> list)
        {
            decimal sum = 0;
            foreach(var item in list)
            {
                sum += item.Dugovna - item.Potrazna;
                item.Stanje = sum;
            }
        }
    }
}
