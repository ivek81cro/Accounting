using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public class DatabaseBackupEndpoint : IDatabaseBackupEndpoint
    {
        private readonly IApiService _apiService;

        public DatabaseBackupEndpoint(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task CreateBackup()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/DbBackup/Create"))
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

        public async Task<List<DatabaseBackupModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiService.ApiClient.GetAsync("/api/DbBackup"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DatabaseBackupModel>>();
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
