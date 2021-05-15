using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookUraRestData
    {
        List<BookUraRestModel> GetAll();
        void Insert(List<BookUraRestModel> data);
        List<BookUraRestModel> GetDiscounts();
        void SetProcessed(int uraNumber);
    }
}