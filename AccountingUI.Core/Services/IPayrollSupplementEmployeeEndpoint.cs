using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IPayrollSupplementEmployeeEndpoint
    {
        Task DeleteSupplement(int id);
        Task<List<PayrollSupplementEmployeeModel>> GetAll();
        Task<List<PayrollSupplementEmployeeModel>> GetByOib(string oib);
        Task PostSupplement(PayrollSupplementEmployeeModel supplement);
    }
}