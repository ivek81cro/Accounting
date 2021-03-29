using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface ICountyEndpoint
    {
        Task DeleteCounty(int id);
        Task<List<CountyModel>> GetAll();
        Task<CountyModel> GetById(int id);
        Task PostCounty(CountyModel county);
    }
}