using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBankReportEndpoint
    {
        Task<int> GetHeaderId(int reportNumber);
        Task<List<int>> GetProcessed();
        Task<bool> PostHeader(BankReportModel header);
        Task<bool> PostItems(List<BankReportItemModel> items);
        Task<BankReportModel> GetHeader(int id);
        Task<List<BankReportItemModel>> GetItems(int headerId);
        Task<List<BankReportModel>> GetAllHeaders();
        Task UpdateHeader(BankReportModel reportHeader);
        Task Delete(int id);
    }
}