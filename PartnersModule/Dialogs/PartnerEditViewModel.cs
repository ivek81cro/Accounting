using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PartnersModule.Dialogs
{
    public class PartnerEditViewModel : BindableBase, IDialogAware
    {
        private readonly IPartnersEndpoint _partnersEndpoint;


        public PartnerEditViewModel(IPartnersEndpoint partnersEndpoint)
        {
            SavePartnerCommand = new DelegateCommand(CloseDialog);
            _partnersEndpoint = partnersEndpoint;
        }

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand SavePartnerCommand { get; }
        public string Title => "Izmjena Podataka Partnera";

        private PartnersModel _partner;
        public PartnersModel Partner
        {
            get { return _partner; }
            set { SetProperty(ref _partner, value); }
        }

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

        private string _mbo;
        public string Mbo
        {
            get { return _mbo; }
            set { SetProperty(ref _mbo, value); }
        }

        private string _kontoK;
        public string KontoK
        {
            get { return _kontoK; }
            set { SetProperty(ref _kontoK, value); }
        }

        private string _kontoD;
        public string KontoD
        {
            get { return _kontoD; }
            set { SetProperty(ref _kontoD, value); }
        }

        private void CloseDialog()
        {
            Partner = new PartnersModel
            {
                Id = _id,
                Oib = _oib,
                Naziv = _naziv,
                Ulica = _ulica,
                Broj = _broj,
                Posta = _posta,
                Mjesto = _mjesto,
                Telefon = _telefon,
                Fax = _fax,
                Email = _email,
                Iban = _iban,
                Mbo = _mbo,
                KontoK = _kontoK,
                KontoD = _kontoD
            };

            _partnersEndpoint.PostPartner(Partner);
            var result = ButtonResult.OK;
            var p = new DialogParameters();
            p.Add("partner", Partner);

            RequestClose?.Invoke(new DialogResult(result, p));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var partnerId = parameters.GetValue<PartnersModel>("partner").Id;
            if (partnerId != 0)
            {
                GetPartnerFromDatabase(partnerId);
            }
        }

        private async void GetPartnerFromDatabase(int partnerId)
        {
            Partner = await _partnersEndpoint.GetById(partnerId);
            Id = Partner.Id;
            Oib = Partner.Oib;
            Naziv = Partner.Naziv;
            Ulica = Partner.Ulica;
            Broj = Partner.Broj;
            Posta = Partner.Posta;
            Mjesto = Partner.Mjesto;
            Telefon = Partner.Telefon;
            Fax = Partner.Fax;
            Email = Partner.Email;
            Iban = Partner.Iban;
            Mbo = Partner.Mbo;
            KontoK = Partner.KontoK;
            KontoD = Partner.KontoD;
        }
    }
}
