using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IPartnersData
    {
        List<PartnersModel> GetPartners();
        PartnersModel GetPartnersById(int id);
        void InsertPartner(PartnersModel partner);
        void UpdatePartner(PartnersModel partner);
        void DeletePartner(int id);
    }
}
