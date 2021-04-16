using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BookAccountsEndpoint : IBookAccountsEndpoint
    {
        private readonly IApiService _apiService;

        public BookAccountsEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<BookAccountModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/BookAccounts"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BookAccountModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
