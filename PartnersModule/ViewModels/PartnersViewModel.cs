using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using PartnersModule.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PartnersModule.ViewModels
{
    public class PartnersViewModel : BindableBase, INavigationAware
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
        }
        public DelegateCommand<PartnersModel> PartnerSelectedCommand { get; private set; }
        public DelegateCommand NewPartnerCommand { get; private set; }
        public DelegateCommand EditPartnerCommand { get; private set; }

        private string _title = "Partneri";
        public string Title
        {
            get { return _title = "Partneri"; }
            set { SetProperty(ref _title, value); }
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

        private PartnersModel _partner = new();
        private void PartnerSelected(PartnersModel partner)
        {
            _partner = partner;
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
