using AccountingUI.Core.Events;
using AccountingUI.Core.Models;
using Microsoft.Extensions.Configuration;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccountingUI.Core.Service
{
    public class ApiService : IApiService
    {
        private HttpClient _apiClient;
        private ILoggedInUserModel _loggedInUserModel;
        private IEventAggregator _eventAggregator;
        private IConfiguration _config;

        public ApiService(ILoggedInUserModel loggedInUserModel, IEventAggregator eventAggregator, IConfiguration config)
        {
            InitializeClient();
            _loggedInUserModel = loggedInUserModel;
            _eventAggregator = eventAggregator;
            _config = config;
        }

        public HttpClient ApiClient
        {
            get
            {
                return _apiClient;
            }
        }

        private void InitializeClient()
        {
            string api = _config.GetValue<string>("uri"); ;

            _apiClient = new HttpClient
            {
                BaseAddress = new Uri(api)
            };
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type", "password"),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password),
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUserModel.CreatedDate = result.CreatedDate;
                    _loggedInUserModel.EmailAddress = result.EmailAddress;
                    _loggedInUserModel.FirstName = result.FirstName;
                    _loggedInUserModel.Id = result.Id;
                    _loggedInUserModel.LastName = result.LastName;
                    _loggedInUserModel.Token = token;

                    SendUserStatusMessage();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private void SendUserStatusMessage()
        {
            bool message = false;
            if(_loggedInUserModel.Id != null)
            {
                message = true;
            }
            _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(message);
        }
    }
}
