using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookIraHzzoData : IBookIraHzzoData
    {
        private readonly ISqlDataAccess _sql;

        public BookIraHzzoData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<BookIraHzzoModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookIraHzzoModel, dynamic>("dbo.spBookIraHzzo_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Insert(List<BookIraHzzoModel> paymebts)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookIraHzzo_Insert", paymebts);
            }
            catch (System.Exception e)
            {
                _ = e.Message;
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
