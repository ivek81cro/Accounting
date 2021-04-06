using AccountingUI.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PayrollModule.ServiceLocal
{
    public interface IPayrollArchivePrepare
    {
        PayrollArchiveModel Process(ObservableCollection<PayrollArchivePayrollModel> payrollCalculations, 
            ObservableCollection<PayrollArchiveSupplementModel> supplementCalculations, 
            PayrollArchiveHeaderModel payrollAccounting);
        Task<bool> SaveToDatabase(PayrollArchiveModel archive);
    }
}