using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using BookJournalModule.Dialogs;
using BookJournalModule.LocalModels;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace BookJournalModule.ViewModels
{
    class JournalViewModel : ViewModelBase
    {
        private readonly IAccountingJournalEndpoint _accountingJournalEndpoint;
        private readonly IDialogService _showDialog;

        public JournalViewModel(IAccountingJournalEndpoint accountingJournalEndpoint, IDialogService showDialog)
        {
            _accountingJournalEndpoint = accountingJournalEndpoint;
            _showDialog = showDialog;

            LoadJournalCommand = new DelegateCommand(LoadJournalDetails);
            ProcessItemCommand = new DelegateCommand(ProcessJournal, CanProcess);
            DeleteJournalCommand = new DelegateCommand(DeleteJournal, CanDelete);
            SumColumnsCommand = new DelegateCommand(DeleteRow);
            LoadSavedCommand = new DelegateCommand(LoadProcessedHeaders);
            SaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            OpenCardCommand = new DelegateCommand(OpenAccountBalance);
            NewJournalCommand = new DelegateCommand(OpenNewJournal);

            LoadHeaders();
        }

        #region Delegate commands
        public DelegateCommand LoadJournalCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        public DelegateCommand DeleteJournalCommand { get; private set; }
        public DelegateCommand SumColumnsCommand { get; private set; }
        public DelegateCommand LoadSavedCommand { get; private set; }
        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand OpenCardCommand { get; private set; }
        public DelegateCommand NewJournalCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<JournalHeaders> _journalHeaders;
        public ObservableCollection<JournalHeaders> JournalHeaders
        {
            get { return _journalHeaders; }
            set { SetProperty(ref _journalHeaders, value); }
        }

        private JournalHeaders _selectedJournal;
        public JournalHeaders SelectedJournal
        {
            get { return _selectedJournal; }
            set 
            { 
                SetProperty(ref _selectedJournal, value);
                ProcessItemCommand.RaiseCanExecuteChanged();
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<AccountingJournalModel> _journalDetails;
        public ObservableCollection<AccountingJournalModel> JournalDetails
        {
            get { return _journalDetails; }
            set 
            {                 
                SetProperty(ref _journalDetails, value);
                ProcessItemCommand.RaiseCanExecuteChanged();
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        private AccountingJournalModel _selectedJournalDetail;
        public AccountingJournalModel SelectedJournalDetail
        {
            get { return _selectedJournalDetail; }
            set 
            {
                if (value != null)
                {
                    if (value.VrstaTemeljnice == null)
                    {
                        value.VrstaTemeljnice = SelectedJournal.VrstaTemeljnice;
                    }
                }
                SetProperty(ref _selectedJournalDetail, value);
            }
        }

        private decimal _sumDugovna;
        public decimal SumDugovna
        {
            get { return _sumDugovna; }
            set { SetProperty(ref _sumDugovna, value); }
        }

        private decimal _sumPotrazna;
        public decimal SumPotrazna
        {
            get { return _sumPotrazna; }
            set { SetProperty(ref _sumPotrazna, value); }
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

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        #endregion

        #region Load headers and journal details
        public async void LoadHeaders()
        {
            var list = await _accountingJournalEndpoint.LoadUnprocessedJournals();
            JournalHeaders = new ObservableCollection<JournalHeaders>();
            foreach(var item in list)
            {
                JournalHeaders.Add(
                    new JournalHeaders
                    {
                        BrojTemeljnice = item.BrojTemeljnice,
                        VrstaTemeljnice = item.VrstaTemeljnice,
                        Dugovna = item.Dugovna,
                        Potrazna = item.Potrazna,
                        Stanje = item.Dugovna - item.Potrazna
                    });
            }
        }

        private async void LoadJournalDetails()
        {
            IsLoading = true;
            var list = await _accountingJournalEndpoint
                            .LoadJournalDetails(new AccountingJournalModel
                            {
                                BrojTemeljnice = SelectedJournal.BrojTemeljnice,
                                VrstaTemeljnice = SelectedJournal.VrstaTemeljnice
                            });

            JournalDetails = new ObservableCollection<AccountingJournalModel>(list);
            DeleteJournalCommand.RaiseCanExecuteChanged();
            SumColumns();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private void DatagridLoaded()
        {
            IsLoading = false;
        }

        private void LoadProcessedHeaders()
        {
            _showDialog.ShowDialog(nameof(ProcessedJournalsDialog), null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedJournal = result.Parameters.GetValue<JournalHeaders>("selectedJournal");
                    LoadJournalDetails();
                }
            });
        }

        private void SumColumns()
        {
            SumDugovna = JournalDetails.Sum(x => x.Dugovna);
            SumPotrazna = JournalDetails.Sum(x => x.Potrazna);
            SumStanje = SumDugovna - SumPotrazna;
            SidesEqual = SumStanje == 0;
        }
        #endregion

        #region Processing to main ledger
        private bool CanProcess() => 
            JournalDetails != null && SelectedJournal != null && SelectedJournal.BrojTemeljnice == 0;
        //TODO: Revise what happens to journal with number 0 when it get's processed (separate table?? delete??) 
        private void ProcessJournal()
        {
            _showDialog.ShowDialog(nameof(EnterDateDialog), null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedJournal.DatumKnjizenja = result.Parameters.GetValue<DateTime?>("date");
                    ProcessJournalToMainLedger();
                }
            });
        }

        private async void ProcessJournalToMainLedger()
        {
            IsLoading = true;
            int journamNumber = await _accountingJournalEndpoint.LatestJournalNumber();
            foreach (var item in JournalDetails)
            {
                item.DatumKnjizenja = SelectedJournal.DatumKnjizenja;
                item.BrojTemeljnice = journamNumber + 1;
            }
            await _accountingJournalEndpoint.Post(JournalDetails.ToList());

            ResetCommandsAndView();
        }
        #endregion        

        #region Delete journal
        private bool CanDelete() => JournalDetails != null;
        private void DeleteJournal()
        {
            var parameter = new DialogParameters();
            var message = "UPOZORENJE!\nBrisanjem temeljnice morate ručno označiti vezane " +
                "dokumente kao neknjižene da bi mogli ponovo izvršiti knjiženje na temeljnicu.\n" +
                "Želite li nastaviti?";
            parameter.Add("message", message);
            _showDialog.ShowDialog("AreYouSureView", parameter, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    await _accountingJournalEndpoint.Delete(new AccountingJournalModel
                    {
                        BrojTemeljnice = SelectedJournal.BrojTemeljnice,
                        VrstaTemeljnice = SelectedJournal.VrstaTemeljnice
                    });
                    ResetCommandsAndView();
                }
            });
        }
        #endregion

        #region Save changes to journal
        private bool CanSaveChanges() => SelectedJournal != null;
        private async void SaveChanges()
        {
            IsLoading = true;
            await _accountingJournalEndpoint.Update(JournalDetails.ToList());
            ResetCommandsAndView();
        }
        #endregion

        #region Create new journal
        private void OpenNewJournal()
        {
            _showDialog.Show("NewJournalDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var journalHeader = new JournalHeaders
                    {
                        Broj = 0,
                        BrojTemeljnice = 0,
                        Valuta="HRK",
                        VrstaTemeljnice = result.Parameters.GetValue<string>("name")
                    };
                    JournalHeaders.Add(journalHeader);
                    SelectedJournal = journalHeader;

                    JournalDetails = new ObservableCollection<AccountingJournalModel>();
                }
            });
        }
        #endregion

        private async void ResetCommandsAndView()
        {
            LoadHeaders();
            JournalDetails = null;
            SelectedJournal = null;
            DeleteJournalCommand.RaiseCanExecuteChanged();
            ProcessItemCommand.RaiseCanExecuteChanged();
            SaveChangesCommand.RaiseCanExecuteChanged();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private void DeleteRow()
        {
            JournalDetails.Remove(SelectedJournalDetail);
            SumColumns();
        }

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

        public void OpenAccountsDialog()
        {
            _showDialog.ShowDialog("AccountsSelectionDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedJournalDetail.Konto = result.Parameters.GetValue<BookAccountModel>("account").Konto;
                    SelectedJournalDetail.Opis = result.Parameters.GetValue<BookAccountModel>("account").Opis;
                }
            });
        }
    }
}
