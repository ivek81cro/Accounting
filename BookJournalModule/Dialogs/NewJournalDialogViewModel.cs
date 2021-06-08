using AccountingUI.Core.Models;
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
    public class NewJournalDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IConfiguration _config;
        private readonly IDialogService _showDialog;

        public NewJournalDialogViewModel(IConfiguration config,
                                         IDialogService showDialog)
        {
            _config = config;
            _showDialog = showDialog;

            OpenCommand = new DelegateCommand(OpenJournal);
            AddDetailCommand = new DelegateCommand(AddNewDetail);
            OpenCardCommand = new DelegateCommand(OpenAccountBalance);
        }

        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand AddDetailCommand { get; private set; }
        public DelegateCommand OpenCardCommand { get; private set; }

        public string Title => "Nova temeljnica";

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

        private ObservableCollection<AccountingJournalModel> _journalDetails;
        public ObservableCollection<AccountingJournalModel> JournalDetails
        {
            get { return _journalDetails; }
            set
            {
                SetProperty(ref _journalDetails, value);
            }
        }

        private AccountingJournalModel _selectedJournalDetail;
        public AccountingJournalModel SelectedJournalDetail
        {
            get { return _selectedJournalDetail; }
            set { SetProperty(ref _selectedJournalDetail, value); }
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
            SelectedJournalDetail = new AccountingJournalModel
            {
                VrstaTemeljnice = SelectedJournalName,
                BrojTemeljnice = 0,
                Broj = 0,
                Valuta ="HRK"
            };
        }

        private void AddNewDetail()
        {
            if(JournalDetails == null)
            {
                JournalDetails = new ObservableCollection<AccountingJournalModel>();
            }

            if (SelectedJournalDetail != null)
            {
                JournalDetails.Add(SelectedJournalDetail);
            }

            OpenJournal();
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
                }
            });
        }
    }
}
