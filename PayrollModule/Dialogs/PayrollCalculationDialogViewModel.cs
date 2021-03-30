using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using PayrollModule.ServiceLocal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PayrollModule.Dialogs
{
    class PayrollCalculationDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IPayrollEndpoint _payrollEndpoint;
        private readonly IPayrollCalculation _payrollCalculation;

        public PayrollCalculationDialogViewModel(IPayrollEndpoint payrollEndpoint, IPayrollCalculation payrollCalculation)
        {
            _payrollEndpoint = payrollEndpoint;
            _payrollCalculation = payrollCalculation;

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

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        private void SaveAndCloseDialog()
        {

        }

        private void CalculatePayroll()
        {
            //TODO: get prirez for selected employee
            _payrollCalculation.Calculate(Payroll, 18);
        }
    }
}
