using Accounting.DataManager.Models;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class VatArchiveData : IVatArchiveData
    {
        private readonly ISqlDataAccess _sql;

        public VatArchiveData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public VatModel GetByPeriod(VatModel vatData)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                var vatCalculation = _sql.LoadDataInTransaction<VatModel, dynamic>("dbo.spVatArchive_CalculateForPeriod",
                    new { DateFrom = vatData.DateFrom, DateTo = vatData.DateTo }).FirstOrDefault();
                vatCalculation.DateFrom = vatData.DateFrom;
                vatCalculation.DateTo = vatData.DateTo;

                return vatCalculation;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
