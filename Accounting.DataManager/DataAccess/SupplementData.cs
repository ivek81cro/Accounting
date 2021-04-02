using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class SupplementData : ISupplementData
    {

        private ISqlDataAccess _sql;

        public SupplementData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<PayrollSupplementModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PayrollSupplementModel, dynamic>("dbo.spPayrollSupplement_GetAll", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
