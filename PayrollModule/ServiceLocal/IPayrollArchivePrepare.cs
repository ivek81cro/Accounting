using AccountingUI.Core.Models;
using System.Collections.ObjectModel;

namespace PayrollModule.ServiceLocal
{
    public interface IPayrollArchivePrepare
    {
        PayrollArchiveModel Process(ObservableCollection<PayrollCalculationModel> payrollCalculations, ObservableCollection<PayrollSupplementCalculationModel> supplementCalculations, PayrollAccountingModel payrollAccounting);
        void SaveToDatabase(PayrollArchiveModel archive);
    }
}