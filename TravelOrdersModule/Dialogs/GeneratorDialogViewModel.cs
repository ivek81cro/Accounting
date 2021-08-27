using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TravelOrdersModule.Dialogs
{
    public class GeneratorDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IEmployeeEndpoint _employeeEndpoint; 
        private readonly ITravelOrdersEndpoint _travelOrdersEndpoint;
        private readonly IDialogService _showDialog;

        public GeneratorDialogViewModel(IEmployeeEndpoint employeeEndpoint,
                                        IDialogService showDialog,
                                        ITravelOrdersEndpoint travelOrdersEndpoint)
        {
            _employeeEndpoint = employeeEndpoint;
            _showDialog = showDialog;
            _travelOrdersEndpoint = travelOrdersEndpoint;

            GenerateListCommand = new DelegateCommand(GenerateOrders, CanGenerate);
            SaveOrderCommand = new DelegateCommand(SaveOrder, CanSave);
            CellValueChanged = new DelegateCommand(CellChanged);
        }

        public string Title => "Generiranje naloga";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand GenerateListCommand { get; private set; }
        public DelegateCommand SaveOrderCommand { get; private set; }
        public DelegateCommand CellValueChanged { get; private set; }

        private ObservableCollection<LocoOrderModel> _locoOrdersList = new();
        public ObservableCollection<LocoOrderModel> LocoOrdersList
        {
            get { return _locoOrdersList; }
            set { SetProperty(ref _locoOrdersList, value); }
        }

        private List<EmployeeModel> _employees = new();
        public List<EmployeeModel> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private EmployeeModel _selectedEmployee;
        public EmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value);
                GenerateListCommand.RaiseCanExecuteChanged();
            }
        }

        private string _vehicleMake;
        public string VehicleMake
        {
            get { return _vehicleMake; }
            set
            {
                SetProperty(ref _vehicleMake, value);
                GenerateListCommand.RaiseCanExecuteChanged();
            }
        }

        private string _vehicleRegistration;
        public string VehicleRegistration
        {
            get { return _vehicleRegistration; }
            set
            {
                SetProperty(ref _vehicleRegistration, value);
                GenerateListCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                SetProperty(ref _startDate, value);
                GenerateListCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _finishDate;
        public DateTime? FinishDate
        {
            get { return _finishDate; }
            set
            {
                SetProperty(ref _finishDate, value);
                GenerateListCommand.RaiseCanExecuteChanged();
            }
        }

        private int _startingKilometers;
        public int StartingKilometers
        {
            get { return _startingKilometers; }
            set
            {
                SetProperty(ref _startingKilometers, value);
                GenerateListCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal _totalKm;
        public decimal TotalKm
        {
            get { return _totalKm; }
            set { SetProperty(ref _totalKm, value); }
        }

        private LocoCalculationModel _calculation;
        public LocoCalculationModel Calculation
        {
            get { return _calculation; }
            set { SetProperty(ref _calculation, value); }
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
            if (parameters.Count != 0)
            {
                Calculation = parameters.GetValue<LocoCalculationModel>("orderCalc");
                GetLocoOrders();
            }
            LoadEmployees();
        }

        private async void GetLocoOrders()
        {
            var list = await _travelOrdersEndpoint.GetLocoOrders(Calculation.Id);
            LocoOrdersList = new ObservableCollection<LocoOrderModel>(list);
        }

        private async void LoadEmployees()
        {
            Employees = await _employeeEndpoint.GetAll();
        }

        private void CellChanged()
        {
            SumTotal();
            GenerateListCommand.RaiseCanExecuteChanged();
        }

        private void SumTotal()
        {
            TotalKm = LocoOrdersList.Sum(x => x.TotalDistance);
            Calculation.TotalCost = TotalKm * 2.0m;
        }

        private bool CanGenerate()
        {
            return SelectedEmployee != null
                && VehicleRegistration != null
                && VehicleMake != null
                && VehicleRegistration != ""
                && VehicleMake != ""
                && StartDate != null
                && FinishDate != null
                && StartingKilometers != 0
                && LocoOrdersList.Count == 0;
        }

        private void GenerateOrders()
        {
            TotalKm = 0;
            DateTime futureDate = (DateTime)FinishDate;
            DateTime date = (DateTime)StartDate;
            int pocetno;
            int zavrsno = StartingKilometers;
            LocoOrdersList = new();
            for (DateTime startDate = date; startDate < futureDate; startDate = startDate.AddDays(1.0))
            {
                int random = new Random().Next(40, 90);
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    pocetno = zavrsno;
                    zavrsno += random;
                    _locoOrdersList.Add(
                        new LocoOrderModel
                        {
                            Date = startDate,
                            Description = "Dostava",
                            Destination = "ZG-ZG-ZG",
                            StartingKm = pocetno,
                            FinishKm = zavrsno,
                            TotalDistance = random
                        });
                    zavrsno += new Random().Next(10, 20);
                    TotalKm += random;
                }
            }
            decimal totalCost = TotalKm * 2.0m;

            Calculation = new LocoCalculationModel
            {
                EmployeeId = SelectedEmployee.Id,
                TotalCost = totalCost,
                VehicleMake = _vehicleMake,
                VehicleRegistration = _vehicleRegistration
            };

            SaveOrderCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            return Calculation != null && LocoOrdersList.Count > 0;
        }

        private void SaveOrder()
        {
            DialogParameters param = new DialogParameters();
            param.Add("calculation", Calculation);
            param.Add("orders", LocoOrdersList.ToList());
            _showDialog.ShowDialog("SaveLocoOrderDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
