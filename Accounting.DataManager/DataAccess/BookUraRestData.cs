using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookUraRestData : IBookUraRestData
    {
        private ISqlDataAccess _sql;

        public BookUraRestData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void Insert(List<BookUraRestModel> data)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookUraRest_Insert", data);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<BookUraRestModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookUraRestModel, dynamic>("dbo.spBookUraRest_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void SetProcessed(int uraNumber)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<BookUraRestModel, dynamic>("dbo.spBookUraRest_SetProcessed", new { BrojUKnjiziUra = uraNumber });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<BookUraRestModel> GetDiscounts()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookUraRestModel, dynamic>("dbo.spBookUraDiscounts_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
