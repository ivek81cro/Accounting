using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class DatabaseBackupData : IDatabaseBackupData
    {
        private readonly ISqlDataAccess _sql;

        public DatabaseBackupData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<DatabaseBackupModel> GetAllBackups()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<DatabaseBackupModel, dynamic>("dbo.spBackupDatabase_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void CreateBackup()
        {
            try
            {
                _sql.BackupData("dbo.spBackupDatabase_Full", "AccountingConnStr");
            }
            catch (System.Exception e)
            {
                var mes = e.Message;
                throw;
            }
        }
    }
}
