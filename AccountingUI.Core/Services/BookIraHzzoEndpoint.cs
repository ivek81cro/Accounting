using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BookIraHzzoEndpoint : IBookIraHzzoEndpoint
    {
        private readonly IApiService _apiService;

        public BookIraHzzoEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostPayments(List<BookIraHzzoModel> payments)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/BookIraHzzo", payments))
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

        public async Task<List<BookIraHzzoModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/BookIraHzzo"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BookIraHzzoModel>>();
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
