using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class PayrollArchiveEndpoint : IPayrollArchiveEndpoint
    {
        private readonly IApiService _apiService;

        public PayrollArchiveEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostToArchive(PayrollArchiveModel archive)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/PayrollArchive", archive))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var error = response.ReasonPhrase;
                    return false;
                }
            }
        }
    }
}
