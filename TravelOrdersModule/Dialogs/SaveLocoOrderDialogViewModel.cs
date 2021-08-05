using AccountingUI.Core.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace TravelOrdersModule.Dialogs
{
    public class SaveLocoOrderDialogViewModel : BindableBase, IDialogAware
    {
        public SaveLocoOrderDialogViewModel()
        {
            SaveOrderCommand = new DelegateCommand(SaveOrder, CanSave);
        }

        public DelegateCommand SaveOrderCommand { get; private set; }

        public string Title => "Spremanje naloga";

        public event Action<IDialogResult> RequestClose;

        private DateTime? _calculationDate;
        public DateTime? CalculationDate
        {
            get { return _calculationDate; }
            set
            {
                SetProperty(ref _calculationDate, value);
                SaveOrderCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _paymentDate;
        public DateTime? PaymentDate
        {
            get { return _paymentDate; }
            set
            {
                SetProperty(ref _paymentDate, value);
                SaveOrderCommand.RaiseCanExecuteChanged();
            }
        }

        private LocoCalculationModel _locoCalculation;
        public LocoCalculationModel LocoCalculation
        {
            get { return _locoCalculation; }
            set { SetProperty(ref _locoCalculation, value); }
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
            LocoCalculation = parameters.GetValue<LocoCalculationModel>("calculation");
        }

        private bool CanSave()
        {
            return PaymentDate != null && CalculationDate != null;
        }

        private void SaveOrder()
        {
            //Save to database
            //Close dialog
        }
    }
}
