using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class BankReportEndpoint : IBankReportEndpoint
    {
        private readonly IApiService _apiService;

        public BankReportEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<int> GetHeaderId(int reportNumber)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/BankReport/{reportNumber}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<int>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<BankReportModel> GetHeader(int id)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/BankReport/GetHeader/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<BankReportModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<BankReportModel>> GetAllHeaders()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/BankReport"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BankReportModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> PostHeader(BankReportModel header)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/BankReport/Header", header))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task UpdateHeader(BankReportModel reportHeader)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/BankReport/Update", reportHeader))
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

        public async Task<List<BankReportItemModel>> GetItems(int headerId)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/BankReport/GetItems/{headerId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<BankReportItemModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> PostItems(List<BankReportItemModel> items)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/BankReport/Items", items))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Delete(int id)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.DeleteAsync($"/api/BankReport/{id}"))
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
