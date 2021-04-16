using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookAccountData
    {
        List<BookAccountModel> GetAll();
    }
}