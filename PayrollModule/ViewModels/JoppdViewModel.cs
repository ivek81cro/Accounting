using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using PayrollModule.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PayrollModule.ViewModels
{
    public class JoppdViewModel : ViewModelBase
    {
        private readonly IJoppdEmployeeEndpoint _joppdEmployeeEndpoint;
        private IDialogService _showDialog;

        public JoppdViewModel(IJoppdEmployeeEndpoint joppdEmployeeEndpoint, IDialogService showDialog)
        {
            _joppdEmployeeEndpoint = joppdEmployeeEndpoint;
            _showDialog = showDialog;

            EditEmployeeCommand = new DelegateCommand(EditEmployee, CanEditEmployee);
        }

        public DelegateCommand EditEmployeeCommand { get; private set; }

        private PayrollArchiveModel _archive;
        public PayrollArchiveModel Archive
        {
            get { return _archive; }
            set { SetProperty(ref _archive, value); }
        }

        private DateTime? _formDate;
        public DateTime? FormDate
        {
            get { return _formDate; }
            set
            {
                SetProperty(ref _formDate, value);
                SetJoppdFormNumber();
            }
        }

        private string _formIdentifier;
        public string FormNumber
        {
            get { return _formIdentifier; }
            set { SetProperty(ref _formIdentifier, value); }
        }

        private string _formcreator;
        public string FormCreator
        {
            get { return _formcreator; }
            set { SetProperty(ref _formcreator, value); }
        }

        private ObservableCollection<JoppdEmployeeModel> _joppdEmployees;
        public ObservableCollection<JoppdEmployeeModel> JoppdEmployees
        {
            get { return _joppdEmployees; }
            set { SetProperty(ref _joppdEmployees, value); }
        }

        private JoppdEmployeeModel _selectedEmployee;
        public JoppdEmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set 
            { 
                SetProperty(ref _selectedEmployee, value);
                EditEmployeeCommand.RaiseCanExecuteChanged();
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Archive = navigationContext.Parameters.GetValue<PayrollArchiveModel>("archive");
        }

        public async void LoadJoppdEmployees()
        {
            var list = await _joppdEmployeeEndpoint.GetAll();
            JoppdEmployees = new ObservableCollection<JoppdEmployeeModel>(list);
        }

        private bool CanEditEmployee()
        {
            return SelectedEmployee != null;
        }

        private void EditEmployee()
        {
            var parameters = new DialogParameters();
            parameters.Add("employee", SelectedEmployee);
            _showDialog.ShowDialog(nameof(JoppdEmployee), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoadJoppdEmployees();
                }
            });
        }

        private void SetJoppdFormNumber()
        {
            if (FormDate != null)
            {
                string formNumber = FormDate?.Year.ToString();
                string dayOfYear = FormDate?.DayOfYear.ToString();

                if (dayOfYear.Length < 3)
                {
                    while (dayOfYear.Length != 3)
                    {
                        dayOfYear = "0" + dayOfYear;
                    }
                }
                formNumber = formNumber[^2..] + dayOfYear;

                FormNumber = formNumber;
            }
        }
    }
}
