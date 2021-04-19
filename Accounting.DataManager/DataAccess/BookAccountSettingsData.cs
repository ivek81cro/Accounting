using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookAccountSettingsData : IBookAccountSettingsData
    {
        private readonly ISqlDataAccess _sql;

        public BookAccountSettingsData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<BookAccountsSettingsModel> Get(string name)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookAccountsSettingsModel, dynamic>("dbo.spBookAccountsSettings_GetByBookName", new { BookName = name });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Insert(BookAccountsSettingsModel setting)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookAccountsSettings_Insert", setting);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<BookAccountsSettingsModel, dynamic>("dbo.spBookAccountsSettings_Delete", new { Id = id });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
