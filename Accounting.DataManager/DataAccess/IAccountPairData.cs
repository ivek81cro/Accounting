using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IAccountPairData
    {
        List<AccountPairModel> GetByBookName(string bookName);
        void Insert(AccountPairModel pair);
    }
}