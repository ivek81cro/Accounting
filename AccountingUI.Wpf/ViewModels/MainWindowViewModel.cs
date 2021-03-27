using AccountingUI.Core.Events;
using AccountingUI.Core.TabControlRegion;
using LoginModule.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AccountingUI.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private IDialogService _showDialog;

        public MainWindowViewModel(IRegionManager regionManager, 
            IEventAggregator eventAggregator, IDialogService showDialog)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            ShowMenuSelectedViewCommand = new DelegateCommand<string>(Navigate);
            TabControlLoadedCommand = new DelegateCommand(OnTabControlLoaded);

            _eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(EventIsUserLoggedIn);
            _showDialog = showDialog;
        }

        public DelegateCommand<string> ShowMenuSelectedViewCommand { get; set; }
        public DelegateCommand TabControlLoadedCommand { get; private set; }

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

        private void Navigate(string param)
        {            
            var p = new NavigationParameters();
            string tabTitle = TabHeaderTitles.GetHeaderTitle(param);
            p.Add("title", tabTitle);
            _regionManager.RequestNavigate("ContentRegion", param, p);
        }

        private void OnTabControlLoaded()
        {
            _showDialog.ShowDialog(nameof(LoginView));
        }

        private void EventIsUserLoggedIn(bool value)
        {
            IsUserLoggedIn = value;
        }
    }
}
