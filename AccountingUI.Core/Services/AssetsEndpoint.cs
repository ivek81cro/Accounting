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

        public async Task<List<AssetModel>> GetAssets()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync($"/api/Assets/"))
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
    }
}
