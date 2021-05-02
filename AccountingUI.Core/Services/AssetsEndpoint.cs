using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class AssetsEndpoint : IAssetsEndpoint
    {
        private readonly IApiService _apiService;

        public AssetsEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<AssetModel>> GetAssets(string vrstaImovine)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/Assets/{vrstaImovine}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<AssetModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> Insert(AssetModel asset)
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.PostAsJsonAsync("/api/Assets", asset))
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
    }
}
