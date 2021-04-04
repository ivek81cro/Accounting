using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using AutoMapper;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace PayrollModule.ViewModels
{
    class PayrollProcessingViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly IPayrollEndpoint _payrollEndpoint;
        private readonly IPayrollSupplementEmployeeEndpoint _supplementEndpoint;

        public PayrollProcessingViewModel(IMapper mapper,
            IPayrollEndpoint payrollEndpoint,
            IPayrollSupplementEmployeeEndpoint supplementEndpoint)
        {
            _mapper = mapper;
            _payrollEndpoint = payrollEndpoint;
            _supplementEndpoint = supplementEndpoint;

            CalculatePayrollCommand = new DelegateCommand(Calculate, CanCalculate);
        }

        private bool CanCalculate()
        {
            if(!IfPayrolls && !IfSupplements)
            {
                return false;
            }

            if(Accounting != null)
            {
                if(Accounting.DatumDo == null 
                    || Accounting.DatumOd==null 
                    || Accounting.DatumObracuna==null 
                    || Accounting.SatiRada == 0)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private void Calculate()
        {

        }

        public DelegateCommand CalculatePayrollCommand { get; set; }

        #region Payrolls

        private bool _ifPayrolls;
        public bool IfPayrolls
        {
            get { return _ifPayrolls; }
            set
            {
                SetProperty(ref _ifPayrolls, value);
                if (value)
                {
                    LoadPayrolls();
                }
                else
                {
                    PayrollCalculations = null;
                    SelectAllPayrolls = false;
                    FilterPayrolls = "";
                }
                CalculatePayrollCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<PayrollCalculationModel> _payrollCalculations;
        public ObservableCollection<PayrollCalculationModel> PayrollCalculations
        {
            get { return _payrollCalculations; }
            set { SetProperty(ref _payrollCalculations, value); }
        }

        private bool _selectAllPayrolls;
        public bool SelectAllPayrolls
        {
            get { return _selectAllPayrolls; }
            set 
            { 
                SetProperty(ref _selectAllPayrolls, value);
                if (PayrollCalculations != null)
                {
                    foreach (var item in PayrollCalculations)
                    {
                        item.IsChecked = value;
                    }
                }
            }
        }

        private ICollectionView _payrollsView;
        private string _filterPayrolls;
        public string FilterPayrolls
        {
            get { return _filterPayrolls; }
            set
            {
                SetProperty(ref _filterPayrolls, value);
                _payrollsView.Refresh();
            }
        }

        public async void LoadPayrolls()
        {
            var payrollList = await _payrollEndpoint.GetAll();
            var cPayrollList = _mapper.Map<List<PayrollCalculationModel>>(payrollList);
            PayrollCalculations = new ObservableCollection<PayrollCalculationModel>(cPayrollList);

            _payrollsView = CollectionViewSource.GetDefaultView(PayrollCalculations);
            _payrollsView.Filter = o => string.IsNullOrEmpty(FilterPayrolls) ?
                true : ((PayrollCalculationModel)o).Prezime.ToLower().Contains(FilterPayrolls.ToLower());
        }


        #endregion

        #region Supplements

        private bool _ifSupplements;
        public bool IfSupplements
        {
            get { return _ifSupplements; }
            set
            {
                SetProperty(ref _ifSupplements, value);
                if (value)
                {
                    LoadSupplements();
                }
                else
                {
                    SupplementSelectDisplay = null;
                    SelectAllSupplements = false;
                    FilterSupplementsDisplay = "";
                }
                CalculatePayrollCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _selectAllSupplements;
        public bool SelectAllSupplements
        {
            get { return _selectAllSupplements; }
            set 
            { 
                SetProperty(ref _selectAllSupplements, value);
                if (SupplementSelectDisplay != null)
                {
                    foreach (var item in SupplementSelectDisplay)
                    {
                        item.IsChecked = value;
                    }
                }
            }
        }

        private ObservableCollection<PayrollSupplementCalculationModel> _supplementCalculations;
        public ObservableCollection<PayrollSupplementCalculationModel> SupplementCalculations
        {
            get { return _supplementCalculations; }
            set { SetProperty(ref _supplementCalculations, value); }
        }

        private ObservableCollection<PayrollSupplementEmployeeModel> _supplementSelectDisplay;
        public ObservableCollection<PayrollSupplementEmployeeModel> SupplementSelectDisplay
        {
            get { return _supplementSelectDisplay; }
            set { SetProperty(ref _supplementSelectDisplay, value); }
        }

        private ICollectionView _supplementsView;
        private string _filterSupplementsDisplay;
        public string FilterSupplementsDisplay
        {
            get { return _filterSupplementsDisplay; }
            set
            {
                SetProperty(ref _filterSupplementsDisplay, value);
                _supplementsView.Refresh();
            }
        }

        private async void LoadSupplements()
        {
            var supplementList = await _supplementEndpoint.GetAll();
            var cSupplementList = _mapper.Map<List<PayrollSupplementCalculationModel>>(supplementList);
            SupplementCalculations = new ObservableCollection<PayrollSupplementCalculationModel>(cSupplementList);

            var distinctSuppl = await _supplementEndpoint.GetDistinct();
            SupplementSelectDisplay = new ObservableCollection<PayrollSupplementEmployeeModel>(distinctSuppl);

            _supplementsView = CollectionViewSource.GetDefaultView(SupplementSelectDisplay);
            _supplementsView.Filter = o => string.IsNullOrEmpty(FilterSupplementsDisplay) ?
                true : ((PayrollSupplementEmployeeModel)o).Naziv.ToLower().Contains(FilterSupplementsDisplay.ToLower());
        }
        #endregion

        #region PayrollAccounting

        private PayrollAccountingModel _accounting = new();
        public PayrollAccountingModel Accounting
        {
            get { return _accounting; }
            set 
            { 
                SetProperty(ref _accounting, value);
            }
        }

        #endregion
    }
}
