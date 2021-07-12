using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookIraHzzoEndpoint
    {
        Task<List<BookIraHzzoModel>> GetAll();
        Task<bool> PostPayments(List<BookIraHzzoModel> payments);
    }
}