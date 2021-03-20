using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace AccountingUI.MainWindowModule.ViewModels
{
    public class LoginWindowViewModel : BindableBase, IDialogAware
    {
        private string _title = "Knjigovodstvo - Prijava korisnika";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _userName = "mariocro11@gmail.com";
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _errorMessage;


        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        

        public event Action<IDialogResult> RequestClose;
        private void ExecuteDialogCloseCommand()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }


        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public bool CanCloseDialog()
        {
            return true;
        }
    }
}
