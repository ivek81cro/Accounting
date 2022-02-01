using AccountingUI.Core.Models;
using Microsoft.Extensions.Configuration;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PayrollModule.Dialogs
{
    public class RecalculateDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IConfiguration _config;
        private decimal _amountDeduct;

        public RecalculateDialogViewModel(IConfiguration config)
        {
            _config = config;

            CalculateCommand = new DelegateCommand(CalculatePayroll, CanCalculate);
        }

        public DelegateCommand CalculateCommand { get; private set; }

        public string Title => "Podaci za obračun";        

        private PayrollArchivePayrollModel _payroll;
        public PayrollArchivePayrollModel Payroll
        {
            get => _payroll;
            set => SetProperty(ref _payroll, value);
        }

        private PayrollArchiveHeaderModel _hoursTotal;
        public PayrollArchiveHeaderModel HoursTotal
        {
            get => _hoursTotal;
            set => SetProperty(ref _hoursTotal, value);
        }

        private PayrollHours _hours = new();
        public PayrollHours Hours
        {
            get => _hours;
            set => SetProperty(ref _hours, value);
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Payroll = parameters.GetValue<PayrollArchivePayrollModel>("paramPayroll");
            Hours = parameters.GetValue<PayrollHours>("paramHours");
        }

        private bool CanCalculate()
        {
            return !Hours.HasErrors;
        }

        private void CalculatePayroll()
        {
            if (Hours.SickDays > 0) 
            { 
                _amountDeduct += Payroll.Bruto / Hours.TotalHours * _config.GetValue<decimal>("OdbitakBolovanje1") * Hours.SickDays;
                Hours.RegularHours -= Hours.SickDays;
            }

            Payroll.Bruto -= _amountDeduct;

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
    }
}
