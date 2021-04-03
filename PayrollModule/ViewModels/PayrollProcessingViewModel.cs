using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using AutoMapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PayrollModule.ViewModels
{
    class PayrollProcessingViewModel : ViewModelBase
    {
        private IMapper _mapper;
        private IPayrollEndpoint _payrollEndpoint;

        public PayrollProcessingViewModel(IMapper mapper, 
            IPayrollEndpoint payrollEndpoint)
        {
            _mapper = mapper;
            _payrollEndpoint = payrollEndpoint;
        }

        private bool _ifPayrolls;
        public bool IfPayrolls
        {
            get { return _ifPayrolls; }
            set { SetProperty(ref _ifPayrolls, value); }
        }

        private ObservableCollection<PayrollCalculationModel> _payrollCalculations;
        public ObservableCollection<PayrollCalculationModel> PayrollCalculations
        {
            get { return _payrollCalculations; }
            set { SetProperty(ref _payrollCalculations, value); }
        }

        private PayrollCalculationModel _filterPayrollCalc;
        public PayrollCalculationModel FilterPayrollCalc
        {
            get { return _filterPayrollCalc; }
            set { SetProperty(ref _filterPayrollCalc, value); }
        }

        private bool _selectAllPayrolls;
        public bool SelectAllPayrolls
        {
            get { return _selectAllPayrolls; }
            set { SetProperty(ref _selectAllPayrolls, value); }
        }

        public async void LoadPayrolls()
        {
            var payrollList = await _payrollEndpoint.GetAll();
            var cPayrollList = _mapper.Map<List<PayrollCalculationModel>>(payrollList);
            PayrollCalculations = new ObservableCollection<PayrollCalculationModel>(cPayrollList);
        }
    }
}
