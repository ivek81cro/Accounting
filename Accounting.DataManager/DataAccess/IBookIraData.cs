using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookIraData
    {
        List<BookIraModel> GetAll();
        void Insert(List<BookIraModel> primke);
        void SetProcessed(int iraNumber);
    }
}