using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class PayrollAccountingData : IPayrollAccountingData
    {
        private readonly ISqlDataAccess _sql;

        public PayrollAccountingData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<PayrollArchiveHeaderModel> GetAll()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PayrollArchiveHeaderModel, dynamic>("dbo.spPayrollAccounting_GetById", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public PayrollArchiveHeaderModel GetById(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PayrollArchiveHeaderModel, dynamic>("dbo.spPayrollAccounting_GetById", new { Id = id })
                        .FirstOrDefault();

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Insert(PayrollArchiveHeaderModel payroll)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPayrollAccounting_Insert", payroll);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Update(PayrollArchiveHeaderModel payroll)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPayrollAccounting_Update", payroll);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<PayrollArchiveHeaderModel, dynamic>("dbo.spPayrollAccounting_Delete", new { Id = id });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
