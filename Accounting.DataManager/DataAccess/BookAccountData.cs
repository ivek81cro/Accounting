using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

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

        public bool Exists(BookAccountModel account)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                var result = _sql.LoadDataInTransaction<BookAccountModel, dynamic>("dbo.spBookAccounts_IfExists", new { Konto = account.Konto });
                return result.Count > 0;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
        
        public void Insert(BookAccountModel account)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookAccounts_Insert", account);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Update(BookAccountModel account)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookAccounts_Update", account);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public BookAccountModel GetByNumber(string number)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                var result = _sql.LoadDataInTransaction<BookAccountModel, dynamic>("dbo.spBookAccounts_GetByNumber", new { Konto = number });
                return result.FirstOrDefault();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
