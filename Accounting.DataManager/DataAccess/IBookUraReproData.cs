using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookUraReproData
    {
        List<BookUraPrimkaReproModel> GetAll();
        void InsertPrimke(List<BookUraPrimkaReproModel> primke);
        void SetProcessed(int number);
    }
}