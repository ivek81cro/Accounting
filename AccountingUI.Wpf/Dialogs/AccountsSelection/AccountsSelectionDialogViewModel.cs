using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace AccountingUI.Wpf.Dialogs.AccountsSelection
{
    public class AccountsSelectionDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;

        public AccountsSelectionDialogViewModel(IBookAccountsEndpoint bookAccountsEndpoint)
        {
            _bookAccountsEndpoint = bookAccountsEndpoint;
            SelectItemCommand = new DelegateCommand(SelectAndClose, CanReturnSelected);
        }

        public DelegateCommand SelectItemCommand { get; private set; }

        public string Title => "Odabir konta";

        private ObservableCollection<BookAccountModel> _accounts;
        public ObservableCollection<BookAccountModel> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }

        private BookAccountModel _selectedItem;
        public BookAccountModel SelectedItem
        {
            get { return _selectedItem; }
            set 
            { 
                SetProperty(ref _selectedItem, value);
                SelectItemCommand.RaiseCanExecuteChanged();
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

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            LoadAccounts();
        }

        private async void LoadAccounts()
        {
            StatusMessage = "Učitavam kontni plan...";
            var list = await _bookAccountsEndpoint.GetAll();
            StatusMessage = null;

            Accounts = new ObservableCollection<BookAccountModel>(list);

            _filterView = CollectionViewSource.GetDefaultView(Accounts);
            _filterView.Filter = o => string.IsNullOrEmpty(FilterKonto) ?
                true : (((BookAccountModel)o).Konto.Contains(FilterKonto) || ((BookAccountModel)o).Opis.ToLower().Contains(FilterKonto.ToLower()));
        }

        private bool CanReturnSelected()
        {
            return SelectedItem != null;
        }

        private void SelectAndClose()
        {
            if(SelectedItem != null)
            {
                var result = ButtonResult.OK;
                var p = new DialogParameters();
                p.Add("account", SelectedItem);

                RequestClose?.Invoke(new DialogResult(result, p));
            }
        }
    }
}
