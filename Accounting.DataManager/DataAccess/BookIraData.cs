using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BookIraData : IBookIraData
    {
        private ISqlDataAccess _sql;

        public BookIraData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void Insert(List<BookIraModel> ira)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookIra_Insert", ira);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<BookIraModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BookIraModel, dynamic>("dbo.spBookIra_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void SetProcessed(int iraNumber)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<BookUraRestModel, dynamic>("dbo.spBookIra_SetProcessed", new { RedniBroj = iraNumber });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Update(BookIraModel item)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookIra_Update", item);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void UpdateHzzo(BookIraHzzoModel item)
        {
            string racun = item.OriginalniBroj.Split('-')[0];
            string godina = item.OriginalniBroj.Split('/')[1];
            decimal iznos = item.PlaceniIznos;
            int id = item.Id;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBookIraHzzo_Update", new { Racun = racun, Godina = godina, Iznos = iznos, Id = id });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
