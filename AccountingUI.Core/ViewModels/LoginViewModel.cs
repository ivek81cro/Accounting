using MvvmCross.Commands;
using MvvmCross.ViewModels;
using AccountingUI.Core.Models;
using System.Collections.ObjectModel;

namespace AccountingUI.Core.ViewModels
{
    class LoginViewModel : MvxViewModel
    {
        public IMvxCommand AddUserCommand { get; set; }
        private ObservableCollection<UserModel> _user = new();

        public LoginViewModel()
        {
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
        }
    }
}
