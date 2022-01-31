using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IPayrollArchiveData
    {
        void Insert(PayrollArchiveModel archive);
        bool IfExists(string identifier);
        List<PayrollArchiveHeaderModel> GetHeaders();
        List<PayrollArchivePayrollModel> GetArchivePayrolls(int uniqueId);
        List<PayrollArchiveSupplementModel> GetArchiveSupplements(int accountingId);
        List<PayrollHours> GetArchiveHours(int accountingId);
        void DeleteRecord(int accountingId);
        void SetProcessed(int id);
        PayrollArchiveHeaderModel GetHeader(int id);
    }
}