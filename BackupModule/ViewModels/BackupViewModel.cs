using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
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

            LoadData();
        }

        private ObservableCollection<DatabaseBackupModel> _existingBackups;
        public ObservableCollection<DatabaseBackupModel> ExistingBackups
        {
            get { return _existingBackups; }
            set { SetProperty(ref _existingBackups, value); }
        }

        private async void LoadData()
        {
            List<DatabaseBackupModel> list = await _databaseBackupEndpoint.GetAll();
            ExistingBackups = new ObservableCollection<DatabaseBackupModel>(list);
        }
    }
}
