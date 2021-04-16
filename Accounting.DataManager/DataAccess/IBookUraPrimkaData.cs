using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookUraPrimkaData
    {
        void InsertPrimke(List<BookUraPrimkaModel> primke);
        List<BookUraPrimkaModel> GetAll();
    }
}