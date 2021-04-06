using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IPayrollAccountingData
    {
        List<PayrollArchiveHeaderModel> GetAll();
        PayrollArchiveHeaderModel GetById(int id);
        void Insert(PayrollArchiveHeaderModel payroll);
        void Update(PayrollArchiveHeaderModel payroll);
        void Delete(int id);
    }
}