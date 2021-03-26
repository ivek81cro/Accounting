using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using Prism.Regions;

namespace CompanyModule.ViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        private CompanyEndPoint _companyEndpoint;
        private IRegionManager _regionManager;

        public CompanyViewModel(CompanyEndPoint companyEndpoint, IRegionManager regionManager)
        {
            _companyEndpoint = companyEndpoint;
            _regionManager = regionManager;

            SaveCompanyCommand = new DelegateCommand(SaveCompany);
        }

        public DelegateCommand SaveCompanyCommand { get; private set; }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _oib;
        public string Oib
        {
            get { return _oib; }
            set { SetProperty(ref _oib, value); }
        }

        private string _naziv;
        public string Naziv
        {
            get { return _naziv; }
            set { SetProperty(ref _naziv, value); }
        }

        private string _ulica;
        public string Ulica
        {
            get { return _ulica; }
            set { SetProperty(ref _ulica, value); }
        }

        private string _broj;
        public string Broj
        {
            get { return _broj; }
            set { SetProperty(ref _broj, value); }
        }

        private string _posta;
        public string Posta
        {
            get { return _posta; }
            set { SetProperty(ref _posta, value); }
        }

        private string _mjesto;
        public string Mjesto
        {
            get { return _mjesto; }
            set { SetProperty(ref _mjesto, value); }
        }

        private string _telefon;
        public string Telefon
        {
            get { return _telefon; }
            set { SetProperty(ref _telefon, value); }
        }

        private string _fax;
        public string Fax
        {
            get { return _fax; }
            set { SetProperty(ref _fax, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _iban;
        public string Iban
        {
            get { return _iban; }
            set { SetProperty(ref _iban, value); }
        }

        private string _vrstaDjelatnosti;
        public string VrstaDjelatnosti
        {
            get { return _vrstaDjelatnosti; }
            set { SetProperty(ref _vrstaDjelatnosti, value); }
        }

        private string _sifraDjelatnosti;
        public string SifraDjelatnosti
        {
            get { return _sifraDjelatnosti; }
            set { SetProperty(ref _sifraDjelatnosti, value); }
        }

        private string _nazivDjelatnosti;
        public string NazivDjelatnosti
        {
            get { return _nazivDjelatnosti; }
            set { SetProperty(ref _nazivDjelatnosti, value); }
        }

        private string _mbo;
        public string Mbo
        {
            get { return _mbo; }
            set { SetProperty(ref _mbo, value); }
        }

        private void SaveCompany()
        {
            if (Id != 0)
            {
                SaveCompanyToDatabase();
            }
        }

        private async void SaveCompanyToDatabase()
        {
            CompanyModel c = new CompanyModel
            {
                Oib = this.Oib,
                Broj = this.Broj,
                Email = this.Email,
                Fax = this.Fax,
                Iban=this.Iban,
                Mbo=this.Mbo,
                Mjesto=this.Mjesto,
                Id=this.Id,
                Naziv=this.Naziv,
                NazivDjelatnosti=this.NazivDjelatnosti,
                Posta=this.Posta,
                SifraDjelatnosti=this.SifraDjelatnosti,
                Telefon=this.Telefon,
                Ulica=this.Ulica,
                VrstaDjelatnosti=this.VrstaDjelatnosti
            };
            await _companyEndpoint.PostCompany(c);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            CompanyModel c = await _companyEndpoint.Get();

            Id = c.Id;
            Naziv = c.Naziv;
            Oib = c.Oib;
            Broj = c.Broj;
            Email = c.Email;
            Fax = c.Fax;
            Iban = c.Iban;
            Mbo = c.Mbo;
            Mjesto = c.Mjesto;
            NazivDjelatnosti = c.NazivDjelatnosti;
            Posta = c.Posta;
            SifraDjelatnosti = c.SifraDjelatnosti;
            Telefon = c.Telefon;
            Ulica = c.Ulica;
            VrstaDjelatnosti = c.VrstaDjelatnosti;
        }
    }
}
