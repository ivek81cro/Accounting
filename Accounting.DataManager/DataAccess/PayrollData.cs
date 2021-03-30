using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class PayrollData : IPayrollData
    {
        private ISqlDataAccess _sql;

        public PayrollData(ISqlDataAccess sql)
        {
            _sql = sql;
        }
        public List<PayrollModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PayrollModel, dynamic>("dbo.spPayroll_GetAll", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public PayrollModel GetByOib(string oib)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PayrollModel, dynamic>("dbo.spPayroll_GetByOib", new {Oib = oib });

                return output.FirstOrDefault();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void InsertPayroll(PayrollModel payroll)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPayroll_Insert", payroll);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void UpdatePayroll(PayrollModel payroll)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPayroll_Update", payroll);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void DeletePayroll(string oib)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<PayrollModel, dynamic>("dbo.spPayroll_Delete", new { Oib = oib });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
