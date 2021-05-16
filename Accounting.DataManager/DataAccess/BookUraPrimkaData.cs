using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookUraPrimkaData : IBookUraPrimkaData
    {
        private ISqlDataAccess _sql;

        public BookUraPrimkaData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void InsertPrimke(List<BookUraPrimkaModel> primke)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookUraPrimka_Insert", primke);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<BookUraPrimkaModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookUraPrimkaModel, dynamic>("dbo.spBookUraPrimka_GetAll", new { });
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

                _sql.LoadDataInTransaction<BookUraPrimkaModel, dynamic>("dbo.spBookUraPrimka_SetProcessed", new { BrojUKnjiziUra = number });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
