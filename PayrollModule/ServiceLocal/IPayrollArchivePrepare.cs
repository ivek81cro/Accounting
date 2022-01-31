using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayrollModule.ServiceLocal
{
    public interface IPayrollArchivePrepare
    {
        PayrollArchiveModel Process(List<PayrollArchivePayrollModel> payrollCalculations,
                                           List<PayrollArchiveSupplementModel> supplementCalculations,
                                           PayrollArchiveHeaderModel payrollAccounting);

        Task<bool> SaveToDatabase(PayrollArchiveModel archive);

        Task<List<AccountingJournalModel>> CreateJournalEntries(List<PayrollArchivePayrollModel> payrolls,
                                                                PayrollArchiveHeaderModel selectedArchive,
                                                                List<BookAccountsSettingsModel> accountingSettings,
                                                                List<PayrollArchiveSupplementModel> supplements);
    }
}