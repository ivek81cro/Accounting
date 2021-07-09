using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace BalanceSheetModule.ViewModels
{
    public class BalanceViewModel : ViewModelBase
    {
        private readonly IBalanceSheetEndpoint _balanceSheetEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;

        public BalanceViewModel(IBalanceSheetEndpoint balanceSheetEndpoint,
                                IDialogService showDialog, 
                                IBookAccountsEndpoint bookAccountsEndpoint)
        {
            _balanceSheetEndpoint = balanceSheetEndpoint;
            _showDialog = showDialog;
            _bookAccountsEndpoint = bookAccountsEndpoint;

            OpenCardCommand = new DelegateCommand(OpenBalanceCard);
            PrintCommand = new DelegateCommand<Visual>(ShowPreview);
            SelectPeriodCommand = new DelegateCommand(LoadSelectedPeriod);

            LoadBalanceSheet();
        }

        public DelegateCommand OpenCardCommand { get; private set; }
        public DelegateCommand<Visual> PrintCommand { get; private set; }
        public DelegateCommand SelectPeriodCommand { get; private set; }

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

        private ICollectionView _filterView;
        private string _filterKonto;
        public string FilterKonto
        {
            get { return _filterKonto; }
            set
            {
                SetProperty(ref _filterKonto, value);
                _filterView.Refresh();
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
            var list = await _balanceSheetEndpoint.LoadFullBalanceSheet();
            BalanceList = new ObservableCollection<BalanceSheetModel>(list);
            FilterData();
        }

        private async void LoadSelectedPeriod()
        {
            if (DateFrom != null && DateTo != null)
            {
                List<DateTime> dates = new();
                dates.Add((DateTime)DateFrom);
                dates.Add((DateTime)DateTo);
                var list = await _balanceSheetEndpoint.LoadPeriodBalanceSheet(dates);
                BalanceList = new ObservableCollection<BalanceSheetModel>(list);
                FilterData();
            }
        }

        private void FilterData()
        {
            _filterView = CollectionViewSource.GetDefaultView(BalanceList);
            _filterView.Filter = o => string.IsNullOrEmpty(FilterKonto) ?
                true : ((BalanceSheetModel)o).Konto.StartsWith(FilterKonto);
            SumColumns();
        }

        private void SumColumns()
        {
            IEnumerable<BalanceSheetModel> list = _filterView.Cast<BalanceSheetModel>();
            SumDugovna = list.Sum(x => x.Dugovna);
            SumPotrazna = list.Sum(x => x.Potrazna);
            SumStanje = SumDugovna - SumPotrazna;
        }
        #endregion

        #region Open Balance card
        private void OpenBalanceCard()
        {
            DialogParameters parameters = new DialogParameters();
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
            DialogParameters parameters = new DialogParameters();
            parameters.Add("datagrid", v);
            parameters.Add("title", "Bilanca");
            _showDialog.Show("PrintDialogView", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                }
            });

            if (DateFrom != null)
            {
                LoadSelectedPeriod();
            }
            else
            {
                LoadBalanceSheet();
            }
        }

        private async void InsertGroupAccountSumRows()
        {
            List<BalanceSheetModel> list = _filterView.Cast<BalanceSheetModel>().ToList();
            BalanceList = new ObservableCollection<BalanceSheetModel>();
            var accounts = await _bookAccountsEndpoint.GetAll();

            for(int i = 0; i < 10; i++)
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

                        if(dugovna == 0 && potrazna == 0)
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
            FilterData();
        }
        #endregion
    }
}
