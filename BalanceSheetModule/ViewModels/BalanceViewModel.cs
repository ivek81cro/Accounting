﻿using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BalanceSheetModule.ViewModels
{
    public class BalanceViewModel : ViewModelBase
    {
        private readonly IBalanceSheetEndpoint _balanceSheetEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;

        private string _title;

        public BalanceViewModel(IBalanceSheetEndpoint balanceSheetEndpoint,
                                IDialogService showDialog,
                                IBookAccountsEndpoint bookAccountsEndpoint)
        {
            _balanceSheetEndpoint = balanceSheetEndpoint;
            _showDialog = showDialog;
            _bookAccountsEndpoint = bookAccountsEndpoint;

            OpenCardCommand = new DelegateCommand(OpenBalanceCard);
            PrintCommand = new DelegateCommand<Visual>(ShowPreview);
            SelectPeriodCommand = new DelegateCommand(LoadBalanceSheet);
            TransferToNextYearCommand = new DelegateCommand(TransferBalanceToNextYearAsync);

            LoadBalanceSheet();
        }

        public DelegateCommand OpenCardCommand { get; private set; }
        public DelegateCommand<Visual> PrintCommand { get; private set; }
        public DelegateCommand SelectPeriodCommand { get; private set; }
        public DelegateCommand TransferToNextYearCommand { get; private set; }

        #region Properties
        private ObservableCollection<BalanceSheetModel> _balanceList;
        public ObservableCollection<BalanceSheetModel> BalanceList
        {
            get { return _balanceList; }
            set { SetProperty(ref _balanceList, value); }
        }

        private BalanceSheetModel _selectedBalanceItem;
        public BalanceSheetModel SelectedBalanceItem
        {
            get { return _selectedBalanceItem; }
            set { SetProperty(ref _selectedBalanceItem, value); }
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

        private ICollectionView _filterDataGrid;
        private string _filterKonto;
        public string FilterKonto
        {
            get { return _filterKonto; }
            set
            {
                SetProperty(ref _filterKonto, value);
                FilterDataGridView();
                SumColumns();
            }
        }

        private string _filterName;
        public string FilterName
        {
            get { return _filterName; }
            set
            {
                SetProperty(ref _filterName, value);
                FilterDataGridView();
                SumColumns();
            }
        }

        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }
        #endregion

        #region Loading and filtering data
        private async void LoadBalanceSheet()
        {
            if (DateFrom == null && DateTo == null)
            {
                await LoadAllData();
            }
            else
            {
                await LoadSelectedData();
            }
        }

        private async Task LoadAllData()
        {
            var list = await _balanceSheetEndpoint.LoadFullBalanceSheet();
            BalanceList = new ObservableCollection<BalanceSheetModel>(list);
            FilterDataGridView();
            _title = "Bilanca - sve";
        }

        private async Task LoadSelectedData()
        {
            if (DateFrom != null && DateTo != null)
            {
                List<DateTime> dates = new();
                dates.Add((DateTime)DateFrom);
                dates.Add((DateTime)DateTo);
                var list = await _balanceSheetEndpoint.LoadPeriodBalanceSheet(dates);
                BalanceList = new ObservableCollection<BalanceSheetModel>(list);
                FilterDataGridView();
                _title = $"Bilanca za razdoblje {DateFrom.Value.Date.ToShortDateString()} do {DateTo.Value.Date.ToShortDateString()}";
            }
        }

        private void FilterDataGridView()
        {
            _filterDataGrid = CollectionViewSource.GetDefaultView(BalanceList);
            _filterDataGrid.Filter = o => FilterData((BalanceSheetModel)o);
            SumColumns();
        }

        private bool FilterData(BalanceSheetModel o)
        {
            if(FilterKonto != null && FilterName == null)
            {
                return o.Konto.StartsWith(FilterKonto);
            }
            else if(FilterKonto == null && FilterName != null)
            {
                return o.Opis.ToUpper().Contains(FilterName.ToUpper());
            }
            else if(FilterKonto !=null && FilterName != null)
            {
                return o.Opis.ToUpper().Contains(FilterName.ToUpper()) && o.Konto.StartsWith(FilterKonto);
            }
            else
            {
                return true;
            }
        }

        private void SumColumns()
        {
            IEnumerable<BalanceSheetModel> list = _filterDataGrid.Cast<BalanceSheetModel>();
            SumDugovna = list.Sum(x => x.Dugovna);
            SumPotrazna = list.Sum(x => x.Potrazna);
            SumStanje = SumDugovna - SumPotrazna;
        }
        #endregion

        #region Open Balance card
        private void OpenBalanceCard()
        {
            DialogParameters parameters = new();
            parameters.Add("accountNumber", SelectedBalanceItem.Konto);
            _showDialog.Show("BalanceCardDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
        #endregion

        #region Print preview building
        private void ShowPreview(Visual v)
        {
            InsertGroupAccountSumRows();
            DialogParameters parameters = new();
            parameters.Add("datagrid", v);
            parameters.Add("title", _title);
            _showDialog.Show("PrintDialogView", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                }
            });

            LoadBalanceSheet();
        }

        private async void InsertGroupAccountSumRows()
        {
            List<BalanceSheetModel> list = _filterDataGrid.Cast<BalanceSheetModel>().ToList();
            BalanceList = new ObservableCollection<BalanceSheetModel>();
            var accounts = await _bookAccountsEndpoint.GetAll();

            for (int i = 0; i < 10; i++)
            {
                string par = i.ToString();
                decimal potrazna = list.Where(x => x.Konto.StartsWith(par)).Sum(y => y.Potrazna);
                decimal dugovna = list.Where(x => x.Konto.StartsWith(par)).Sum(y => y.Dugovna);
                decimal stanje = list.Where(x => x.Konto.StartsWith(par)).Sum(y => y.Stanje);
                if (dugovna == 0 && potrazna == 0 && stanje == 0)
                {
                    continue;
                }
                else
                {
                    BalanceList.Add(new BalanceSheetModel
                    {
                        Konto = i.ToString(),
                        Dugovna = dugovna,
                        Potrazna = potrazna,
                        Stanje = stanje,
                        Opis = accounts.FirstOrDefault(x => x.Konto == par).Opis
                    });
                }
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        string param = i.ToString() + j.ToString() + k.ToString();
                        potrazna = list.Where(x => x.Konto.StartsWith(param)).Sum(y => y.Potrazna);
                        dugovna = list.Where(x => x.Konto.StartsWith(param)).Sum(y => y.Dugovna);
                        stanje = list.Where(x => x.Konto.StartsWith(param)).Sum(y => y.Stanje);

                        BalanceList.AddRange(list.Where(x => x.Konto.StartsWith(param) && x.Konto.Length > 3));

                        if (dugovna == 0 && potrazna == 0)
                        {
                            continue;
                        }
                        else
                        {
                            BalanceList.Add(new BalanceSheetModel
                            {
                                Konto = param,
                                Dugovna = dugovna,
                                Potrazna = potrazna,
                                Stanje = stanje,
                                Opis = accounts.FirstOrDefault(x => x.Konto == param).Opis
                            });
                        }
                    }
                }
            }
            FilterDataGridView();
        }
        #endregion

        private async void TransferBalanceToNextYearAsync()
        {
            List<AccountingJournalModel> startJournal = new();
            foreach (var item in BalanceList)
            {
                if (item.Stanje != 0
                    && !item.Konto.StartsWith('3')
                    && !item.Konto.StartsWith('4')
                    && !item.Konto.StartsWith('5')
                    && !item.Konto.StartsWith('7')
                    && !item.Konto.StartsWith('8'))
                {                    
                    startJournal.Add(new AccountingJournalModel
                    {
                        Opis = item.Opis,
                        Dokument = "Početno stanje",
                        Broj=0,
                        Konto=item.Konto,
                        Datum=new DateTime(DateTime.Today.Year, 01,01),
                        Valuta="EUR",
                        Dugovna = item.Stanje > 0?item.Stanje:0,
                        Potrazna = item.Stanje < 0?item.Stanje*(-1):0,
                        VrstaTemeljnice="Pocetno stanje"
                    });
                }
            }

            SendToProcessingDialog(startJournal);
        }

        private void SendToProcessingDialog(List<AccountingJournalModel> startJournal)
        {
            var parameters = new DialogParameters
            {
                { "entries", startJournal }
            };
            _showDialog.ShowDialog("ProcessToJournal", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
