using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public class PartnersData : IPartnersData
    {
        private readonly ISqlDataAccess _sql;

        public PartnersData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<PartnersModel> GetPartners()
        {
            var output = _sql.LoadData<PartnersModel, dynamic>("dbo.spPartners_GetAll", new { }, "DefaultConnection");

            return output;
        }
    }
}