using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IBookRetailEndpoint
    {
        Task<bool> Post(List<RetailIraModel> list);
        Task<List<RetailIraModel>> GetAll();
        Task<bool> MarkAsProcessed(int redniBroj);
    }
}