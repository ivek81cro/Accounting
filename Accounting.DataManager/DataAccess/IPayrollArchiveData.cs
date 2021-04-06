using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IPayrollArchiveData
    {
        void Insert(PayrollArchiveModel archive);
        bool IfExists(string identifier);
    }
}