using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PartnersModule.ViewModels
{
    public class PartnersViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand<PartnersModel> PartnerSelectedCommand { get; private set; }
        public DelegateCommand<object> CloseTabCommand { get; set; }

        private IApiService _apiService;
        private ILoggedInUserModel _loggedInUserModel;
        private IPartnersEndpoint _partnersEndpoint;
        private IRegionManager _regionManager;

        public PartnersViewModel(IApiService apiService, ILoggedInUserModel loggedInUserModel,
            IPartnersEndpoint partnersEndpoint, IRegionManager regionManager)
        {
            _apiService = apiService;
            _loggedInUserModel = loggedInUserModel;
            _partnersEndpoint = partnersEndpoint;
            _regionManager = regionManager;

            PartnerSelectedCommand = new DelegateCommand<PartnersModel>(PartnerSelected);
            CloseTabCommand = new DelegateCommand<object>(OnExecuteCloseCommand);
        }

        private ObservableCollection<PartnersModel> _partners = new();
        public ObservableCollection<PartnersModel> Partners
        {
            get { return _partners;; }
            set 
            { 
                SetProperty(ref _partners, value);
                RaisePropertyChanged(nameof(Partners));
            }
        }

        private async Task LoadPartners()
        {
            var partnersList = await _partnersEndpoint.GetAll();
            Partners = new ObservableCollection<PartnersModel>(partnersList);
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await LoadPartners();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private void PartnerSelected(PartnersModel partner)
        {
            var param = new NavigationParameters();
            param.Add("partner", partner);

            if(partner != null)
            {
                _regionManager.RequestNavigate("PartnerDetailsRegion", "PartnerDetails", param);
            }
        }
        //TODO: resolve closing dynymically created tabitem's
        private void OnExecuteCloseCommand(object tabItem)
        {
        //    _regionManager.Regions["PartnerDetailsRegion"].Remove(tabItem);
        }
    }
}
