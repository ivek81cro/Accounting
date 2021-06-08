using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BookIraEndpoint : IBookIraEndpoint
    {
        private readonly IApiService _apiService;

        public BookIraEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostPrimke(List<BookIraModel> primke)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/BookIra", primke))
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

        public async Task<List<BookIraModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/BookIra"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BookIraModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> MarkAsProcessed(int numberInIra)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync($"/api/BookIra/Processed", numberInIra))
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

        public async Task PostRow(BookIraModel selectedIra)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync($"/api/BookIra/Update", selectedIra))
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
