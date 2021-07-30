using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TravelOrdersModule.Dialogs
{
    public class GeneratorDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IEmployeeEndpoint _employeeEndpoint;

        public GeneratorDialogViewModel(IEmployeeEndpoint employeeEndpoint)
        {
            _employeeEndpoint = employeeEndpoint;

            GenerateList = new DelegateCommand(GenerateOrders);
        }

        public string Title => "Generiranje naloga";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand GenerateList { get; private set; }

        private ObservableCollection<LocoOrderModel> _locoOrdersList;
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
            set { SetProperty(ref _selectedEmployee, value); }
        }

        private string _vehicleName;
        public string VehicleName
        {
            get { return _vehicleName; }
            set { SetProperty(ref _vehicleName, value); }
        }

        private string _vehicleRegistration;
        public string VehicleRegistration
        {
            get { return _vehicleRegistration; }
            set { SetProperty(ref _vehicleRegistration, value); }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime? _finishDate;
        public DateTime? FinishDate
        {
            get { return _finishDate; }
            set { SetProperty(ref _finishDate, value); }
        }

        private int _startingKilometers;
        public int StartingKilometers
        {
            get { return _startingKilometers; }
            set { SetProperty(ref _startingKilometers, value); }
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
            LoadEmployees();
        }

        private async void LoadEmployees()
        {
            Employees = await _employeeEndpoint.GetAll();
        }

        private void GenerateOrders()
        {
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
                            ZaposlenikId = SelectedEmployee.Id,
                            Datum = startDate,
                            MarkaVozila = "Osobni 1.9",
                            Registracija = "ZG123AB",
                            Opis = "Dostava",
                            Relacija = "ZG-ZG-ZG",
                            PocetnoStanje = pocetno,
                            ZavrsnoStanje = zavrsno,
                            PrijedeniKilometri = random
                        });
                    zavrsno += new Random().Next(10, 20);
                }
            }
        }
    }
}
