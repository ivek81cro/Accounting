using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BookUraEndpoint : IBookUraEndpoint
    {
        private readonly IApiService _apiService;

        public BookUraEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostPrimke(List<BookUraPrimkaModel> primke)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/UraPrimka", primke))
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

        public async Task<List<BookUraPrimkaModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/UraPrimka"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BookUraPrimkaModel>>();
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
