﻿using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PayrollModule.ViewModels
{
    public class ArchiveViewModel : ViewModelBase
    {
        private readonly IPayrollArchiveEndpoint _archiveEndpoint; 
        private readonly IDialogService _showDialog;
        private readonly IRegionManager _regionManager;

        private List<PayrollArchiveHeaderModel> _archiveHeaders;
        private List<PayrollArchivePayrollModel> _archivePayrolls;
        private List<PayrollArchiveSupplementModel> _archiveSupplements;

        public ArchiveViewModel(IPayrollArchiveEndpoint archiveEndpoint, 
            IDialogService showDialog, IRegionManager regionManager)
        {
            _archiveEndpoint = archiveEndpoint;
            _showDialog = showDialog;
            _regionManager = regionManager;

            DeletePayrollCommand = new DelegateCommand(DeleteSelectedRecord, CanDelete);
            CreateJoppdFormCommand = new DelegateCommand(CreateJoppdDialog, CanCreateJoppd);
        }

        public DelegateCommand CreateJoppdFormCommand { get; private set; }
        public DelegateCommand DeletePayrollCommand { get; private set; }

        private ObservableCollection<PayrollArchiveHeaderModel> _accountingHeaders;
        public ObservableCollection<PayrollArchiveHeaderModel> AccountingHeaders
        {
            get { return _accountingHeaders; }
            set { SetProperty(ref _accountingHeaders, value); }
        }

        private PayrollArchiveHeaderModel _selectedArchive;
        public PayrollArchiveHeaderModel SelectedArchive
        {
            get { return _selectedArchive; }
            set 
            { 
                SetProperty(ref _selectedArchive, value);
                if (value != null)
                {
                    LoadDetails();
                }
                DeletePayrollCommand.RaiseCanExecuteChanged();
                CreateJoppdFormCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<PayrollArchivePayrollModel> _payrolls;
        public ObservableCollection<PayrollArchivePayrollModel> Payrolls
        {
            get { return _payrolls; }
            set { SetProperty(ref _payrolls, value); }
        }

        private ObservableCollection<PayrollArchiveSupplementModel> _supplements;
        public ObservableCollection<PayrollArchiveSupplementModel> Supplements
        {
            get { return _supplements; }
            set { SetProperty(ref _supplements, value); }
        }

        private decimal _brutoSum;
        public decimal BrutoSum
        {
            get { return _brutoSum; }
            set { SetProperty(ref _brutoSum, value); }
        }

        private decimal _healthcareSum;
        public decimal HealthcareSum
        {
            get { return _healthcareSum; }
            set { SetProperty(ref _healthcareSum, value); }
        }

        private decimal _supplementsSum;
        public decimal SupplementsSum
        {
            get { return _supplementsSum; }
            set { SetProperty(ref _supplementsSum, value); }
        }

        private decimal _payrollExpense;
        public decimal PayrollExpense
        {
            get { return _payrollExpense; }
            set { SetProperty(ref _payrollExpense, value); }
        }

        public async void LoadArchive()
        {
            _archiveHeaders = new();
            _archiveHeaders = await _archiveEndpoint.GetArchiveHeaders();
            AccountingHeaders = new ObservableCollection<PayrollArchiveHeaderModel>(_archiveHeaders);
        }

        private async void LoadDetails()
        {
            BrutoSum = 0;
            HealthcareSum = 0;
            SupplementsSum = 0;
            PayrollExpense = 0;

            _archivePayrolls = new();
            _archivePayrolls = await _archiveEndpoint.GetArchivePayrolls(SelectedArchive.Id);
            Payrolls = new ObservableCollection<PayrollArchivePayrollModel>(_archivePayrolls);

            _archiveSupplements = new();
            _archiveSupplements = await _archiveEndpoint.GetArchiveSupplements(SelectedArchive.Id);
            Supplements = new ObservableCollection<PayrollArchiveSupplementModel>(_archiveSupplements);

            SetSums();
        }

        private void SetSums()
        {
            foreach(var p in Payrolls)
            {
                BrutoSum += p.Bruto;
                HealthcareSum += p.DoprinosZdravstvo;
            }

            foreach(var s in Supplements)
            {
                SupplementsSum += s.Iznos;
            }

            PayrollExpense = BrutoSum + HealthcareSum + SupplementsSum;
        }

        private void DeleteSelectedRecord()
        {
            if(SelectedArchive != null)
            {
                _showDialog.ShowDialog("AreYouSureView", null, result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        _archiveEndpoint.DeleteRecord(SelectedArchive.Id);
                        AccountingHeaders.Remove(SelectedArchive);
                        SelectedArchive = null;
                        Payrolls = null;
                        Supplements = null;
                    }
                });
            }
        }

        private bool CanDelete()
        {
            return SelectedArchive != null;
        }

        private void CreateJoppdDialog()
        {
            PayrollArchiveModel archive = new PayrollArchiveModel
            {
                Calculation = SelectedArchive,
                Payrolls = _archivePayrolls,
                Supplements = _archiveSupplements
            };

            var p = new NavigationParameters();
            string tabTitle = TabHeaderTitles.GetHeaderTitle("JoppdView");
            p.Add("archive", archive);
            p.Add("title", tabTitle);
            _regionManager.RequestNavigate("ContentRegion", "JoppdView", p);
        }

        private bool CanCreateJoppd()
        {
            return SelectedArchive != null;
        }
    }
}