using Accounting.DataManager.Models;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class CompanyData : ICompanyData
    {
        private ISqlDataAccess _sql;

        public CompanyData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void InsertCompany(CompanyModel company)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spCompany_Insert", company);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public CompanyModel GetCompany()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<CompanyModel, dynamic>("dbo.spCompany_Get", new { })
                .FirstOrDefault();

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void UpdateCompany(CompanyModel company)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spCompany_Update", company);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
