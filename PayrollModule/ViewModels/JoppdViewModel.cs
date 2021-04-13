using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using PayrollModule.Dialogs;
using PayrollModule.ServiceLocal;
using PayrollModule.ServiceLocal.EporeznaModels;
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
        private readonly IJoppdGenerate _joppdGenerate;

        public JoppdViewModel(IJoppdEmployeeEndpoint joppdEmployeeEndpoint,
            IDialogService showDialog, IJoppdGenerate joppdGenerate)
        {
            _joppdEmployeeEndpoint = joppdEmployeeEndpoint;
            _showDialog = showDialog;
            _joppdGenerate = joppdGenerate;

            EditEmployeeCommand = new DelegateCommand(EditEmployee, CanEditEmployee);
            GenerateJoppdCommand = new DelegateCommand(GenerateJoppd, CanGenerateJoppd);
        }

        public DelegateCommand EditEmployeeCommand { get; private set; }
        public DelegateCommand GenerateJoppdCommand { get; private set; }

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
                GenerateJoppdCommand.RaiseCanExecuteChanged();
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
            set 
            {
                SetProperty(ref _formcreator, value);
                GenerateJoppdCommand.RaiseCanExecuteChanged();
            }
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

        private ObservableCollection<sPrimateljiP> _irsRecipients;
        public ObservableCollection<sPrimateljiP> IrsRecipients
        {
            get { return _irsRecipients; }
            set { SetProperty(ref _irsRecipients, value); }
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

        private async void GenerateJoppd()
        {
            sObrazacJOPPD joppd = await _joppdGenerate.CreateJoppdEporezna(FormDate, FormNumber, FormCreator, Archive, JoppdEmployees.ToList());
            IrsRecipients = new ObservableCollection<sPrimateljiP>(joppd.StranaB[0]);
            //TODO first generate XML then read into datagrid
        }

        private bool CanGenerateJoppd()
        {
            bool result=true;

            if (JoppdEmployees != null)
            {
                foreach (var emp in JoppdEmployees)
                {
                    if (emp.HasErrors)
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }

            result = FormDate != null;
            result = FormCreator != null;

            return result;
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
