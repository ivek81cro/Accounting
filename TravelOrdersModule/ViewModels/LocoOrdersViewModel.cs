using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;

namespace TravelOrdersModule.ViewModels
{
    public class LocoOrdersViewModel : ViewModelBase
    {
        private readonly IDialogService _showDialog;
        private readonly ITravelOrdersEndpoint _travelOrdersEndpoint;
        private readonly IEmployeeEndpoint _employeeEndpoint;

        public LocoOrdersViewModel(IDialogService showDialog,
                                   ITravelOrdersEndpoint travelOrdersEndpoint,
                                   IEmployeeEndpoint employeeEndpoint)
        {
            _showDialog = showDialog;
            _travelOrdersEndpoint = travelOrdersEndpoint;
            _employeeEndpoint = employeeEndpoint;

            GenerateList = new DelegateCommand(GenerateOrders);
            EditOrderCommand = new DelegateCommand(EditOrder, CanEditOrder);
            DeleteOrderCommand = new DelegateCommand(DeleteOrder, CanDeleteOrder);

            InitialDataLoad();
        }

        public DelegateCommand GenerateList { get; private set; }
        public DelegateCommand EditOrderCommand { get; private set; }
        public DelegateCommand DeleteOrderCommand { get; private set; }

        private List<LocoCalculationModel> _locoCalculation;
        public List<LocoCalculationModel> LocoCalculation
        {
            get { return _locoCalculation; }
            set { SetProperty(ref _locoCalculation, value); }
        }

        private LocoCalculationModel _selectedCalculation;
        public LocoCalculationModel SelectedCalculation
        {
            get { return _selectedCalculation; }
            set
            {
                SetProperty(ref _selectedCalculation, value);
                EditOrderCommand.RaiseCanExecuteChanged();
                DeleteOrderCommand.RaiseCanExecuteChanged();
            }
        }

        private async void InitialDataLoad()
        {
            LocoCalculation = await _travelOrdersEndpoint.GetLocoCalculations();
        }

        private void GenerateOrders()
        {
            DialogParameters param = new DialogParameters();
            if (SelectedCalculation != null)
            {
                param.Add("orderCalc", SelectedCalculation);
            }

            _showDialog.ShowDialog("GeneratorDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    InitialDataLoad();
                }
            });
        }

        private bool CanEditOrder()
        {
            return SelectedCalculation != null;
        }

        private void EditOrder()
        {
            GenerateOrders();
        }

        private async void DeleteOrder()
        {
            bool result = await _travelOrdersEndpoint.DeleteOrder(SelectedCalculation.Id);
            if (result)
            {
                InitialDataLoad();
            }
        }

        private bool CanDeleteOrder()
        {
            return SelectedCalculation != null;
        }
    }
}
