using AccountingUI.Core.Models;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface ICompanyEndpoint
    {
        Task<CompanyModel> Get();
        Task PostCompany(CompanyModel company);
    }
}