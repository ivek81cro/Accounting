using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class CashRegisterBookEndpoint : ICashRegisterBookEndpoint
    {
        private readonly IApiService _apiService;

        public CashRegisterBookEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostItems(List<CashRegisterModel> list)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/CashRegister", list))
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

        public async Task<List<CashRegisterModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/CashRegister"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<CashRegisterModel>>();
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
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync($"/api/CashRegister/Processed", number))
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
