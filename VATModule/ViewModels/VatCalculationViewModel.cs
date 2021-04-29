using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Prism.Commands;
using System;

namespace VATModule.ViewModels
{
    public class VatCalculationViewModel : ViewModelBase
    {
        private readonly IVatEndpoint _vatEndpoint;


        public VatCalculationViewModel(IVatEndpoint vatEndpoint)
        {
            _vatEndpoint = vatEndpoint;

            CalculateVatCommand = new DelegateCommand(LoadCalculation);
        }

        public DelegateCommand CalculateVatCommand { get; private set; }

        private VatModel _vat = new();
        public VatModel Vat
        {
            get { return _vat; }
            set { SetProperty(ref _vat, value); }
        }

        private decimal _vatAmmount;
        public decimal VatTotal
        {
            get { return _vatAmmount; }
            set { SetProperty(ref _vatAmmount, value); }
        }

        private decimal _vatInTotal;
        public decimal VatInTotal
        {
            get { return _vatInTotal; }
            set { SetProperty(ref _vatInTotal, value); }
        }

        private decimal _taxableInTotal;
        public decimal TaxableInTotal
        {
            get { return _taxableInTotal; }
            set { SetProperty(ref _taxableInTotal, value); }
        }

        private decimal _vatOutTotal;
        public decimal VatOutTotal
        {
            get { return _vatOutTotal; }
            set { SetProperty(ref _vatOutTotal, value); }
        }

        private decimal _taxableOutTotal;
        public decimal TaxableOutTotal
        {
            get { return _taxableOutTotal; }
            set { SetProperty(ref _taxableOutTotal, value); }
        }

        private async void LoadCalculation()
        {
            if (Vat.DateFrom != null && Vat.DateTo != null)
            {
                Vat = await _vatEndpoint.GetVatForPeriod(Vat);
                CalculateTotal();
            }

        }

        private void CalculateTotal()
        {
            VatOutTotal = Vat.Pdv5 + Vat.Pdv10 + Vat.Pdv13 + Vat.Pdv25;
            TaxableInTotal = Vat.UraOsnovica5 + Vat.UraOsnovica10 + Vat.UraOsnovica13 + Vat.UraOsnovica25;
            VatInTotal = Vat.PretporezT5 + Vat.PretporezT10 + Vat.PretporezT13 + Vat.PretporezT25;
            TaxableOutTotal = Vat.IraOsnovica5 + Vat.IraOsnovica10 + Vat.IraOsnovica13 + Vat.IraOsnovica25;

            VatTotal = (VatOutTotal) - (VatInTotal);
        }

        //TODO: Extract method to separate file (local service)
        private void SerializeToXml(DateTime dateFrom, DateTime dateTo, string autorIme, string autorPrezime, decimal total12)
        {
            //TODO: Create model for reqired data items in method arguments(local service model)
            sObrazacPDV form = new sObrazacPDV
            {
                Metapodaci = new sPDVmetapodaci
                {
                    Datum = new sDatumTemeljni() { Value = DateTime.Today },
                    Naslov = new sNaslovTemeljni() { Value = "Prijava poreza na dodanu vrijednost" },
                    Autor = new sAutorTemeljni() { Value = autorIme + " " + autorPrezime },
                    Format = new sFormatTemeljni() { Value = tFormat.textxml },
                    Jezik = new sJezikTemeljni() { Value = tJezik.hrHR },
                    Identifikator = new sIdentifikatorTemeljni() { Value = Guid.NewGuid().ToString() },
                    Uskladjenost = new sUskladjenost() { Value = "ObrazacPDV-v9-0" },
                    Tip = new sTipTemeljni() { Value = tTip.Elektroničkiobrazac },
                    Adresant = new sAdresantTemeljni() { Value = "Ministarstvo Financija, Porezna uprava, Zagreb" }
                },
                Zaglavlje = new sZaglavlje
                {
                    Ispostava = "3398",
                    ObracunSastavio = new sObracunSastavio { Ime = autorIme, Prezime = autorPrezime },
                    Razdoblje = new sRazdoblje { DatumOd = dateFrom, DatumDo = dateTo},
                },
                Tijelo = new sTijelo
                {
                    //TODO: calculated sums for monthly report
                    Podatak000 = total12
                }
            };

            //TODO: Generate XML file
        }
    }
}
