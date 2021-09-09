using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class DatabaseBackupModel : ValidationBindableBase
    {
        private string _serverName;
        public string ServerName
        {
            get { return _serverName; }
            set { SetProperty(ref _serverName, value); }
        }

        private string _dbName;
        public string DbName
        {
            get { return _dbName; }
            set { SetProperty(ref _dbName, value); }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime? _finishDate;
        public DateTime? FinishDate
        {
            get { return _finishDate; }
            set { SetProperty(ref _finishDate, value); }
        }

        private string _backupType;
        public string BackupType
        {
            get { return _backupType; }
            set { SetProperty(ref _backupType, value); }
        }

        private string _backupSize;
        public string BackupSize
        {
            get { return _backupSize; }
            set { SetProperty(ref _backupSize, value); }
        }

        private string _savePath;
        public string SavePath
        {
            get { return _savePath; }
            set { SetProperty(ref _savePath, value); }
        }

        private string _backupSetName;
        public string BackupSetName
        {
            get { return _backupSetName; }
            set { SetProperty(ref _backupSetName, value); }
        }
    }
}
