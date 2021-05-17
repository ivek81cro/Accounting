using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using BookJournalModule.LocalModels;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BookJournalModule.ViewModels
{
    class JournalViewModel : ViewModelBase
    {
        private readonly IAccountingJournalEndpoint _accountingJournalEndpoint;

        public JournalViewModel(IAccountingJournalEndpoint accountingJournalEndpoint)
        {
            _accountingJournalEndpoint = accountingJournalEndpoint;

            LoadJournalCommand = new DelegateCommand(LoadJournalDetails);
        }

        public DelegateCommand LoadJournalCommand { get; private set; }

        private List<JournalHeaders> _unprocessedJournals;
        public List<JournalHeaders> UnprocessedJournals
        {
            get { return _unprocessedJournals; }
            set { SetProperty(ref _unprocessedJournals, value); }
        }

        private JournalHeaders _selectedJournal;
        public JournalHeaders SelectedJournal
        {
            get { return _selectedJournal; }
            set { SetProperty(ref _selectedJournal, value); }
        }

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

        public async void LoadData()
        {
            var list = await _accountingJournalEndpoint.LoadUnprocessedJournals();
            UnprocessedJournals = new List<JournalHeaders>();
            foreach(var item in list)
            {
                UnprocessedJournals.Add(
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
            var list = await _accountingJournalEndpoint
                            .LoadJournalDetails(new AccountingJournalModel
                            {
                                BrojTemeljnice = SelectedJournal.BrojTemeljnice,
                                VrstaTemeljnice = SelectedJournal.VrstaTemeljnice
                            });

            JournalDetails = new ObservableCollection<AccountingJournalModel>(list);
        }
    }
}
