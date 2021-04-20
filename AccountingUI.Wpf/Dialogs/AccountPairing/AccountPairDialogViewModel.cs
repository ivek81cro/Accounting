using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace AccountingUI.Wpf.Dialogs.AccountPairing
{
    public class AccountPairDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IBookAccountsEndpoint _accountsEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IAccountPairsEndpoint _accountPairsEndpoint;

        public AccountPairDialogViewModel(IBookAccountsEndpoint accountsEndpoint,
                                          IDialogService showDialog,
                                          IAccountPairsEndpoint accountPairsEndpoint)
        {
            _accountsEndpoint = accountsEndpoint;
            _showDialog = showDialog;

            OpenAccountsSelectionCommand = new DelegateCommand(OpenAccountsSelection);
            SavePairCommand = new DelegateCommand(SavePair);
            _accountPairsEndpoint = accountPairsEndpoint;
        }

        public DelegateCommand OpenAccountsSelectionCommand { get; private set; }
        public DelegateCommand SavePairCommand { get; private set; }

        public string Title => "Uparivanje konta";

        public event Action<IDialogResult> RequestClose;

        private string _account;
        public string Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        private AccountPairModel _accountPair = new();
        public AccountPairModel AccountPair
        {
            get { return _accountPair; }
            set { SetProperty(ref _accountPair, value); }
        }

        private AccountingJournalModel _entry;
        public AccountingJournalModel Entry
        {
            get { return _entry; }
            set { SetProperty(ref _entry, value); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
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
            Entry = parameters.GetValue<AccountingJournalModel>("entry");
            AccountPair.Name = Entry.Dokument.Split(':')[0];
            AccountPair.Description = Entry.Opis;
            AccountPair.BookName = Entry.VrstaTemeljnice;
        }

        private void OpenAccountsSelection()
        {
            _showDialog.ShowDialog("AccountsSelectionDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    AccountPair.Account = result.Parameters.GetValue<BookAccountModel>("account").Konto;
                }
            });
        }

        private bool CanSave()
        {
            return AccountPair != null
                && AccountPair.Description != null
                && AccountPair.Account != null
                && AccountPair.Name != null;
        }

        private void SavePair()
        {
            if (CanSave())
            {
                var param = new DialogParameters();
                param.Add("account", AccountPair.Account);
                _accountPairsEndpoint.Post(AccountPair);
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param)); 
            }
            else
            {
                StatusMessage = "Niste unjeli sve potrebne podatke";
            }
        }
    }
}
