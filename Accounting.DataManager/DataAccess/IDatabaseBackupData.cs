using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IDatabaseBackupData
    {
        List<DatabaseBackupModel> GetAllBackups();
        void CreateBackup();
    }
}