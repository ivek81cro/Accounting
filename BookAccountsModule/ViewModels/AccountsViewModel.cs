using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace BookAccountsModule.ViewModels
{
    public class AccountsViewModel : ViewModelBase
    {
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;
        private readonly IDialogService _showDialog;

        public AccountsViewModel(IBookAccountsEndpoint bookAccountsEndpoint, IDialogService showDialog)
        {
            _bookAccountsEndpoint = bookAccountsEndpoint;
            _showDialog = showDialog;

            EditAccountCommand = new DelegateCommand(OpenEditDialog, CanEdit);
            AddAccountCommand = new DelegateCommand(AddAccount);
            OpenBalanceCommand = new DelegateCommand(OpenBalanceCard, CanOpenBalance);
        }

        public DelegateCommand EditAccountCommand { get; private set; }
        public DelegateCommand AddAccountCommand { get; private set; }
        public DelegateCommand OpenBalanceCommand { get; private set; }

        private ObservableCollection<BookAccountModel> _accounts;
        public ObservableCollection<BookAccountModel> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }

        private BookAccountModel _selectedAccount;
        public BookAccountModel SelectedAccount
        {
            get { return _selectedAccount; }
            set 
            { 
                SetProperty(ref _selectedAccount, value);
                EditAccountCommand.RaiseCanExecuteChanged();
                OpenBalanceCommand.RaiseCanExecuteChanged();
            }
        }

        private ICollectionView _filterView;
        private string _filterKonto;
        public string FilterKonto
        {
            get { return _filterKonto; }
            set
            {
                SetProperty(ref _filterKonto, value);
                _filterView.Refresh();
            }
        }

        public async void LoadAccounts()
        {
            var list = await _bookAccountsEndpoint.GetAll();
            Accounts = new ObservableCollection<BookAccountModel>(list);
            _filterView = CollectionViewSource.GetDefaultView(Accounts);
            _filterView.Filter = o => string.IsNullOrEmpty(FilterKonto) ?
                true : (((BookAccountModel)o).Konto.Contains(FilterKonto) || ((BookAccountModel)o).Opis.ToLower().Contains(FilterKonto.ToLower()));
        }

        private bool CanEdit()
        {
            return SelectedAccount != null;
        }

        private void AddAccount()
        {
            SelectedAccount = null;
            OpenEditDialog();
        }

        private void OpenEditDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("account", SelectedAccount);
            _showDialog.ShowDialog("AddEditDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoadAccounts();
                }
            });
        }

        private bool CanOpenBalance() => SelectedAccount != null;

        private void OpenBalanceCard()
        {
            var parameters = new DialogParameters();
            parameters.Add("accountNumber", SelectedAccount.Konto);
            _showDialog.ShowDialog("BalanceCardDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
