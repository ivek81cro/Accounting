using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class EmployeeEndpoint : IEmployeeEndpoint
    {
        private readonly IApiService _apiService;

        public EmployeeEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<EmployeeModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/Employee"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<EmployeeModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<EmployeeModel> GetById(int id)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/Employee/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<EmployeeModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<EmployeeModel> GetByOib(string oib)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/Employee/Oib/{oib}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<EmployeeModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> PostEmployee(EmployeeModel employee)
        {
            if(employee.DatumOdlaska == null)
            {
                employee.DatumOdlaska = new DateTime(day: 1, month: 1, year: 1900);
            }

            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/Employee", employee))
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

        public async Task DeleteEmployee(int id)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.DeleteAsync($"/api/Employee/{id}"))
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
