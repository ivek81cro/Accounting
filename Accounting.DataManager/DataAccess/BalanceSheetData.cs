using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class BalanceSheetData : IBalanceSheetData
    {
        private readonly ISqlDataAccess _sql;

        public BalanceSheetData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<BalanceSheetModel> GetBalance()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BalanceSheetModel, dynamic>("dbo.spBalance_GetAll", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
