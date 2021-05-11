using AccountingUI.Core.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AccountingUI.Wpf.Dialogs.AccountingProcessing
{
    public class ProcessToJournalViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService _showDialog;

        public ProcessToJournalViewModel(IDialogService openDialog)
        {
            _showDialog = openDialog;

            AccountsLinkCommand = new DelegateCommand(AddNewPair, CanAddPair);
            AddRowCommand = new DelegateCommand(AddRow);
            DeleteRowCommand = new DelegateCommand(DeleteRow);
        }

        public string Title => "Knjiženje na temeljnicu";

        public DelegateCommand AccountsLinkCommand { get; private set; }
        public DelegateCommand AddRowCommand { get; private set; }
        public DelegateCommand DeleteRowCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        private ObservableCollection<AccountingJournalModel> _entries;
        public ObservableCollection<AccountingJournalModel> Entries
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

        private string _bookName;
        public string BookName
        {
            get { return _bookName; }
            set { SetProperty(ref _bookName, value); }
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
            var list = parameters.GetValue<List<AccountingJournalModel>>("entries");
            if(list.Count > 0)
            {
                BookName = list[0].VrstaTemeljnice;
            }
            Entries = new ObservableCollection<AccountingJournalModel>(list);
            SumSidesAndCompare();
        }

        public void SumSidesAndCompare()
        {
            Dugovna = Entries.Sum(x => x.Dugovna);
            Potrazna = Entries.Sum(x => x.Potrazna);

            SidesEqual = Dugovna == Potrazna;
        }

        private bool CanAddPair()
        {
            return SelectedEntry != null && SelectedEntry.Dokument!=null;
        }

        private void AddNewPair()
        {
            var param = new DialogParameters();
            param.Add("entry", SelectedEntry);
            _showDialog.ShowDialog("AccountPairDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    foreach(var ent in Entries)
                    {
                        if (ent.Konto == null || ent.Konto.Length < 3)
                        {
                            ent.Konto = result.Parameters.GetValue<string>("account");
                        }
                    }                    
                }
            });
        }

        private void AddRow()
        {
            SelectedEntry = new AccountingJournalModel
            {
                VrstaTemeljnice = BookName
            };
            Entries.Add(SelectedEntry);
            SelectedEntry.Datum = null;
            SelectedEntry.Konto = null;
            SelectedEntry.Dokument = null;
            SelectedEntry.Opis = null;
            SumSidesAndCompare();
        }

        private void DeleteRow()
        {
            Entries.Remove(SelectedEntry);
            SumSidesAndCompare();
        }

        public void OpenAccountsDialog()
        {
            _showDialog.ShowDialog("AccountsSelectionDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedEntry.Konto = result.Parameters.GetValue<BookAccountModel>("account").Konto;
                }
            });
        }
    }
}
