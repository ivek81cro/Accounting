using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface ICashRegisterData
    {
        List<CashRegisterModel> GetAll();
        void InsertItems(List<CashRegisterModel> items);
        void SetProcessed(int number);
    }
}