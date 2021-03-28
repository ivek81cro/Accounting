using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface ICityEndpoint
    {
        Task DeleteCity(int id);
        Task<List<CityModel>> GetAll();
        Task<CityModel> GetById(int id);
        Task PostCity(CityModel city);
    }
}