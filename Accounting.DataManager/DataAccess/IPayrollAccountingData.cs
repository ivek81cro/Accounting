using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IPayrollAccountingData
    {
        List<PayrollAccountingModel> GetAll();
        PayrollAccountingModel GetById(int id);
        void Insert(PayrollAccountingModel payroll);
        void Update(PayrollAccountingModel payroll);
        void Delete(int id);
    }
}