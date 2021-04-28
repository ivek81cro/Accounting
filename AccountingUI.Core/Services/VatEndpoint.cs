using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class VatEndpoint : IVatEndpoint
    {
        private readonly IApiService _apiService;

        public VatEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<VatModel> GetVatForPeriod(VatModel vatData)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/Vat/Period", vatData))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<VatModel>();
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
