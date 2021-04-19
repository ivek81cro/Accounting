using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IAccountPairsEndpoint
    {
        Task<List<AccountPairModel>> GetByBookName(string bookName);
        Task<bool> Post(AccountPairModel pair);
    }
}