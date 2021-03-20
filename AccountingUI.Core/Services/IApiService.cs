using AccountingUI.Core.Models;
using System.Threading.Tasks;

namespace AccountingUI.Core.Service
{
    public interface IApiService
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}