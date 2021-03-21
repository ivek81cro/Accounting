using AccountingUI.Core.Events;
using AccountingUI.Core.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace AccountingUI.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly ILoggedInUserModel _loggedInUser;
        private readonly IEventAggregator _eventAggregator;
        public MainWindowViewModel(ILoggedInUserModel loggedInUser, IRegionManager regionManager, 
            IEventAggregator eventAggregator)
        {
            _loggedInUser = loggedInUser;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            ShowMenuSelectedViewCommand = new DelegateCommand<string>(Navigate);
            _eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(EventIsUserLoggedIn);
        }

        public DelegateCommand<string> ShowMenuSelectedViewCommand { get; set; }
        private void Navigate(string param)
        {
            _regionManager.RequestNavigate("ContentRegion", param);
        }

        private void EventIsUserLoggedIn(bool value)
        {
            IsUserLoggedIn = value;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }

        private bool _isUserLoggedIn = false;
        public bool IsUserLoggedIn
        {
            get { return _isUserLoggedIn; }
            set 
            { 
                SetProperty(ref _isUserLoggedIn, value);
                RaisePropertyChanged(nameof(IsUserLoggedIn));
            }
        }


    }
}
