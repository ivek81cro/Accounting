using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using PartnersModule.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;

namespace PartnersModule.ViewModels
{
    public class PartnersViewModel : ViewModelBase
    {

        private IPartnersEndpoint _partnersEndpoint;
        private IRegionManager _regionManager;
        private IDialogService _showDialog;

        public PartnersViewModel(IPartnersEndpoint partnersEndpoint, IRegionManager regionManager, 
            IDialogService showDialog)
        {
            _partnersEndpoint = partnersEndpoint;
            _regionManager = regionManager;
            _showDialog = showDialog;
            
            NewPartnerCommand = new DelegateCommand(SavePartnerToDatabase);
            DeletePartnerCommand = new DelegateCommand(DeletePartner, CanDelete);
        }

        public DelegateCommand NewPartnerCommand { get; private set; }
        public DelegateCommand DeletePartnerCommand { get; private set; }

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

        private PartnersModel _selectedPartner;
        public PartnersModel SelectedPartner
        {
            get { return _selectedPartner; }
            set 
            { 
                SetProperty(ref _selectedPartner, value);
                DeletePartnerCommand.RaiseCanExecuteChanged();
            }
        }

        public async void LoadPartners()
        {
            var partnersList = await _partnersEndpoint.GetAll();
            Partners = new ObservableCollection<PartnersModel>(partnersList);
        }

        private void SavePartnerToDatabase()
        {
            var parameters = new DialogParameters();
            parameters.Add("partner", SelectedPartner);
            _showDialog.ShowDialog(nameof(PartnerEdit), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    PartnersModel partner = result.Parameters.GetValue<PartnersModel>("partner");
                    _partnersEndpoint.PostPartner(partner);
                }
            });
        }

        private bool CanDelete()
        {
            return SelectedPartner != null;
        }

        private void DeletePartner()
        {
            _partnersEndpoint.DeletePartner(SelectedPartner.Id);
            _partners.Remove(SelectedPartner);
        }
    }
}
