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

        public EmployeeEditViewModel(IEmployeeEndpoint employeeEndpoint)
        {
            _employeeEndpoint = employeeEndpoint;
            SaveEmployeeCommand = new DelegateCommand(CloseDialog);
        }

        public DelegateCommand SaveEmployeeCommand { get; }

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
    }
}
