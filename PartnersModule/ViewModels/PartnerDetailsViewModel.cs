using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using PartnersModule.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace PartnersModule.ViewModels
{
    public class PartnerDetailsViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand ShowDialogCommand { get; }

        private readonly IDialogService _dialogService;
        public PartnerDetailsViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            ShowDialogCommand = new DelegateCommand(ShowDialog);
        }


        private PartnersModel _selectedPartner;
        public PartnersModel SelectedPartner
        {
            get { return _selectedPartner; }
            set 
            {
                SetProperty(ref _selectedPartner, value);
            }
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


        private void ShowDialog()
        {
            var partner = new DialogParameters();
            partner.Add("partner", SelectedPartner);
            _dialogService.ShowDialog(nameof(PartnerEdit), partner, result =>
            {
                if(result.Result == ButtonResult.OK)
                {
                    PartnersModel partner = result.Parameters.GetValue<PartnersModel>("partner");
                    //TODO: update partner data
                }
            });
        }
    }
}
