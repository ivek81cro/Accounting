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
    }
}
