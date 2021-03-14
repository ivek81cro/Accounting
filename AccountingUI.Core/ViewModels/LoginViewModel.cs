using MvvmCross.Commands;
using MvvmCross.ViewModels;
using AccountingUI.Core.Models;
using System.Collections.ObjectModel;
using AccountingUI.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace AccountingUI.Core.ViewModels
{
    class LoginViewModel : MvxViewModel
    {
        public IMvxCommand AddUserCommand { get; set; }

        private ObservableCollection<UserModel> _user = new();

        private IApiHelper _apiHelper;
        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;

            AddUserCommand = new MvxCommand(AddUser);
        }

        public ObservableCollection<UserModel> User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set 
            {
                SetProperty(ref _userName, value);
                RaisePropertyChanged(() => UserName);
                RaisePropertyChanged(() => CanAddUser);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set 
            { 
                SetProperty(ref _password, value);
                RaisePropertyChanged(() => Password);
                RaisePropertyChanged(() => CanAddUser);
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

            User.Add(user);

            _ = LogIn(user);
        }

        private async Task LogIn(UserModel user)
        {
            try
            {
                var result = await _apiHelper.Authenticate(user.UserName, user.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
