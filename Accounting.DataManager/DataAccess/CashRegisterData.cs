using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class CashRegisterData : ICashRegisterData
    {
        private readonly ISqlDataAccess _sql;

        public CashRegisterData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<CashRegisterModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<CashRegisterModel, dynamic>("dbo.spCashRegister_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void InsertItems(List<CashRegisterModel> items)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spCashRegister_InsertItems", items);
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

                _sql.LoadDataInTransaction<CashRegisterModel, dynamic>("dbo.spCashRegister_SetProcessed", new { RedniBroj = number });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
