using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IPartnersData
    {
        List<PartnersModel> GetPartners();
    }
}
