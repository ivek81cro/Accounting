using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IProcessToJournalService
    {
        Task<bool> ProcessEntries(List<AccountingJournalModel> entries);
    }
}