using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Microsoft.Win32;
using PayrollModule.Dialogs;
using PayrollModule.ServiceLocal;
using PayrollModule.ServiceLocal.EporeznaModels;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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
            
            LoadJoppdEmployees();

        }

        public DelegateCommand EditEmployeeCommand { get; private set; }
        public DelegateCommand GenerateJoppdCommand { get; private set; }

        #region Properties
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

        private ObservableCollection<sPrimateljiP> _irsRecipients;
        public ObservableCollection<sPrimateljiP> IrsRecipients
        {
            get { return _irsRecipients; }
            set { SetProperty(ref _irsRecipients, value); }
        }
        #endregion

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

        private async void GenerateJoppd()
        {
            sObrazacJOPPD joppd = new();
            joppd = await _joppdGenerate.CreateJoppdEporezna(FormDate, FormNumber, FormCreator, Archive, JoppdEmployees.ToList());

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "XML file|*.xml",
                Title = "Spremi XML datoteku"
            };
            bool? result = sfd.ShowDialog();
            string path;
            if (result != null && result == true)
            {
                path = sfd.FileName;

                TextWriter txtWriter = new StreamWriter(path);
                XmlSerializer x = new XmlSerializer(joppd.GetType());
                x.Serialize(txtWriter, joppd);
                txtWriter.Close();

                StreamReader reader = new StreamReader(path);
                var data = (sObrazacJOPPD)x.Deserialize(reader);
                reader.Close();

                IrsRecipients = new ObservableCollection<sPrimateljiP>(data.StranaB[0]);
            }
            //TODO new class for result display in datagrid map sJoppdObrazac to new class
        }

        private bool CanGenerateJoppd()
        {
            if (JoppdEmployees != null)
            {
                foreach (var emp in JoppdEmployees)
                {
                    if (emp.HasErrors)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            if(FormDate == null || !CheckFormCreatorName())
            {
                return false;
            }

            return true;
        }

        private bool CheckFormCreatorName()
        {
            if (FormCreator != null)
            {
                return FormCreator.Length > 0 && FormCreator.Contains(" ");
            }

            return false;
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
