using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BookAccountSettingsEndpoint : IBookAccountSettingsEndpoint
    {
        private readonly IApiService _apiService;

        public BookAccountSettingsEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostSetting(BookAccountsSettingsModel setting)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/BookSettings", setting))
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

        public async Task<List<BookAccountsSettingsModel>> GetByBookName(string name)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/BookSettings/{name}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BookAccountsSettingsModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Delete(int id)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.DeleteAsync($"/api/BookSettings/{id}"))
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
