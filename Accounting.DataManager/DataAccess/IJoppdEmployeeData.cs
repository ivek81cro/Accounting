using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IJoppdEmployeeData
    {
        List<JoppdEmployeeModel> GetAll();
        void SaveData(JoppdEmployeeModel employee);
    }
}