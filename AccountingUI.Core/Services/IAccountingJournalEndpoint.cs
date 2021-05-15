using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IAccountingJournalEndpoint
    {
        Task<bool> Post(List<AccountingJournalModel> pair);
        Task<List<AccountingJournalModel>> LoadUnprocessedJournals();
        Task<List<AccountingJournalModel>> LoadJournalDetails(AccountingJournalModel model);
    }
}