using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BookUraRestEndpoint : IBookUraRestEndpoint
    {
        private readonly IApiService _apiService;

        public BookUraRestEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostPrimke(List<BookUraRestModel> list)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/UraRest", list))
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

        public async Task<List<BookUraRestModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/UraRest"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BookUraRestModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<BookUraRestModel>> GetDiscounts()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/UraRest/Discounts"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BookUraRestModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> MarkAsProcessed(int numberInUra)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync($"/api/UraRest/Processed", numberInUra))
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

        public async Task PostRow(BookUraRestModel selectedUraPrimke)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync($"/api/UraRest/Update", selectedUraPrimke))
            {
                if (response.IsSuccessStatusCode)
                {

                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
