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

        public List<PayrollArchiveHeaderModel> GetHeaders()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<PayrollArchiveHeaderModel, dynamic>("spPayrollArchive_GetHeaders", new { });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<PayrollArchivePayrollModel> GetArchivePayrolls(int accountingId)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<PayrollArchivePayrollModel, dynamic>("spPayrollArchive_GetPayrolls", new { AccountingId = accountingId });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<PayrollArchiveSupplementModel> GetArchiveSupplements(int accountingId)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<PayrollArchiveSupplementModel, dynamic>("spPayrollArchive_GetSupplements", new { AccountingId = accountingId });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public bool IfExists(string identifier)
        {
            bool result;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                result = _sql.LoadDataInTransaction<int, dynamic>("spPayrollArchive_IfExists", new { UniqueId = identifier }).Count() != 0;
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();

            return result;
        }

        public void Insert(PayrollArchiveModel archive)
        {
            InsertAccountingHeader(archive.Calculation);

            int id = GetLatestId();

            InsertPayrollArchive(archive.Payrolls, id);

            if (archive.Supplements != null)
            {
                InsertSupplementArchive(archive.Supplements, id); 
            }
        }

        private int GetLatestId()
        {
            int result = 0;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                result = _sql.LoadDataInTransaction<int, dynamic>("spPayrollArchive_GetLatestId", new { }).First();
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();

            return result;
            
        }

        private void InsertAccountingHeader(PayrollArchiveHeaderModel accounting)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPayrollArchiveHeader_Insert", accounting);
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();
        }

        private void InsertPayrollArchive(List<PayrollArchivePayrollModel> payrolls, int id)
        {
            foreach (var p in payrolls)
            {
                p.AccountingId = id;
                try
                {
                    _sql.StartTransaction("AccountingConnStr");

                    _sql.SaveDataInTransaction("dbo.spPayrollArchivePayroll_Insert", p);
                }
                catch (Exception)
                {
                    _sql.RollBackTransaction();
                    throw;
                }
                _sql.Dispose();
            }
        }

        private void InsertSupplementArchive(List<PayrollArchiveSupplementModel> supplements, int id)
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

        public void DeleteRecord(int accountingId)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<PayrollArchiveHeaderModel, dynamic>("dbo.spPayrollArchive_Delete", new { Id = accountingId });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();
        }
    }
}
