using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookAccountData : IBookAccountData
    {
        private readonly ISqlDataAccess _sql;

        public BookAccountData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<BookAccountModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookAccountModel, dynamic>("dbo.spBookAccounts_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
