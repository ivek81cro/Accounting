using Accounting.DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class PayrollArchiveData : IPayrollArchiveData
    {
        private readonly ISqlDataAccess _sql;

        public PayrollArchiveData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void Insert(PayrollArchiveModel archive)
        {
            InsertAccountingHeader(archive.Calculation);

            int id = GetLatestId();

            InsertPayrollArchive(archive.Payrolls, id);

            InsertSupplementArchive(archive.Supplements, id);
        }

        private int GetLatestId()
        {
            int result = 0;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                result = _sql.LoadDataInTransaction<int, dynamic>("spPayrollArchive_GetLatestId", new { }).First();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();

            return result;
            
        }

        private void InsertAccountingHeader(PayrollAccountingModel accounting)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPayrollArchiveHeader_Insert", accounting);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();
        }

        private void InsertPayrollArchive(List<PayrollCalculationModel> payrolls, int id)
        {
            foreach (var p in payrolls)
            {
                p.AccountingId = id;
                try
                {
                    _sql.StartTransaction("AccountingConnStr");

                    _sql.SaveDataInTransaction("dbo.spPayrollArchivePayroll_Insert", p);
                }
                catch (System.Exception)
                {
                    _sql.RollBackTransaction();
                    throw;
                }
                _sql.Dispose();
            }
        }

        private void InsertSupplementArchive(List<PayrollSupplementCalculationModel> supplements, int id)
        {
            foreach (var s in supplements)
            {
                s.AccountingId = id;
                try
                {
                    _sql.StartTransaction("AccountingConnStr");

                    _sql.SaveDataInTransaction("dbo.spPayrollArchiveSupplement_Insert", s);
                }
                catch (System.Exception)
                {
                    _sql.RollBackTransaction();
                    throw;
                }
                _sql.Dispose();
            }
        }
    }
}
