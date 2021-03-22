using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IPartnersEndpoint
    {
        Task<List<PartnersModel>> GetAll();
        Task<PartnersModel> GetById(int id);
        Task PostPartner(PartnersModel partner);
    }
}