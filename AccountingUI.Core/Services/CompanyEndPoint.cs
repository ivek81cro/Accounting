using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class CompanyEndpoint : ICompanyEndpoint
    {
        private readonly IApiService _apiService;

        public CompanyEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<CompanyModel> Get()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/Company"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<CompanyModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task PostCompany(CompanyModel company)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/Company", company))
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
