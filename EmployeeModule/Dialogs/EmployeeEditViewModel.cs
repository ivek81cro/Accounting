using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace EmployeeModule.Dialogs
{
    public class EmployeeEditViewModel : BindableBase, IDialogAware
    {
        private readonly IEmployeeEndpoint _employeeEndpoint;
        private IDialogService _showDialog;

        public EmployeeEditViewModel(IEmployeeEndpoint employeeEndpoint, IDialogService showDialog)
        {
            _employeeEndpoint = employeeEndpoint;
            _showDialog = showDialog;

            SaveEmployeeCommand = new DelegateCommand(CloseDialog);
            OpenCitySelectionCommand = new DelegateCommand(OpenCitySelectDialog);
        }

        public DelegateCommand SaveEmployeeCommand { get; private set; }
        public DelegateCommand OpenCitySelectionCommand { get; private set; }

        public string Title => "Izmjena Podataka Zaposlenika";

        public event Action<IDialogResult> RequestClose;

        private EmployeeModel _employee;
        public EmployeeModel Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        private async void CloseDialog()
        {
            if(Employee.DatumOdlaska == null)
            {
                Employee.DatumOdlaska = new DateTime(day:1, month:1, year:1900);
            }
            if (Employee != null && !Employee.HasErrors &&
                await _employeeEndpoint.PostEmployee(Employee))
            {
                var result = ButtonResult.OK;

                var p = new DialogParameters();
                p.Add("employee", Employee);

                RequestClose?.Invoke(new DialogResult(result, p));
            }
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
            Employee = parameters.GetValue<EmployeeModel>("employee");
            if (Employee != null)
            {
                GetEmployeeFromDatabase(Employee.Id);
            }
            else
            {
                Employee = new EmployeeModel();
                Employee.Reset();
            }
        }

        private async void GetEmployeeFromDatabase(int id)
        {
            Employee = await _employeeEndpoint.GetById(id);
        }

        private void OpenCitySelectDialog()
        {
            _showDialog.ShowDialog(nameof(CitySelectDialog), null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    CityModel city = result.Parameters.GetValue<CityModel>("city");
                    Employee.Mjesto = city.Mjesto;
                }
            });
        }

    }
}
