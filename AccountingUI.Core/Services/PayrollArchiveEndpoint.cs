using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class PayrollArchiveEndpoint : IPayrollArchiveEndpoint
    {
        private readonly IApiService _apiService;

        public PayrollArchiveEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task DeleteRecord(int accountingId)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.DeleteAsync($"/api/PayrollArchive/{accountingId}"))
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

        public async Task<List<PayrollArchiveHeaderModel>> GetArchiveHeaders()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/PayrollArchive/Headers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PayrollArchiveHeaderModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<PayrollArchivePayrollModel>> GetArchivePayrolls(int accountingId)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/PayrollArchive/Payrolls/{accountingId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PayrollArchivePayrollModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<PayrollArchiveSupplementModel>> GetArchiveSupplements(int accountingId)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/PayrollArchive/Supplements/{accountingId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PayrollArchiveSupplementModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> IfIdentifierExists(string identifier)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/PayrollArchive/IfExists/{identifier}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<bool>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> PostToArchive(PayrollArchiveModel archive)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/PayrollArchive", archive))
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

        public async Task MarkAsProcessed(int id)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/PayrollArchive/Processed", id))
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
