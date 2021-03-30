using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class PayrollSupplementEmployeeEndpoint : IPayrollSupplementEmployeeEndpoint
    {
        private readonly IApiService _apiService;

        public PayrollSupplementEmployeeEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<PayrollSupplementEmployeeModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/EmployeeSupplement"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PayrollSupplementEmployeeModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task PostSupplement(PayrollSupplementEmployeeModel supplement)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/EmployeeSupplement", supplement))
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

        public async Task DeleteSupplement(int id)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.DeleteAsync($"/api/EmployeeSupplement/{id}"))
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
