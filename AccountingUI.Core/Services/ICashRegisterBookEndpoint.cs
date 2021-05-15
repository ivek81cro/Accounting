using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface ICashRegisterBookEndpoint
    {
        Task<List<CashRegisterModel>> GetAll();
        Task<bool> PostItems(List<CashRegisterModel> list);
        Task<bool> MarkAsProcessed(int redniBroj);
    }
}