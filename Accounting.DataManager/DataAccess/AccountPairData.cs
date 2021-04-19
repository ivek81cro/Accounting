using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class AccountPairData : IAccountPairData
    {
        private readonly ISqlDataAccess _sql;

        public AccountPairData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<AccountPairModel> GetByBookName(string bookName)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AccountPairModel, dynamic>("dbo.spAccountPairs_GetByBookName", new { BookName = bookName });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Insert(AccountPairModel pair)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spAccountPairs_Insert", pair);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
