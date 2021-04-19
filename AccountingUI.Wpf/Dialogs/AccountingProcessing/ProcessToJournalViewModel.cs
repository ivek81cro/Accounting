using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingUI.Wpf.Dialogs.AccountingProcessing
{
    public class ProcessToJournalViewModel : BindableBase, IDialogAware
    {
        private readonly IAccountPairsEndpoint _pairsEndpoint;
        private readonly IDialogService _showDialog;

        public ProcessToJournalViewModel(IAccountPairsEndpoint pairsEndpoint, IDialogService openDialog)
        {
            _pairsEndpoint = pairsEndpoint;

            AccountsLinkCommand = new DelegateCommand(AddNewPair, CanAddPair);
            _showDialog = openDialog;
        }

        public string Title => "Knjiženje na temeljnicu";

        public DelegateCommand AccountsLinkCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        private List<AccountingJournalModel> _entries;
        public List<AccountingJournalModel> Entries
        {
            get { return _entries; }
            set { SetProperty(ref _entries, value); }
        }

        private AccountingJournalModel _selectedEntry;
        public AccountingJournalModel SelectedEntry
        {
            get { return _selectedEntry; }
            set 
            { 
                SetProperty(ref _selectedEntry, value);
                AccountsLinkCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal _dugovna;
        public decimal Dugovna
        {
            get { return _dugovna; }
            set { SetProperty(ref _dugovna, value); }
        }

        private decimal _potrazna;
        public decimal Potrazna
        {
            get { return _potrazna; }
            set { SetProperty(ref _potrazna, value); }
        }

        private bool _sidesEqual;
        public bool SidesEqual
        {
            get { return _sidesEqual; }
            set { SetProperty(ref _sidesEqual, value); }
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
            Entries = parameters.GetValue<List<AccountingJournalModel>>("entries");
            SumSidesAndCompare();
        }

        private void SumSidesAndCompare()
        {
            Dugovna = Entries.Sum(x => x.Dugovna);
            Potrazna = Entries.Sum(x => x.Potrazna);

            SidesEqual = Dugovna == Potrazna;
        }

        private bool CanAddPair()
        {
            return SelectedEntry != null;
        }

        private void AddNewPair()
        {
            var param = new DialogParameters();
            param.Add("entry", SelectedEntry);
            _showDialog.ShowDialog("AccountPairDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    
                }
            });
        }
    }
}
