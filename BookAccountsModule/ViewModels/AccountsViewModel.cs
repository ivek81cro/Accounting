using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

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
    }
}
