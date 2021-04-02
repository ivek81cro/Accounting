using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IEmployeeSupplementData
    {
        void DeleteSupplement(int id);
        List<PayrollSupplementEmployeeModel> GetAll();
        List<PayrollSupplementEmployeeModel> GetByOib(string oib);
        void InsertSupplement(PayrollSupplementEmployeeModel supplement);
    }
}