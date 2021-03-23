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
            
            PartnerSelectedCommand = new DelegateCommand<PartnersModel>(PartnerSelected);
            NewPartnerCommand = new DelegateCommand(AddPartner);
            EditPartnerCommand = new DelegateCommand(EditPartner);

            LoadPartners();
        }
        public DelegateCommand<PartnersModel> PartnerSelectedCommand { get; private set; }
        public DelegateCommand NewPartnerCommand { get; private set; }
        public DelegateCommand EditPartnerCommand { get; private set; }

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

        private PartnersModel _partner = new();
        private void PartnerSelected(PartnersModel partner)
        {
            _partner = partner;
        }

        private async void LoadPartners()
        {
            var partnersList = await _partnersEndpoint.GetAll();
            Partners = new ObservableCollection<PartnersModel>(partnersList);
        }

        private void AddPartner()
        {
            _partner = new PartnersModel();
            SavePartnerToDatabase();
        }

        private void EditPartner()
        {
            if (_partner.Id != 0)
            {
                SavePartnerToDatabase(); 
            }
        }

        private void SavePartnerToDatabase()
        {
            var parameters = new DialogParameters();
            parameters.Add("partner", _partner);
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
