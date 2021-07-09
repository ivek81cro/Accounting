using Accounting.DataManager.Models;
using System;
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

        public List<BalanceSheetModel> GetBalancePeriod(List<DateTime> dates)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BalanceSheetModel, dynamic>("dbo.spBalance_GetForPeriod",
                    new { DateFrom = dates[0], DateTo = dates[1] });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
