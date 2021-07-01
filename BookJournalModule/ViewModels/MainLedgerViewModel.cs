using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace BookJournalModule.ViewModels
{
    public class MainLedgerViewModel : ViewModelBase
    {
        private readonly IAccountingJournalEndpoint _accountingJournalEndpoint;
        private readonly IDialogService _showDialog;

        public MainLedgerViewModel(IAccountingJournalEndpoint accountingJournalEndpoint,
                                   IDialogService showDialog)
        {
            _accountingJournalEndpoint = accountingJournalEndpoint;
            _showDialog = showDialog;

            SaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            OpenCardCommand = new DelegateCommand(OpenAccountBalance);
            PrintCommand = new DelegateCommand<Visual>(ShowPreview);
            FilterDataCommand = new DelegateCommand(FilterJournal);

            LoadJournalDetails();
        }

        #region DelegateCommands
        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand OpenCardCommand { get; private set; }
        public DelegateCommand<Visual> PrintCommand { get; private set; }
        public DelegateCommand FilterDataCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<AccountingJournalModel> _journalDetails;
        public ObservableCollection<AccountingJournalModel> JournalDetails
        {
            get { return _journalDetails; }
            set { SetProperty(ref _journalDetails, value); }
        }

        private AccountingJournalModel _selectedJournalDetail;
        public AccountingJournalModel SelectedJournalDetail
        {
            get { return _selectedJournalDetail; }
            set { SetProperty(ref _selectedJournalDetail, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private decimal _sumDugovna;
        public decimal SumDugovna
        {
            get { return _sumDugovna; }
            set { SetProperty(ref _sumDugovna, value); }
        }

        private decimal __sumPotrazna;
        public decimal SumPotrazna
        {
            get { return __sumPotrazna; }
            set { SetProperty(ref __sumPotrazna, value); }
        }

        private decimal _sumStanje;
        public decimal SumStanje
        {
            get { return _sumStanje; }
            set { SetProperty(ref _sumStanje, value); }
        }

        private bool _sidesEqual;
        public bool SidesEqual
        {
            get { return _sidesEqual; }
            set { SetProperty(ref _sidesEqual, value); }
        }

        private ICollectionView _filteredView;
        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set
            {
                SetProperty(ref _dateFrom, value);
            }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set
            {
                SetProperty(ref _dateTo, value);
            }
        }
        #endregion

        #region Data load
        private async void LoadJournalDetails()
        {
            IsLoading = true;
            var list = await _accountingJournalEndpoint.LoadLedger();

            JournalDetails = new ObservableCollection<AccountingJournalModel>(list);
            FilterJournal();
            SumColumns();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private void DatagridLoaded()
        {
            IsLoading = false;
        }
        #endregion


        private bool CanSaveChanges() => SelectedJournalDetail != null;

        private void SaveChanges()
        {

        }

        private void SumColumns()
        {
            SumDugovna = JournalDetails.Sum(x => x.Dugovna);
            SumPotrazna = JournalDetails.Sum(x => x.Potrazna);
            SumStanje = SumDugovna - SumPotrazna;
            SidesEqual = SumStanje == 0;
        }

        #region Filtering datagrid
        private void FilterJournal()
        {
            _filteredView = CollectionViewSource.GetDefaultView(JournalDetails);
            _filteredView.Filter = o => FilterData((AccountingJournalModel)o);
            SumColumns();
        }

        private bool FilterData(AccountingJournalModel o)
        {
            if (DateFrom != null || DateTo != null)
            {
                return o.DatumKnjizenja >= DateFrom && o.DatumKnjizenja <= DateTo;
            }

            return true;
        }
        #endregion

        private void OpenAccountBalance()
        {
            var parameters = new DialogParameters();
            parameters.Add("accountNumber", SelectedJournalDetail.Konto);
            _showDialog.Show("BalanceCardDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }

        private void ShowPreview(Visual v)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("datagrid", v);
            parameters.Add("title", "Dnevnik knjiženja");
            _showDialog.ShowDialog("PrintDialogView", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
