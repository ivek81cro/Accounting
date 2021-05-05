using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace PartnersModule.Dialogs
{
    public class PartnerEditViewModel : BindableBase, IDialogAware
    {
        private readonly IPartnersEndpoint _partnersEndpoint;
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;


        public PartnerEditViewModel(IPartnersEndpoint partnersEndpoint,
                                    IBookAccountsEndpoint bookAccountsEndpoint)
        {
            _partnersEndpoint = partnersEndpoint;
            _bookAccountsEndpoint = bookAccountsEndpoint;

            SavePartnerCommand = new DelegateCommand(SavePartner, CanSavePartner);
        }

        public DelegateCommand SavePartnerCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        public string Title => "Izmjena Podataka Partnera";

        private PartnersModel _partner;
        public PartnersModel Partner
        {
            get { return _partner; }
            set 
            { 
                SetProperty(ref _partner, value);
            }
        }

        private bool _isBuyer;
        public bool IsBuyer
        {
            get { return _isBuyer; }
            set 
            { 
                SetProperty(ref _isBuyer, value);
                SavePartnerCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isSupplier;
        public bool IsSupplier
        {
            get { return _isSupplier; }
            set 
            {
                SetProperty(ref _isSupplier, value);
                SavePartnerCommand.RaiseCanExecuteChanged();
            }
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
            Partner = parameters.GetValue<PartnersModel>("partner");
            if (Partner != null)
            {
                GetPartnerFromDatabase(Partner.Id);
            }
            else 
            {
                Partner = new();
            }
        }

        private async void GetPartnerFromDatabase(int partnerId)
        {
            Partner = await _partnersEndpoint.GetById(partnerId);
            IsBuyer = Partner.KontoK != null && Partner.KontoK.StartsWith("12");
            IsSupplier = Partner.KontoD != null && Partner.KontoD.StartsWith("22");
        }

        private bool CanSavePartner()
        {
            return IsBuyer || IsSupplier;
        }

        private async void SavePartner()
        {
            if (Partner != null && !Partner.HasErrors)
            {
                await _partnersEndpoint.PostPartner(Partner);
                await AddAccountNumber(Partner.Oib);
                var result = ButtonResult.OK;

                RequestClose?.Invoke(new DialogResult(result));
            }
        }

        private async Task AddAccountNumber(string oib)
        {
            Partner = await _partnersEndpoint.GetByOib(oib);

            if (IsBuyer)
            {
                Partner.KontoK = "12";
            }
            if (IsSupplier)
            {
                Partner.KontoD = "22";
            }

            string sifra = Partner.Id.ToString();
            string kontoK = Partner.KontoK;
            string kontoD = Partner.KontoD;

            if (kontoK != null && kontoK.StartsWith("12"))
            {
                while (kontoK.Length + sifra.Length < 8)
                { 
                    kontoK += "0"; 
                }
                kontoK += sifra;
                await _bookAccountsEndpoint.Insert(new BookAccountModel { Konto = kontoK, Opis = Partner.Naziv });
            }

            if (kontoD != null && kontoD.StartsWith("22"))
            {
                while (kontoD.Length + sifra.Length < 8)
                {
                    kontoD += "0";
                }
                kontoD += sifra;
                await _bookAccountsEndpoint.Insert(new BookAccountModel { Konto = kontoD, Opis = Partner.Naziv });
            }

            Partner.KontoK = kontoK;
            Partner.KontoD = kontoD;

            await _partnersEndpoint.PostPartner(Partner);
        }
    }
}
