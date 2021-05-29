using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IAccountingJournalData
    {
        List<AccountingJournalModel> GetByBookNumber(int bookNumber); 
        List<AccountingJournalModel> GetHeaders();
        void Insert(List<AccountingJournalModel> journal);
        List<AccountingJournalModel> GetUnprocessedHeaders();
        List<AccountingJournalModel> GetJournalDetails(AccountingJournalModel model);
        void DeleteJournal(AccountingJournalModel model);
        int LatestNumber();
        void Update(List<AccountingJournalModel> list);
        List<AccountingJournalModel> GetProcessedHeaders();
    }
}