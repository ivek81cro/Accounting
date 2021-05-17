using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookIraRetailData
    {
        List<RetailIraModel> GetAll();
        void Insert(List<RetailIraModel> list);
        void SetProcessed(int number);
    }
}