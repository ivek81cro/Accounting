using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TravelOrdersModule.LocalModel;

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

            InitialDataLoad();
        }

        public DelegateCommand GenerateList { get; private set; }

        private List<LocoCalculationModel> _locoCalculation;
        public List<LocoCalculationModel> LocoCalculation
        {
            get { return _locoCalculation; }
            set { SetProperty(ref _locoCalculation, value); }
        }

        private ObservableCollection<LocoCalculationListModel> _locoCalculationsList = new();
        public ObservableCollection<LocoCalculationListModel> LocoCalculationsList
        {
            get { return _locoCalculationsList; }
            set { SetProperty(ref _locoCalculationsList, value); }
        }

        private async void InitialDataLoad()
        {
            LocoCalculation = await _travelOrdersEndpoint.GetLocoCalculations();
            var employees = await _employeeEndpoint.GetAll();
            foreach(var item in LocoCalculation)
            {
                var employee = employees.Where(x => x.Id == item.EmployeeId).FirstOrDefault();
                LocoCalculationsList.Add(
                    new LocoCalculationListModel
                    {
                        EmployeeName = employee.Ime + " " + employee.Prezime,
                        EmployeeOib = employee.Oib,
                        EmployeeId = item.EmployeeId,
                        DateOfCalculation = item.DateOfCalculation,
                        DateOfPayment = item.DateOfPayment,
                        Id = item.Id,
                        Processed = item.Processed,
                        TotalCost = item.TotalCost,
                        VehicleMake = item.VehicleMake,
                        VehicleRegistration = item.VehicleRegistration
                    });
            }
        }

        private void GenerateOrders()
        {
            _showDialog.ShowDialog("GeneratorDialog", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
