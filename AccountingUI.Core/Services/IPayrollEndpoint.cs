using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IPayrollEndpoint
    {
        Task DeletePayroll(int oib);
        Task<List<PayrollModel>> GetAll();
        Task<PayrollModel> GetByOib(int oib);
        Task<bool> PostPayroll(PayrollModel payroll);
    }
}