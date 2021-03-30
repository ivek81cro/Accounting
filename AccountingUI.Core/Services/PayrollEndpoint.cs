using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class PayrollEndpoint : IPayrollEndpoint
    {
        private readonly IApiService _apiService;

        public PayrollEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<PayrollModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/Payroll"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PayrollModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<PayrollModel> GetByOib(int oib)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/Payroll/{oib}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<PayrollModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> PostPayroll(PayrollModel payroll)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/Payroll", payroll))
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

        public async Task DeletePayroll(int oib)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.DeleteAsync($"/api/Payroll/{oib}"))
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
