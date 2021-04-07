using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using PayrollModule.ServiceLocal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PayrollModule.Dialogs
{
    public class PayrollCalculationDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IPayrollEndpoint _payrollEndpoint;
        private readonly IPayrollCalculation _payrollCalculation;
        private readonly ICityEndpoint _cityEndpoint;

        public PayrollCalculationDialogViewModel(IPayrollEndpoint payrollEndpoint,
            IPayrollCalculation payrollCalculation,
            ICityEndpoint cityEndpoint)
        {
            _payrollEndpoint = payrollEndpoint;
            _payrollCalculation = payrollCalculation;
            _cityEndpoint = cityEndpoint;

            CalculateCommand = new DelegateCommand(CalculatePayroll);
            SaveAndCloseCommand = new DelegateCommand(SaveAndCloseDialog);
        }

        public DelegateCommand CalculateCommand { get; private set; }
        public DelegateCommand SaveAndCloseCommand { get; private set; }

        public string Title => "Izračun plaće";

        public event Action<IDialogResult> RequestClose;

        private PayrollModel _payroll;
        public PayrollModel Payroll
        {
            get { return _payroll; }
            set { SetProperty(ref _payroll, value); }
        }

        private EmployeeModel _employee;
        public EmployeeModel Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        private CityModel _city;
        public CityModel City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            Payroll = parameters.GetValue<PayrollModel>("payroll");
            Employee = parameters.GetValue<EmployeeModel>("employee");
            if (Payroll != null)
            {
                GetPayrollFromDatabase(Payroll.Oib);
            }
            else
            {
                Payroll = new PayrollModel();
            }

            City = await _cityEndpoint.GetByName(Employee.Mjesto);
        }

        private void GetPayrollFromDatabase(string oib)
        {
            _payrollEndpoint.GetByOib(oib);
        }

        private void SaveAndCloseDialog()
        {
            if (Payroll != null && !Payroll.HasErrors)
            {
                _payrollEndpoint.PostPayroll(Payroll);
                var result = ButtonResult.OK;
                var p = new DialogParameters();
                p.Add("payroll", Payroll);

                RequestClose?.Invoke(new DialogResult(result, p));
            }
        }

        private void CalculatePayroll()
        {
            _payrollCalculation.Calculate(Payroll, City.Prirez, Employee.Olaksica);
        }
    }
}
