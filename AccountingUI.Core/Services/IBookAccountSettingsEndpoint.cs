using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookAccountSettingsEndpoint
    {
        Task<List<BookAccountsSettingsModel>> GetByBookName(string name);
        Task<bool> PostSetting(BookAccountsSettingsModel setting);
        Task Delete(int id);
    }
}