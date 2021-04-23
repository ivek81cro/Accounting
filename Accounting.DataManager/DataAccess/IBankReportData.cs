using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBankReportData
    {
        int GetHeaderId(int reportNumber);
        void InsertHeader(BankReportModel header);
        void InsertItems(List<BankReportItemModel> items);
        void Delete(int id);
        List<BankReportItemModel> GetItems(int reportNumber);
        BankReportItemModel GetHeader(int reportNumber);
        List<BankReportModel> GetAllHeaders();
    }
}