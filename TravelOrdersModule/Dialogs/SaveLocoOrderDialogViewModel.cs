using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelOrdersModule.Dialogs
{
    public class SaveLocoOrderDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ITravelOrdersEndpoint _travelOrdersEndpoint;

        public SaveLocoOrderDialogViewModel(ITravelOrdersEndpoint travelOrdersEndpoint)
        {
            _travelOrdersEndpoint = travelOrdersEndpoint;

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

        private List<LocoOrderModel> _locoOrders;
        public List<LocoOrderModel> LocoOrders
        {
            get { return _locoOrders; }
            set { SetProperty(ref _locoOrders, value); }
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
            LocoOrders = new();
            LocoOrders = parameters.GetValue<List<LocoOrderModel>>("orders");
        }

        private bool CanSave()
        {
            return PaymentDate != null && CalculationDate != null;
        }

        private async void SaveOrder()
        {
            LocoCalculation.DateOfCalculation = CalculationDate;
            LocoCalculation.DateOfPayment = PaymentDate;
            TravelOrdersLocoModel order = new TravelOrdersLocoModel
            {
                LocoCalculation = LocoCalculation,
                LocoOrders = LocoOrders
            };

            bool isPosted = await _travelOrdersEndpoint.PostLocoTravel(order);
            if (isPosted)
            {
                var result = ButtonResult.OK;
                var p = new DialogParameters();
                RequestClose?.Invoke(new DialogResult(result, p));
            }
        }
    }
}
