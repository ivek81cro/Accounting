using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IEmployeeData
    {
        void DeleteEmployee(int id);
        EmployeeModel GetEmployeeById(int id);
        List<EmployeeModel> GetEmployees();
        void InsertEmployee(EmployeeModel employee);
        void UpdateEmployee(EmployeeModel employee);
        EmployeeModel GetEmployeeByOib(string oib);
    }
}