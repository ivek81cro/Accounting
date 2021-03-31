using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface ICityData
    {
        void DeleteCity(int id);
        List<CityModel> GetCities();
        CityModel GetCityById(int id);
        CityModel GetCityByName(string name);
        void InsertCity(CityModel city);
        void UpdateCity(CityModel city);
    }
}