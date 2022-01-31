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

        public PayrollArchiveHeaderModel GetHeader(int id)
        {
            PayrollArchiveHeaderModel result;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                result = _sql.LoadDataInTransaction<PayrollArchiveHeaderModel, dynamic>("spPayrollArchive_GetHeader", new { Id = id }).FirstOrDefault();

            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();
            return result;
        }

        public List<PayrollArchivePayrollModel> GetArchivePayrolls(int accountingId)
        {
            List<PayrollArchivePayrollModel> result;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                result = _sql.LoadDataInTransaction<PayrollArchivePayrollModel, dynamic>("spPayrollArchive_GetPayrolls", new { AccountingId = accountingId });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();

            return result;
        }

        public List<PayrollArchiveSupplementModel> GetArchiveSupplements(int accountingId)
        {
            List<PayrollArchiveSupplementModel> result;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                result = _sql.LoadDataInTransaction<PayrollArchiveSupplementModel, dynamic>("spPayrollArchive_GetSupplements", new { AccountingId = accountingId });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();

            return result;
        }

        public List<PayrollHours> GetArchiveHours(int accountingId)
        {
            List<PayrollHours> result;
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                result = _sql.LoadDataInTransaction<PayrollHours, dynamic>("spPayrollArchive_GetHours", new { Id = accountingId });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
            _sql.Dispose();

            return result;
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
            InsertAccountingHeader(archive.Header);

            int id = GetLatestId();

            InsertPayrollArchive(archive.Payrolls, id);
            InsertHoursArchive(archive.WorkedHours, id);

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
                    DeleteRecord(id);
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
                    DeleteRecord(id);
                    throw;
                }
                _sql.Dispose();
            }
        }

        private void InsertHoursArchive(List<PayrollHours> hours, int id)
        {
            foreach (var s in hours)
            {
                s.PayrollId = id;
                try
                {
                    _sql.StartTransaction("AccountingConnStr");

                    _sql.SaveDataInTransaction("dbo.spPayrollArchivePayrollHours_Insert", s);
                }
                catch (System.Exception)
                {
                    _sql.RollBackTransaction();
                    DeleteRecord(id);
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


        public void SetProcessed(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.LoadDataInTransaction<PayrollArchiveHeaderModel, dynamic>("dbo.spPayrollArchive_SetProcessed", new { Id = id });
            }
            catch (Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
