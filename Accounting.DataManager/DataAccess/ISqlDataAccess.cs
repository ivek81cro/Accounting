using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface ISqlDataAccess
    {
        void CommitTransaction();
        void Dispose();
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        void RollBackTransaction();
        void SaveData<T>(string storedProcedure, T parameters, string conectionStringName);
        void SaveDataInTransaction<T>(string storedProcedure, T parameters);
        void StartTransaction(string connectionStringName);
        void BackupRestoreData(string storedProcedure, string conectionStringName);
    }
}
