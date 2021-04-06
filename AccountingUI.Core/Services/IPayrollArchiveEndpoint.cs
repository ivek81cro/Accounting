using AccountingUI.Core.Models;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IPayrollArchiveEndpoint
    {
        Task<bool> PostToArchive(PayrollArchiveModel archive);
        Task<bool> IfIdentifierExists(string identifier);
    }
}