using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BackupModule.ViewModels
{
    public class BackupViewModel : ViewModelBase
    {
        private readonly IDatabaseBackupEndpoint _databaseBackupEndpoint;

        public BackupViewModel(IDatabaseBackupEndpoint databaseBackupEndpoint)
        {
            _databaseBackupEndpoint = databaseBackupEndpoint;

            CreateBackupCommand = new DelegateCommand(CreateBackup);

            LoadData();
        }

        public DelegateCommand CreateBackupCommand { get; private set; }

        private ObservableCollection<DatabaseBackupModel> _existingBackups;
        public ObservableCollection<DatabaseBackupModel> ExistingBackups
        {
            get { return _existingBackups; }
            set { SetProperty(ref _existingBackups, value); }
        }

        private DatabaseBackupModel _selectedBackup;
        public DatabaseBackupModel SelectedBackup
        {
            get { return _selectedBackup; }
            set { SetProperty(ref _selectedBackup, value); }
        }

        private async void LoadData()
        {
            List<DatabaseBackupModel> list = await _databaseBackupEndpoint.GetAll();
            ExistingBackups = new ObservableCollection<DatabaseBackupModel>(list);
        }

        private async void CreateBackup()
        {
            await _databaseBackupEndpoint.CreateBackup();
            LoadData();
        }
    }
}
