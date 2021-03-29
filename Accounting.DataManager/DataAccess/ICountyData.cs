using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface ICountyData
    {
        void DeleteCounty(int id);
        List<CountyModel> GetCounties();
        CountyModel GetCountyById(int id);
        void InsertCounty(CountyModel county);
        void UpdateCounty(CountyModel county);
    }
}