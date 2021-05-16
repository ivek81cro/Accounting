using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookUraReproData : IBookUraReproData
    {
        private ISqlDataAccess _sql;

        public BookUraReproData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void InsertPrimke(List<BookUraPrimkaReproModel> primke)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookUraRepro_Insert", primke);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<BookUraPrimkaReproModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookUraPrimkaReproModel, dynamic>("dbo.spBookUraRepro_GetAll", new { });
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

                _sql.LoadDataInTransaction<BookUraPrimkaReproModel, dynamic>("dbo.spBookUraRepro_SetProcessed", new { BrojUKnjiziUra = number });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
