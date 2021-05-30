using AccountingUI.Core.Services;
using BookJournalModule.LocalModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace BookJournalModule.Dialogs
{
    public class ProcessedJournalsDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IAccountingJournalEndpoint _accountingJournalEndpoint;

        public ProcessedJournalsDialogViewModel(IAccountingJournalEndpoint accountingJournalEndpoint)
        {
            _accountingJournalEndpoint = accountingJournalEndpoint;

            OpenJournalCommand = new DelegateCommand(OpenSelectedJournal, CanOpenJournal);
        }

        public string Title => "Knižene temeljnice";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand OpenJournalCommand { get; private set; }

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
                OpenJournalCommand.RaiseCanExecuteChanged();
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
            LoadHeaders();
        }

        public async void LoadHeaders()
        {
            var list = await _accountingJournalEndpoint.LoadProcessedJournals();
            JournalHeaders = new ObservableCollection<JournalHeaders>();
            foreach (var item in list)
            {
                JournalHeaders.Add(
                    new JournalHeaders
                    {
                        BrojTemeljnice = item.BrojTemeljnice,
                        DatumKnjizenja = item.DatumKnjizenja,
                        VrstaTemeljnice = item.VrstaTemeljnice,
                        Dugovna = item.Dugovna,
                        Potrazna = item.Potrazna,
                        Stanje = item.Dugovna - item.Potrazna
                    });
            }
        }

        private void OpenSelectedJournal()
        {
            if (SelectedJournal != null)
            {
                var result = ButtonResult.OK;
                var param = new DialogParameters();
                param.Add("selectedJournal", SelectedJournal);

                RequestClose?.Invoke(new DialogResult(result, param));
            }
        }

        private bool CanOpenJournal()
        {
            return SelectedJournal != null;
        }
    }
}
