using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class TravelOrdersEndpoint : ITravelOrdersEndpoint
    {
        private readonly IApiService _apiService;

        public TravelOrdersEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> PostLocoTravel(TravelOrdersLocoModel locoTravelOrder)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/TravelOrders/LocoTravel", locoTravelOrder))
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

        public async Task<List<LocoCalculationModel>> GetLocoCalculations()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/TravelOrders/LocoTravel"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<LocoCalculationModel>>();
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
