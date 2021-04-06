using AccountingUI.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PayrollModule.ServiceLocal
{
    public interface IPayrollArchivePrepare
    {
        PayrollArchiveModel Process(ObservableCollection<PayrollCalculationModel> payrollCalculations, 
            ObservableCollection<PayrollSupplementCalculationModel> supplementCalculations, 
            PayrollAccountingModel payrollAccounting);
        Task<bool> SaveToDatabase(PayrollArchiveModel archive);
    }
}