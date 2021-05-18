using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class ProcessToJournalService : IProcessToJournalService
    {
        private readonly IAccountingJournalEndpoint _accountingJournalEndpoint;

        public ProcessToJournalService(IAccountingJournalEndpoint accountingJournalEndpoint)
        {
            _accountingJournalEndpoint = accountingJournalEndpoint;
        }

        private List<AccountingJournalModel> _entries = new();

        public async Task<bool> ProcessEntries(List<AccountingJournalModel> entries)
        {
            _entries = entries;
            if (CanProcess())
            {
                return await _accountingJournalEndpoint.Post(_entries);
            }

            return false;
        }

        private bool CanProcess()
        {
            if (_entries != null)
            {
                foreach (var entry in _entries)
                {
                    if (entry.Konto == null)
                    {
                        return false;
                    }
                    else if (entry.Konto.Length < 3)
                    {
                        return false;
                    }
                    else
                    {

                    }
                }
            }

            return true;
        }
    }
}
