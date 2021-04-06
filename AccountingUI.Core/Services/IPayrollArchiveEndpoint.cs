using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IPayrollArchiveEndpoint
    {
        Task<bool> PostToArchive(PayrollArchiveModel archive);
        Task<bool> IfIdentifierExists(string identifier);
        Task<List<PayrollArchiveHeaderModel>> GetArchiveHeaders();
        Task<List<PayrollArchivePayrollModel>> GetArchivePayrolls(int accountingId);
        Task<List<PayrollArchiveSupplementModel>> GetArchiveSupplements(int accountingId);
        void DeleteRecord(int accountingId);
    }
}