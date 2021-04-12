using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IEmployeeEndpoint
    {
        Task DeleteEmployee(int id);
        Task<List<EmployeeModel>> GetAll();
        Task<EmployeeModel> GetById(int id);
        Task<bool> PostEmployee(EmployeeModel partner);
        Task<EmployeeModel> GetByOib(string oib);
    }
}