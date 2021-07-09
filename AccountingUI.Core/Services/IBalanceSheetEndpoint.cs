using AccountingUI.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBalanceSheetEndpoint
    {
        Task<List<BalanceSheetModel>> LoadFullBalanceSheet();
        Task<List<BalanceSheetModel>> LoadPeriodBalanceSheet(List<DateTime> dates);
    }
}