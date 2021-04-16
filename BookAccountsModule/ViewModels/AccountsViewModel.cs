using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using System.Collections.ObjectModel;

namespace BookAccountsModule.ViewModels
{
    public class AccountsViewModel : ViewModelBase
    {
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;

        public AccountsViewModel(IBookAccountsEndpoint bookAccountsEndpoint)
        {
            _bookAccountsEndpoint = bookAccountsEndpoint;
        }

        private ObservableCollection<BookAccountModel> _accounts;
        public ObservableCollection<BookAccountModel> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }

        public async void LoadAccounts()
        {
            var list = await _bookAccountsEndpoint.GetAll();
            Accounts = new ObservableCollection<BookAccountModel>(list);
        }
    }
}
