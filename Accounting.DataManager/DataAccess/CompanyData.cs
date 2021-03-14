using Accounting.DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataManager.DataAccess
{
    public class CompanyData : ICompanyData
    {
        private ISqlDataAccess _sql;

        public CompanyData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public CompanyModel GetCompany()
        {
            var output = _sql.LoadData<CompanyModel, dynamic>("dbo.spCompany_Get", new { }, "AccountingConnStr")
                .FirstOrDefault();

            return output;
        }
    }
}
