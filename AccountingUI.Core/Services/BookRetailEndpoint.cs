using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BookRetailEndpoint : IBookRetailEndpoint
    {
        private readonly IApiService _apiService;

        public BookRetailEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> Post(List<RetailIraModel> list)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/BookRetail", list))
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

        public async Task<List<RetailIraModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/BookRetail"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<RetailIraModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> MarkAsProcessed(int number)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync($"/api/BookRetail/Processed", number))
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
