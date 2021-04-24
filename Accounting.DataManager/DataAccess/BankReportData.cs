using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class BankReportData : IBankReportData
    {
        private readonly ISqlDataAccess _sql;

        public BankReportData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public int GetHeaderId(int reportNumber)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<int, dynamic>("dbo.spBankReports_GetReportId", new { RedniBroj = reportNumber }).First();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void InsertHeader(BankReportModel header)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBankReports_InsertHeader", header);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void InsertItems(List<BankReportItemModel> items)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spBankReports_InsertItems", items);
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

                _sql.LoadDataInTransaction<CityModel, dynamic>("dbo.spBankReports_Delete", new { Id = id });

                _sql.CommitTransaction();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<BankReportItemModel> GetItems(int headerId)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BankReportItemModel, dynamic>("dbo.spBankReports_GetItems", new { IdIzvod = headerId });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public BankReportItemModel GetHeader(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BankReportItemModel, dynamic>("dbo.spBankReports_GetHeader", new { Id = id }).FirstOrDefault();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<BankReportModel> GetAllHeaders()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<BankReportModel, dynamic>("dbo.spBankReports_GetAllHeaders", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
