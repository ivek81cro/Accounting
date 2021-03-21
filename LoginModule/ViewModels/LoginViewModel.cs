using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace LoginModule.ViewModels 
{ 
    public class LoginViewModel : BindableBase
    {
        private IApiService _apiService;
        private ILoggedInUserModel _loggedInUserModel;
        private readonly IRegionManager _regionManager;
        public LoginViewModel(IApiService apiService, ILoggedInUserModel loggedInUserModel, IRegionManager regionManager)
        {
            _apiService = apiService;
            _loggedInUserModel = loggedInUserModel;
            _regionManager = regionManager;

            LoginUser = new DelegateCommand(LogInAsync);
        }

        public DelegateCommand LoginUser { get; set; }

        private UserModel _user = new();
        public UserModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private string _userName = "mariocro11@gmail.com";
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                RaisePropertyChanged(nameof(CanAddUser));
            }
        }

        private string _password = "Pwd12345.";
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                RaisePropertyChanged(nameof(CanAddUser));
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }


        public bool CanAddUser => UserName?.Length > 0 && Password?.Length > 0;

        public void AddUser()
        {
            UserModel user = new()
            {
                UserName = UserName,
                Password = Password
            };

            UserName = string.Empty;
            Password = string.Empty;

            User = user;
        }

        private async void LogInAsync()
        {
            AddUser();
            try
            {
                ErrorMessage = "";
                var result = await _apiService.Authenticate(User.UserName, User.Password);

                //capture user info
                await _apiService.GetLoggedInUserInfo(result.Access_Token);

                if (_loggedInUserModel.Id != null)
                {
                    _regionManager.RequestNavigate("ContentRegion", "StartView");
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Prijava neuspješna.\n" + ex.Message;
            }
        }
    }
}
