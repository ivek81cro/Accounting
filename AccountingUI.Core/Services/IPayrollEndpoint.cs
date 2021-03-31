using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IPayrollEndpoint
    {
        Task DeletePayroll(string oib);
        Task<List<PayrollModel>> GetAll();
        Task<PayrollModel> GetByOib(string oib);
        Task<bool> PostPayroll(PayrollModel payroll);
    }
}