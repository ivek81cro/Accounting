using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using Prism.Mvvm;

namespace PartnersModule.ViewModels
{
    public class PartnersViewModel : BindableBase
    {
        private IApiService _apiService;
        private ILoggedInUserModel _loggedInUserModel;

        public PartnersViewModel(IApiService apiService, ILoggedInUserModel loggedInUserModel)
        {
            _apiService = apiService;
            _loggedInUserModel = loggedInUserModel;
        }
    }
}
