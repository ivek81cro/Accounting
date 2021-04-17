using Accounting.DataManager.Models;
using System.Collections.Generic;

namespace Accounting.DataManager.DataAccess
{
    public interface IBookAccountSettingsData
    {
        List<BookAccountsSettingsModel> Get(string name);
        void Insert(BookAccountsSettingsModel setting);
        void Delete(int id);
    }
}