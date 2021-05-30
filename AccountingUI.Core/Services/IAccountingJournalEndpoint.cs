using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IAccountingJournalEndpoint
    {
        Task<bool> Post(List<AccountingJournalModel> pair);
        Task<List<AccountingJournalModel>> LoadUnprocessedJournals();
        Task<List<AccountingJournalModel>> LoadJournalDetails(AccountingJournalModel model);
        Task Delete(AccountingJournalModel accountingJournalModel);
        Task<int> LatestJournalNumber();
        Task<List<AccountingJournalModel>> LoadProcessedJournals();
        Task<bool> Update(List<AccountingJournalModel> list);
        Task<List<AccountBalanceModel>> LoadAccountCard(string accountNumber);
    }
}