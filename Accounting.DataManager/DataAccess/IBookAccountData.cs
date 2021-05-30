using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookAccountData
    {
        List<BookAccountModel> GetAll();
        bool Exists(BookAccountModel account);
        void Insert(BookAccountModel account);
        void Update(BookAccountModel account);
        BookAccountModel GetByNumber(string number);
    }
}