using BookJournalModule.LocalModels;
using Microsoft.Extensions.Configuration;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace BookJournalModule.Dialogs
{
    public class JournalNameDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IConfiguration _config;

        public JournalNameDialogViewModel(IConfiguration config)
        {
            _config = config;

            OpenCommand = new DelegateCommand(OpenJournal);
        }

        public DelegateCommand OpenCommand { get; private set; }

        public string Title => "Vrsta nove temeljnice";

        public event Action<IDialogResult> RequestClose;

        #region Properties
        private ObservableCollection<string> _journalNames;
        public ObservableCollection<string> JournalNames
        {
            get { return _journalNames; }
            set { SetProperty(ref _journalNames, value); }
        }

        private string _selectedJournalName;
        public string SelectedJournalName
        {
            get { return _selectedJournalName; }
            set { SetProperty(ref _selectedJournalName, value); }
        }
        #endregion

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            List<string> list = _config.GetSection("JournalNames:Names").Get<List<string>>();
            JournalNames = new ObservableCollection<string>(list);
        }

        private void OpenJournal()
        {
            var result = ButtonResult.OK;
            var p = new DialogParameters();
            p.Add("name", SelectedJournalName);

            RequestClose?.Invoke(new DialogResult(result, p));
        }
    }
}
