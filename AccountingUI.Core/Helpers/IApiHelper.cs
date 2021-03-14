using AccountingUI.Core.Models;
using System.Threading.Tasks;

namespace AccountingUI.Core.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}