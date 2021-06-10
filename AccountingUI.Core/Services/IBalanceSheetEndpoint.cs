using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBalanceSheetEndpoint
    {
        Task<List<BalanceSheetModel>> LoadFullBalanceSheet();
    }
}