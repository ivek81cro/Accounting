using Accounting.DataManager.Models;

namespace Accounting.DataManager.DataAccess
{
    public interface IPayrollArchiveData
    {
        void Insert(PayrollArchiveModel archive);
    }
}