using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class AccountingJournalEndpoint : IAccountingJournalEndpoint
    {
        private readonly IApiService _apiService;

        public AccountingJournalEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> Post(List<AccountingJournalModel> list)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/Journal", list))
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
