using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using PayrollModule.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
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
            AddSupplementCommand = new DelegateCommand(OpenSupplementsDialog, CanAddSupplement);
            DeleteSupplementCommand = new DelegateCommand(DeleteSelectedSupplement, CanAddSupplement);
        }

        public DelegateCommand CalculatePayrollCommand { get; private set; }
        public DelegateCommand AddSupplementCommand { get; private set; }
        public DelegateCommand DeleteSupplementCommand { get; private set; }

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
                AddSupplementCommand.RaiseCanExecuteChanged();
                LoadSupplements();
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

        private decimal _sumOfSupplements;
        public decimal SumOfSupplements
        {
            get { return _sumOfSupplements; }
            set { SetProperty(ref _sumOfSupplements, value); }
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

        private async void LoadSupplements()
        {
            if (SelectedPayroll != null)
            {
                var suppl = await _payrollSupplementEmployeeEndpoint.GetByOib(SelectedPayroll.Oib);

                Supplements = new ObservableCollection<PayrollSupplementEmployeeModel>(suppl);
                SumOfSupplements = Supplements.Sum(x => x.Iznos);
            }

        }

        private void OpenCalculationDialog()
        {
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

        private bool CanAddSupplement()
        {
            return SelectedPayroll != null;
        }


        private void OpenSupplementsDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("oib", SelectedPayroll?.Oib);
            parameters.Add("fullname", FullName);
            _showDialog.ShowDialog(nameof(SupplementsDialog), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoadSupplements();
                }
            });
        }

        private async void DeleteSelectedSupplement()
        {
            if (SelectedSupplement != null)
            {
                await _payrollSupplementEmployeeEndpoint.DeleteSupplement(SelectedSupplement.Id);
                Supplements.Remove(SelectedSupplement);
            }
        }
    }
}
