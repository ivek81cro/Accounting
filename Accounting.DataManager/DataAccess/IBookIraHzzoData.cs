using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookIraHzzoData
    {
        List<BookIraHzzoModel> GetAll();
        void Insert(List<BookIraHzzoModel> paymebts);
    }
}