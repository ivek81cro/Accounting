using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BalanceSheetEndpoint : IBalanceSheetEndpoint
    {
        private readonly IApiService _apiService;

        public BalanceSheetEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<BalanceSheetModel>> LoadFullBalanceSheet()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/Balance"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BalanceSheetModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<BalanceSheetModel>> LoadPeriodBalanceSheet(List<DateTime> dates)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync($"/api/Balance", dates))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BalanceSheetModel>>();
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
