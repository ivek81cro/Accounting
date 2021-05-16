using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookUraReproEndpoint
    {
        Task<List<BookUraPrimkaReproModel>> GetAll();
        Task<bool> PostPrimke(List<BookUraPrimkaReproModel> primke);
        Task MarkAsProcessed(int brojUKnjiziUra);
    }
}