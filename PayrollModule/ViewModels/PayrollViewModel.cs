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

            CalculatePayrollCommand = new DelegateCommand(OpenCalculationDialog, CanCalculate);
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
                DeleteSupplementCommand.RaiseCanExecuteChanged();
                CalculatePayrollCommand.RaiseCanExecuteChanged();
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
        public string FilterPayroll
        {
            get { return _filterPayrolls; }
            set
            {
                SetProperty(ref _filterPayrolls, value);
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
            get 
            { 
                var result = $"{SelectedPayroll?.Ime}  {SelectedPayroll?.Prezime}";
                if (result == "  UKUPNO:")
                {
                    SumOfSupplements = 0;
                    return "";
                }

                return result;
            }
            set { SetProperty(ref _fullName, value); }
        }

        private PayrollModel _payrollSum = new PayrollModel();
        public PayrollModel PayrollSum
        {
            get { return _payrollSum; }
            set { SetProperty(ref _payrollSum, value); }
        }

        public async void LoadPayrolls()
        {
            List<PayrollModel> payrollList = await _payrollEndpoint.GetAll();
            payrollList.Sort((x, y)=> (x.Prezime).CompareTo(y.Prezime));
            Employees = await _employeeEndpoint.GetAll();

            PayrollSum.Prezime = "UKUPNO:";
            PayrollSum.Bruto = payrollList.Sum(x => x.Bruto);
            PayrollSum.Mio1 = payrollList.Sum(x => x.Mio1);
            PayrollSum.Mio2 = payrollList.Sum(x => x.Mio2);
            PayrollSum.Odbitak = payrollList.Sum(x => x.Odbitak);
            PayrollSum.PoreznaOsnovica = payrollList.Sum(x => x.PoreznaOsnovica);
            PayrollSum.PoreznaStopa1 = payrollList.Sum(x => x.PoreznaStopa1);
            PayrollSum.PoreznaStopa2 = payrollList.Sum(x => x.PoreznaStopa2);
            PayrollSum.Prirez = payrollList.Sum(x => x.Prirez);
            PayrollSum.UkupnoPorez = payrollList.Sum(x => x.UkupnoPorez);
            PayrollSum.UkupnoPorezPrirez = payrollList.Sum(x => x.UkupnoPorezPrirez);
            PayrollSum.DoprinosZdravstvo = payrollList.Sum(x => x.DoprinosZdravstvo);
            PayrollSum.Dohodak = payrollList.Sum(x => x.Dohodak);
            PayrollSum.Neto = payrollList.Sum(x => x.Neto);

            payrollList.Add(PayrollSum);

            Payrolls = new ObservableCollection<PayrollModel>(payrollList);
            _payrollsView = CollectionViewSource.GetDefaultView(Payrolls);
            _payrollsView.Filter = o => string.IsNullOrEmpty(FilterPayroll) ? 
                true : ((PayrollModel)o).Prezime.ToLower().Contains(FilterPayroll.ToLower());
        }

        private async void LoadSupplements()
        {
            if (SelectedPayroll != null)
            {
                if (SelectedPayroll.Oib == null)
                {
                    Supplements = null;
                }
                else
                {
                    var suppl = await _payrollSupplementEmployeeEndpoint.GetByOib(SelectedPayroll.Oib);

                    Supplements = new ObservableCollection<PayrollSupplementEmployeeModel>(suppl);
                    SumOfSupplements = Supplements.Sum(x => x.Iznos);
                }
            }

        }

        private bool CanCalculate()
        {
            return SelectedPayroll != null && SelectedPayroll.Oib != null;
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
                    SaveNewCalculation(payroll);
                }
            });
        }

        private async void SaveNewCalculation(PayrollModel payroll)
        {
            await _payrollEndpoint.PostPayroll(payroll);
            LoadPayrolls();
        }

        private bool CanAddSupplement()
        {
            return SelectedPayroll != null && SelectedPayroll.Oib != null;
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
