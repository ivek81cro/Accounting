using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class EmployeeSupplementData : IEmployeeSupplementData
    {
        private ISqlDataAccess _sql;

        public EmployeeSupplementData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<PayrollSupplementEmployeeModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PayrollSupplementEmployeeModel, dynamic>("dbo.spPayrollSupplementEmployee_GetAll", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<PayrollSupplementEmployeeModel> GetByOib(string oib)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PayrollSupplementEmployeeModel, dynamic>("dbo.spPayrollSupplementEmployee_GetByOib", new { oib });

                return output;
            }
            catch (System.Exception ex)
            {
                var str = ex.Message;
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void InsertSupplement(PayrollSupplementEmployeeModel supplement)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPayrollSupplementEmployee_Insert", supplement);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void DeleteSupplement(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<PayrollSupplementEmployeeModel, dynamic>("dbo.spPayrollSupplementEmployee_Delete", new { Id = id });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
