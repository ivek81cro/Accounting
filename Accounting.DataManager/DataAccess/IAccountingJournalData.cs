using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IAccountingJournalData
    {
        List<AccountingJournalModel> GetByBookNumber(int bookNumber); 
        List<AccountingJournalModel> GetHeaders();
        void Insert(List<AccountingJournalModel> journal);
    }
}