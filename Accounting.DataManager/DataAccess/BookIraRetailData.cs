using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookIraRetailData : IBookIraRetailData
    {
        private ISqlDataAccess _sql;

        public BookIraRetailData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<RetailIraModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<RetailIraModel, dynamic>("dbo.spBookIraRetail_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Insert(List<RetailIraModel> list)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookIraRetail_Insert", list);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void SetProcessed(int number)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<BookUraRestModel, dynamic>("dbo.spBookIraRetail_SetProcessed", new { RedniBroj = number });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
