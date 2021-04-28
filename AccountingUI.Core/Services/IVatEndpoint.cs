using AccountingUI.Core.Models;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface IVatEndpoint
    {
        Task<VatModel> GetVatForPeriod(VatModel vatData);
    }
}