using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IPayrollSupplementEndpoint
    {
        Task<List<PayrollSupplementModel>> GetAll();
    }
}