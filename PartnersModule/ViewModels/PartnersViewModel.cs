using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using PartnersModule.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PartnersModule.ViewModels
{
    public class PartnersViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand<PartnersModel> PartnerSelectedCommand { get; private set; }
        public DelegateCommand NewPartnerCommand { get; private set; }

        private IPartnersEndpoint _partnersEndpoint;
        private IRegionManager _regionManager;
        private IDialogService _showDialog;

        public PartnersViewModel(IPartnersEndpoint partnersEndpoint, IRegionManager regionManager, 
            IDialogService showDialog)
        {
            _partnersEndpoint = partnersEndpoint;
            _regionManager = regionManager;
            _showDialog = showDialog;

            PartnerSelectedCommand = new DelegateCommand<PartnersModel>(PartnerSelected);
            NewPartnerCommand = new DelegateCommand(SavePartner);
        }

        private string _isActive;
        public string IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }

        private ObservableCollection<PartnersModel> _partners = new();
        public ObservableCollection<PartnersModel> Partners
        {
            get { return _partners; }
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

        private void SavePartner()
        {
            PartnersModel partner = new PartnersModel();
            var parameters = new DialogParameters();
            parameters.Add("partner", partner);
            _showDialog.ShowDialog(nameof(PartnerEdit), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    PartnersModel partner = result.Parameters.GetValue<PartnersModel>("partner");
                    _partnersEndpoint.PostPartner(partner);
                }
            });
        }
    }
}
