using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookUraEndpoint
    {
        Task<bool> PostPrimke(List<BookUraPrimkaModel> primke);
        Task<List<BookUraPrimkaModel>> GetAll();
    }
}