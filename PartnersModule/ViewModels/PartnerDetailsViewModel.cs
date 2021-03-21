using AccountingUI.Core.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PartnersModule.ViewModels
{
    public class PartnerDetailsViewModel : BindableBase, INavigationAware
    {
        private PartnersModel _selectedPartner;
        public PartnersModel SelectedPartner
        {
            get { return _selectedPartner; }
            set { SetProperty(ref _selectedPartner, value); }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var partner = navigationContext.Parameters["partner"] as PartnersModel;
            if(partner != null)
            {
                return SelectedPartner != null && SelectedPartner.Naziv == partner.Naziv;
            }
            else
            {
                return true;
            }
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var partner = navigationContext.Parameters["partner"] as PartnersModel;
            if (partner != null)
            {
                SelectedPartner = partner;
            }
        }
    }
}
