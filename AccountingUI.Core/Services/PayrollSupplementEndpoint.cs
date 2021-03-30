using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class PayrollSupplementEndpoint : IPayrollSupplementEndpoint
    {
        private readonly IApiService _apiService;

        public PayrollSupplementEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<PayrollSupplementModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/PayrollSupplement"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PayrollSupplementModel>>();
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
