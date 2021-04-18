using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookIraEndpoint
    {
        Task<List<BookIraModel>> GetAll();
        Task<bool> PostPrimke(List<BookIraModel> primke);
    }
}