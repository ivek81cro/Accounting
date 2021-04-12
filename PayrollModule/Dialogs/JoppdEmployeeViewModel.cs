using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PayrollModule.Dialogs
{
    public class JoppdEmployeeViewModel : BindableBase, IDialogAware
    {
        private readonly IJoppdEmployeeEndpoint _joppdEndpoint;
        private readonly IEmployeeEndpoint _employeeEndpoint;
        private readonly ICityEndpoint _cityEndpoint;

        public JoppdEmployeeViewModel(IJoppdEmployeeEndpoint joppdEndpoint,
            IEmployeeEndpoint employeeEndpoint, ICityEndpoint cityEndpoint)
        {
            _joppdEndpoint = joppdEndpoint;
            _employeeEndpoint = employeeEndpoint;
            _cityEndpoint = cityEndpoint;

            SaveDataCommand = new DelegateCommand(SaveJoppdData, CanSave);
        }

        public string Title => "Podaci zaposlenika za JOPPD obrazac";

        public DelegateCommand SaveDataCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        private JoppdEmployeeModel _employeeData;
        public JoppdEmployeeModel Employee
        {
            get { return _employeeData; }
            set { SetProperty(ref _employeeData, value); }
        }

        private string  _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        public bool CanCloseDialog()
        {
            return !Employee.HasErrors;
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            Employee = parameters.GetValue<JoppdEmployeeModel>("employee");

            var emp = await _employeeEndpoint.GetByOib(Employee.Oib);
            FullName = "Podaci zaposlenika: " + emp.Ime + " " + emp.Prezime;

            var city = await _cityEndpoint.GetByName(emp.Mjesto);
            Employee.SifraPrebivalista = city.Sifra;

        }

        private void SaveJoppdData()
        {
            if (!Employee.HasErrors)
            {
                _joppdEndpoint.PostJoppdData(Employee);
            }
            var result = ButtonResult.OK;

            RequestClose?.Invoke(new DialogResult(result));
        }

        private bool CanSave()
        {
            return true;
        }
    }
}
