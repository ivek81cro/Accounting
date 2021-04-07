using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace PayrollModule.Dialogs
{
    public class JoppdDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IJoppdEmployeeEndpoint _joppdEmployeeEndpoint;

        public JoppdDialogViewModel(IJoppdEmployeeEndpoint joppdEmployeeEndpoint)
        {
            _joppdEmployeeEndpoint = joppdEmployeeEndpoint;
        }

        public string Title => "Izrada JOPPD obrasca";

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
            set { SetProperty(ref _selectedEmployee, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Archive = parameters.GetValue<PayrollArchiveModel>("archive");

            LoadJoppdEmployees();
        }

        private async void LoadJoppdEmployees()
        {
            var list = await _joppdEmployeeEndpoint.GetAll();
            JoppdEmployees = new ObservableCollection<JoppdEmployeeModel>(list);
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
