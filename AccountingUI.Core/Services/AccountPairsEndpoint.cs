using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class AccountPairsEndpoint : IAccountPairsEndpoint
    {
        private readonly IApiService _apiService;

        public AccountPairsEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<AccountPairModel>> GetByBookName(string bookName)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/AccountPairs/{bookName}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<AccountPairModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> Post(AccountPairModel pair)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/AccountPairs", pair))
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
