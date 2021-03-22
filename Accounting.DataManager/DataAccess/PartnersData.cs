using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class PartnersData : IPartnersData
    {
        private readonly ISqlDataAccess _sql;

        public PartnersData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public PartnersModel GetPartnersById(int id)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PartnersModel, dynamic>("dbo.spPartners_GetById", new { Id = id })
                        .FirstOrDefault();

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<PartnersModel> GetPartners()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");
                var output = _sql.LoadDataInTransaction<PartnersModel, dynamic>("dbo.spPartners_GetAll", new { });

                return output;
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void SavePartnerToDatabase(PartnersModel partner)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPartners_AddNew", partner);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void UpdatePartner(PartnersModel partner)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spPartners_Update", partner);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}