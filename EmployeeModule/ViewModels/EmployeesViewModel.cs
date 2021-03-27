using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using EmployeeModule.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace EmployeeModule.ViewModels
{
    public class EmployeesViewModel : ViewModelBase
    {
        private IEmployeeEndpoint _employeeEndpoint;
        private IRegionManager _regionManager;
        private IDialogService _showDialog;

        public EmployeesViewModel(IEmployeeEndpoint employeeEndpoint, IRegionManager regionManager, 
            IDialogService showDialog)
        {
            _employeeEndpoint = employeeEndpoint;
            _regionManager = regionManager;
            _showDialog = showDialog;

            NewEmployeeCommand = new DelegateCommand(AddNewEmployee);
            EditEmployeeCommand = new DelegateCommand(EditEmployee);
            DeleteEmployeeCommand = new DelegateCommand(DeleteEmployee, CanDelete);
        }

        public DelegateCommand NewEmployeeCommand { get; private set; }
        public DelegateCommand EditEmployeeCommand { get; private set; }
        public DelegateCommand DeleteEmployeeCommand { get; private set; }

        private ObservableCollection<EmployeeModel> _employees;
        public ObservableCollection<EmployeeModel> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private EmployeeModel _employeeModel;
        public EmployeeModel SelectedEmployee
        {
            get { return _employeeModel; }
            set 
            { 
                SetProperty(ref _employeeModel, value);
                DeleteEmployeeCommand.RaiseCanExecuteChanged();
            }
        }

        private ICollectionView _employeesView;
        private string _filterEmployees;
        public string FilterEmployees
        {
            get { return _filterEmployees; }
            set
            {
                SetProperty(ref _filterEmployees, value.ToUpper());
                _employeesView.Refresh();
            }
        }

        public async void LoadEmployees()
        {
            var employeesList = await _employeeEndpoint.GetAll();
            Employees = new ObservableCollection<EmployeeModel>(employeesList);
            _employeesView = CollectionViewSource.GetDefaultView(Employees);
        }

        private bool CanDelete()
        {
            return SelectedEmployee != null;
        }

        private void DeleteEmployee()
        {
            _showDialog.ShowDialog("AreYouSureView", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    _employeeEndpoint.DeleteEmployee(SelectedEmployee.Id);
                    _employees.Remove(SelectedEmployee);
                }
            });
        }

        private void EditEmployee()
        {
            SaveEmployeeToDatabase();
        }

        private void AddNewEmployee()
        {
            if (SelectedEmployee != null)
            {
                SelectedEmployee = null;
            }
            SaveEmployeeToDatabase();
        }

        private void SaveEmployeeToDatabase()
        {
            var parameters = new DialogParameters();
            parameters.Add("employee", SelectedEmployee);
            _showDialog.ShowDialog(nameof(EmployeeEdit), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    EmployeeModel employee = result.Parameters.GetValue<EmployeeModel>("employee");
                    LoadEmployees();
                }
            });
        }
    }
}
