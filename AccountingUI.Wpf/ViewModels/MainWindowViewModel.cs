using AccountingUI.Core.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AccountingUI.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly ILoggedInUserModel _loggedInUser;
        public MainWindowViewModel(ILoggedInUserModel loggedInUser, IRegionManager regionManager)
        {
            _loggedInUser = loggedInUser;
            _regionManager = regionManager;

            ShowPartnersCommand = new DelegateCommand<string>(Navigate);
        }

        public DelegateCommand<string> ShowPartnersCommand { get; set; }
        private void Navigate(string param)
        {
            _regionManager.RequestNavigate("ContentRegion", param);
        }

        private bool _isUserLoggedIn = false;
        public bool IsUserLoggedIn
        {
            get { return _isUserLoggedIn; }
            set 
            { 
                SetProperty(ref _isUserLoggedIn, value);
                RaisePropertyChanged(nameof(IsUserLoggedIn));//TODO reise event from other view
            }
        }
    }
}
