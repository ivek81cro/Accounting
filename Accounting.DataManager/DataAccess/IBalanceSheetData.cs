using Accounting.DataManager.Models;
using System;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBalanceSheetData
    {
        List<BalanceSheetModel> GetBalance();
        List<BalanceSheetModel> GetBalancePeriod(List<DateTime> dates);
    }
}