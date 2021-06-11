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

namespace BalanceSheetModule.ViewModels
{
    public class BalanceViewModel : ViewModelBase
    {
        private readonly IBalanceSheetEndpoint _balanceSheetEndpoint;
        private readonly IDialogService _showDialog;

        public BalanceViewModel(IBalanceSheetEndpoint balanceSheetEndpoint,
                                IDialogService showDialog)
        {
            _balanceSheetEndpoint = balanceSheetEndpoint;
            _showDialog = showDialog;

            OpenCardCommand = new DelegateCommand(OpenBalanceCard);
            
            LoadBalanceSheet();
        }

        public DelegateCommand OpenCardCommand { get; private set; }

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

        private async void LoadBalanceSheet()
        {
            var list = await _balanceSheetEndpoint.LoadFullBalanceSheet();
            BalanceList = new ObservableCollection<BalanceSheetModel>(list);
            _filterView = CollectionViewSource.GetDefaultView(BalanceList);
            _filterView.Filter = o => string.IsNullOrEmpty(FilterKonto) ?
                true : ((BalanceSheetModel)o).Konto.StartsWith(FilterKonto);
            SumColumns();
        }

        private void SumColumns()
        {
            var list = _filterView.Cast<BalanceSheetModel>();
            SumDugovna = list.Sum(x => x.Dugovna);
            SumPotrazna = list.Sum(x => x.Potrazna);
            SumStanje = SumDugovna - SumPotrazna;
        }

        private void OpenBalanceCard()
        {
            var parameters = new DialogParameters();
            parameters.Add("accountNumber", SelectedBalanceItem.Konto);
            _showDialog.Show("BalanceCardDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
