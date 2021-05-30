using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookAccountsEndpoint
    {
        Task<List<BookAccountModel>> GetAll();
        Task<BookAccountModel> GetByNumber(string number);
        Task<bool> Insert(BookAccountModel account);
    }
}