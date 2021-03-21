using AccountingUI.Core.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Service
{
    public interface IApiService
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}