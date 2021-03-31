using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using PayrollModule.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace PayrollModule.ViewModels
{
    public class PayrollViewModel : ViewModelBase
    {
        private IPayrollEndpoint _payrollEndpoint;
        private IEmployeeEndpoint _employeeEndpoint;
        private IPayrollSupplementEmployeeEndpoint _payrollSupplementEmployeeEndpoint;
        private IPayrollSupplementEndpoint _payrollSupplementEndpoint;
        private IRegionManager _regionManager;
        private IDialogService _showDialog;

        public PayrollViewModel(IPayrollEndpoint payrollEndpoint,
            IPayrollSupplementEmployeeEndpoint payrollSupplementEmployeeEndpoint,
            IPayrollSupplementEndpoint payrollSupplementEndpoint,
            IRegionManager regionManager,
            IDialogService showDialog, 
            IEmployeeEndpoint employeeEndpoint)
        {
            _payrollEndpoint = payrollEndpoint;
            _employeeEndpoint = employeeEndpoint;
            _payrollSupplementEmployeeEndpoint = payrollSupplementEmployeeEndpoint;
            _payrollSupplementEndpoint = payrollSupplementEndpoint;
            _regionManager = regionManager;
            _showDialog = showDialog;

            CalculatePayrollCommand = new DelegateCommand(OpenCalculationDialog);
        }

        public DelegateCommand CalculatePayrollCommand { get; private set; }

        private ObservableCollection<PayrollModel> _payrolls;
        public ObservableCollection<PayrollModel> Payrolls
        {
            get { return _payrolls; }
            set { SetProperty(ref _payrolls, value); }
        }

        private PayrollModel _selectedPayroll;
        public PayrollModel SelectedPayroll
        {
            get { return _selectedPayroll; }
            set 
            { 
                SetProperty(ref _selectedPayroll, value);
                RaisePropertyChanged(nameof(FullName));
            }
        }

        private ObservableCollection<PayrollSupplementEmployeeModel> _supplements;
        public ObservableCollection<PayrollSupplementEmployeeModel> Supplements
        {
            get { return _supplements; }
            set { SetProperty(ref _supplements, value); }
        }

        private PayrollSupplementEmployeeModel _selectedSupplement;
        public PayrollSupplementEmployeeModel SelectedSupplement
        {
            get { return _selectedSupplement; }
            set { SetProperty(ref _selectedSupplement, value); }
        }

        private List<EmployeeModel> _employees;
        public List<EmployeeModel> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private ICollectionView _payrollsView;
        private string _filterPayrolls;
        public string FilterPartners
        {
            get { return _filterPayrolls; }
            set
            {
                SetProperty(ref _filterPayrolls, value.ToUpper());
                _payrollsView.Refresh();
            }
        }

        private string _fullName;
        public string FullName
        {
            get { return $"{SelectedPayroll?.Ime}  {SelectedPayroll?.Prezime}"; }
            set { SetProperty(ref _fullName, value); }
        }

        public async void LoadPayrolls()
        {
            var payrollList = await _payrollEndpoint.GetAll();
            Payrolls = new ObservableCollection<PayrollModel>(payrollList);
            _payrollsView = CollectionViewSource.GetDefaultView(Payrolls);
            _payrollsView.Filter = o => string.IsNullOrEmpty(FilterPartners) ? true : ((PayrollModel)o).Prezime.Contains(FilterPartners);

            Employees = await _employeeEndpoint.GetAll();
        }

        private void OpenCalculationDialog()
        {
            //TODO: Injection of IConfiguration now working in Dialogs.
            var parameters = new DialogParameters();
            parameters.Add("payroll", SelectedPayroll);
            parameters.Add("employee", Employees.Where(x => x.Oib == SelectedPayroll.Oib).FirstOrDefault());
            _showDialog.ShowDialog(nameof(PayrollCalculationDialog), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    PayrollModel payroll = result.Parameters.GetValue<PayrollModel>("payroll");
                    _payrollEndpoint.PostPayroll(payroll);
                    LoadPayrolls();
                }
            });
        }
    }
}
