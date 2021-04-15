using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using AutoMapper;
using PayrollModule.ServiceLocal;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace PayrollModule.ViewModels
{
    class PayrollProcessingViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly IPayrollEndpoint _payrollEndpoint;
        private readonly IPayrollArchivePrepare _processPayroll;
        private readonly IPayrollSupplementEmployeeEndpoint _supplementEndpoint;
        private PayrollArchiveModel _archive;

        public PayrollProcessingViewModel(IMapper mapper,
            IPayrollEndpoint payrollEndpoint,
            IPayrollSupplementEmployeeEndpoint supplementEndpoint, 
            IPayrollArchivePrepare processPayroll)
        {
            _mapper = mapper;
            _payrollEndpoint = payrollEndpoint;
            _supplementEndpoint = supplementEndpoint;
            _processPayroll = processPayroll;

            CalculatePayrollCommand = new DelegateCommand(Calculate, CanCalculate);
            SaveToArchiveCommand = new DelegateCommand(SaveToArchive, CanSave);
        }

        public DelegateCommand CalculatePayrollCommand { get; private set; }
        public DelegateCommand SaveToArchiveCommand { get; private set; }

        #region Archive Preparation And Save

        private string _saveStatusMessage;
        public string SaveStatusMessage
        {
            get { return  _saveStatusMessage; }
            set { SetProperty(ref  _saveStatusMessage, value); }
        }

        private bool CanCalculate()
        {
            if (!IfPayrolls && !IfSupplements)
            {
                return false;
            }

            if(Accounting != null)
            {
                if(Accounting.DatumDo == null 
                    || Accounting.DatumOd==null 
                    || Accounting.DatumObracuna==null)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private void Calculate()
        {
            SaveStatusMessage = null;
            SelectSupplementsForProcessing();

            if(OnlySupplements)
            {
                foreach(var pay in PayrollCalculations)
                {
                    pay.ResetMoneyValues();
                }
            }

            _archive = _processPayroll.Process(PayrollCalculations, SupplementCalculations, Accounting);

            InitializeArchivePreparationDatagrid();

            SaveToArchiveCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            var result = CanCalculate();
            if(Accounting.Opis == null || Accounting.Opis == "")
            {
                result = false;
            }

            if(_archive == null)
            {
                result = false;
            }
            else
            {
                if(_archive.Payrolls.Count == 0)
                {
                    result = false;
                }
                if(_archive.Calculation==null)
                {
                    result = false;
                }
            }            

            return result;
        }

        private async void SaveToArchive()
        {
            if(await _processPayroll.SaveToDatabase(_archive))
            {
                SaveStatusMessage = "Uspješno spremljeno u arhivu";
                SelectAllPayrolls = false;
                SelectAllPayrolls = false;
                IfPayrolls = false;
                IfSupplements = false;
                SupplementCalculations = null;
                PayrollCalculations = null;
                PayrollArchive = null;
                SupplementsArchive = null;
            }
            else
            {
                SaveStatusMessage = "Zapis za razdoblje več postoji u arhivi, ukoliko radite ispravak, " +
                    "izbrišite prvo zapis za isto razdoblje, sa istim datumom obračuna.";
            }
        }

        #endregion

        #region Payrolls

        private bool _ifPayrolls;
        public bool IfPayrolls
        {
            get { return _ifPayrolls; }
            set
            {
                SetProperty(ref _ifPayrolls, value);
                if (value)
                {
                    LoadPayrolls();
                }
                else
                {
                    PayrollCalculations = null;
                    SelectAllPayrolls = false;
                    FilterPayrolls = "";
                }
                CalculatePayrollCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<PayrollArchivePayrollModel> _payrollCalculations;
        public ObservableCollection<PayrollArchivePayrollModel> PayrollCalculations
        {
            get { return _payrollCalculations; }
            set { SetProperty(ref _payrollCalculations, value); }
        }

        private bool _selectAllPayrolls;
        public bool SelectAllPayrolls
        {
            get { return _selectAllPayrolls; }
            set 
            { 
                SetProperty(ref _selectAllPayrolls, value);
                if (PayrollCalculations != null)
                {
                    foreach (var item in PayrollCalculations)
                    {
                        item.IsChecked = value;
                    }
                }
            }
        }

        private bool _onlySupplements;
        public bool OnlySupplements
        {
            get { return _onlySupplements; }
            set { SetProperty(ref _onlySupplements, value); }
        }

        private ICollectionView _payrollsView;
        private string _filterPayrolls;
        public string FilterPayrolls
        {
            get { return _filterPayrolls; }
            set
            {
                SetProperty(ref _filterPayrolls, value);
                _payrollsView.Refresh();
            }
        }

        public async void LoadPayrolls()
        {
            var payrollList = await _payrollEndpoint.GetAll();
            var cPayrollList = _mapper.Map<List<PayrollArchivePayrollModel>>(payrollList);
            PayrollCalculations = new ObservableCollection<PayrollArchivePayrollModel>(cPayrollList);
            LoadPayrollsFilter();
        }

        private void LoadPayrollsFilter()
        {
            _payrollsView = CollectionViewSource.GetDefaultView(PayrollCalculations);
            _payrollsView.Filter = o => string.IsNullOrEmpty(FilterPayrolls) ?
                true : ((PayrollArchivePayrollModel)o).Prezime.ToLower().Contains(FilterPayrolls.ToLower());
        }


        #endregion

        #region Supplements

        private bool _ifSupplements;
        public bool IfSupplements
        {
            get { return _ifSupplements; }
            set
            {
                SetProperty(ref _ifSupplements, value);
                if (value)
                {
                    LoadSupplements();
                }
                else
                {
                    SupplementSelectDisplay = null;
                    SelectAllSupplements = false;
                    FilterSupplementsDisplay = "";
                }
                CalculatePayrollCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _selectAllSupplements;
        public bool SelectAllSupplements
        {
            get { return _selectAllSupplements; }
            set 
            { 
                SetProperty(ref _selectAllSupplements, value);
                if (SupplementSelectDisplay != null)
                {
                    foreach (var item in SupplementSelectDisplay)
                    {
                        item.IsChecked = value;
                    }
                }
            }
        }

        private ObservableCollection<PayrollArchiveSupplementModel> _supplementCalculations;
        public ObservableCollection<PayrollArchiveSupplementModel> SupplementCalculations
        {
            get { return _supplementCalculations; }
            set { SetProperty(ref _supplementCalculations, value); }
        }

        private ObservableCollection<PayrollSupplementEmployeeModel> _supplementSelectDisplay;
        public ObservableCollection<PayrollSupplementEmployeeModel> SupplementSelectDisplay
        {
            get { return _supplementSelectDisplay; }
            set { SetProperty(ref _supplementSelectDisplay, value); }
        }

        private ICollectionView _supplementsView;
        private string _filterSupplementsDisplay;
        public string FilterSupplementsDisplay
        {
            get { return _filterSupplementsDisplay; }
            set
            {
                SetProperty(ref _filterSupplementsDisplay, value);
                _supplementsView.Refresh();
            }
        }

        private async void LoadSupplements()
        {
            var supplementList = await _supplementEndpoint.GetAll();
            var cSupplementList = _mapper.Map<List<PayrollArchiveSupplementModel>>(supplementList);
            SupplementCalculations = new ObservableCollection<PayrollArchiveSupplementModel>(cSupplementList);

            var distinctSuppl = await _supplementEndpoint.GetDistinct();
            SupplementSelectDisplay = new ObservableCollection<PayrollSupplementEmployeeModel>(distinctSuppl);
            LoadSupplementsFilter();
        }

        private void LoadSupplementsFilter()
        {
            _supplementsView = CollectionViewSource.GetDefaultView(SupplementSelectDisplay);
            _supplementsView.Filter = o => string.IsNullOrEmpty(FilterSupplementsDisplay) ?
                true : ((PayrollSupplementEmployeeModel)o).Naziv.ToLower().Contains(FilterSupplementsDisplay.ToLower());
        }

        private void SelectSupplementsForProcessing()
        {
            if (SupplementSelectDisplay != null)
            {
                foreach (var displayItem in SupplementSelectDisplay)
                {
                    if (displayItem.IsChecked)
                    {
                        foreach (var item in SupplementCalculations)
                        {
                            if (item.Sifra == displayItem.Sifra)
                            {
                                item.IsChecked = true;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region PayrollAccounting

        private PayrollArchiveHeaderModel _accounting = new();
        public PayrollArchiveHeaderModel Accounting
        {
            get { return _accounting; }
            set 
            { 
                SetProperty(ref _accounting, value);
            }
        }

        private ObservableCollection<PayrollArchivePayrollModel> _payrollArchive;
        public ObservableCollection<PayrollArchivePayrollModel> PayrollArchive
        {
            get { return _payrollArchive; }
            set { SetProperty(ref _payrollArchive, value); }
        }

        private ObservableCollection<PayrollSupplementEmployeeModel> _supplementArchive;
        public ObservableCollection<PayrollSupplementEmployeeModel> SupplementsArchive
        {
            get { return _supplementArchive; }
            set { SetProperty(ref _supplementArchive, value); }
        }

        private void InitializeArchivePreparationDatagrid()
        {
            PayrollArchive = new ObservableCollection<PayrollArchivePayrollModel>(_archive.Payrolls);
            
            if (SupplementsArchive != null)
            {
                SupplementsArchive = new ObservableCollection<PayrollSupplementEmployeeModel>(_archive.Supplements);
            }
        }

        #endregion
    }
}
