using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class JoppdEmployeeEndpoint : IJoppdEmployeeEndpoint
    {
        private readonly IApiService _apiService;

        public JoppdEmployeeEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<JoppdEmployeeModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/JoppdEmployee"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<JoppdEmployeeModel>>();
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
