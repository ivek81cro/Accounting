using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookUraRestEndpoint
    {
        Task<List<BookUraRestModel>> GetAll();
        Task<bool> PostPrimke(List<BookUraRestModel> list);
        Task<List<BookUraRestModel>> GetDiscounts();
        Task<bool> MarkAsProcessed(int numberInUra);
        Task PostRow(BookUraRestModel selectedUraPrimke);
    }
}