using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace PayrollModule.Dialogs
{
    public class SupplementsDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IPayrollSupplementEndpoint _supplementsEndpoint;
        private readonly IPayrollSupplementEmployeeEndpoint _payrollSupplementEmployeeEndpoint;

        public SupplementsDialogViewModel(IPayrollSupplementEndpoint supplementsEndpoint, 
            IPayrollSupplementEmployeeEndpoint payrollSupplementEmployeeEndpoint)
        {
            _supplementsEndpoint = supplementsEndpoint;
            _payrollSupplementEmployeeEndpoint = payrollSupplementEmployeeEndpoint;

            AddSupplementCommand = new DelegateCommand(AddSupplement, CanAddSupplement);
        }

        public DelegateCommand AddSupplementCommand { get; set; }
        public string Title => "Dodaci zaposlenika";

        private ObservableCollection<PayrollSupplementModel> _supplements;
        public ObservableCollection<PayrollSupplementModel> Supplements
        {
            get { return _supplements; }
            set { SetProperty(ref _supplements, value); }
        }

        private PayrollSupplementModel _selectedSupplement;
        public PayrollSupplementModel SelectedSupplement
        {
            get { return _selectedSupplement; }
            set 
            { 
                SetProperty(ref _selectedSupplement, value);
                AddSupplementCommand.RaiseCanExecuteChanged();
            }
        }

        private string _employeeOib;
        public string EmployeeOib
        {
            get { return _employeeOib; }
            set { SetProperty(ref _employeeOib, value); }
        }

        private decimal _supplementValue;
        public decimal SupplementValue
        {
            get { return _supplementValue; }
            set 
            {
                SetProperty(ref _supplementValue, value);
                AddSupplementCommand.RaiseCanExecuteChanged();
            }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private ObservableCollection<PayrollSupplementEmployeeModel> _employeeSupplements;
        public ObservableCollection<PayrollSupplementEmployeeModel> EmployeeSupplements
        {
            get { return _employeeSupplements; }
            set { SetProperty(ref _employeeSupplements, value); }
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
            FullName = parameters.GetValue<string>("fullname");
            EmployeeOib = parameters.GetValue<string>("oib");

            LoadEmployeeSupplements();
            LoadSupplementsList();
        }

        private bool CanAddSupplement()
        {
            return SelectedSupplement != null && SupplementValue != 0;
        }

        private async void LoadEmployeeSupplements()
        {
            var sup = await _payrollSupplementEmployeeEndpoint.GetByOib(EmployeeOib);
            EmployeeSupplements = new ObservableCollection<PayrollSupplementEmployeeModel>(sup);
        }

        private async void LoadSupplementsList()
        {
            var sup = await _supplementsEndpoint.GetAll();
            Supplements = new ObservableCollection<PayrollSupplementModel>(sup);
        }

        private async void AddSupplement()
        {
            var psm = new PayrollSupplementEmployeeModel
            {
                Oib = EmployeeOib,
                Iznos = SupplementValue,
                Sifra = SelectedSupplement.Sifra,
                Naziv = SelectedSupplement.Naziv
            };

            await _payrollSupplementEmployeeEndpoint.PostSupplement(psm);
            EmployeeSupplements.Add(psm);
        }
    }
}
