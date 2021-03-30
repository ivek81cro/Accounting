using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IPayrollData
    {
        void DeletePayroll(string oib);
        List<PayrollModel> GetAll();
        void InsertPayroll(PayrollModel payroll);
        void UpdatePayroll(PayrollModel payroll);
        PayrollModel GetByOib(string oib);
    }
}