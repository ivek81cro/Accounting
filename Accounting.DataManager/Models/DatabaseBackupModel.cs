using System;

namespace Accounting.DataManager.Models
{
    public class DatabaseBackupModel
    {
        public string ServerName { get; set; }

        public string DbName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public string BackupType { get; set; }

        public string BackupSize { get; set; }

        public string SavePath { get; set; }

        public string BackupSetName { get; set; }
    }
}
