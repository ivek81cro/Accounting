using AccountingUI.Core.Models;
using Microsoft.Extensions.Configuration;

namespace PayrollModule.ServiceLocal
{
    public class PayrollCalculation : IPayrollCalculation
    {
        private readonly IConfiguration _config;

        public PayrollCalculation(IConfiguration config)
        {
            _config = config;
        }

        public void Calculate(PayrollModel p, CityModel grad, decimal osobniOdbitak)
        {
            decimal iznos = p.Bruto;
            iznos = iznos < 1300 ? iznos-((1300 - iznos) * 0.5m) : iznos;
            if (p.SamoPrviStupMio)
            {
                iznos -= p.Mio1 = iznos * (_config.GetValue<decimal>("Mio1") + _config.GetValue<decimal>("Mio2"));
                p.Mio2 = 0;
            }
            else
            {
                p.Mio1 = iznos * _config.GetValue<decimal>("Mio1");
                p.Mio2 = p.Bruto * _config.GetValue<decimal>("Mio2");
                iznos = p.Bruto - (p.Mio1 + p.Mio2);
            }
            p.Dohodak = iznos;
            decimal odbitakLoc = _config.GetValue<decimal>("Odbitak");
            decimal koefOdbitkaLoc = _config.GetValue<decimal>("KoefOdbitka");
            decimal odbitakZaposlenika = odbitakLoc + (osobniOdbitak * odbitakLoc * koefOdbitkaLoc);
            iznos -= p.Odbitak = odbitakZaposlenika;
            if (iznos < 0)
            {
                iznos = 0;
                p.Odbitak = p.Dohodak;
            }
            p.PoreznaOsnovica = iznos;

            if (p.PoreznaOsnovica > 4200)
            {
                iznos -= p.PoreznaStopa1 = 4200 * grad.Porez1;
                iznos -= p.PoreznaStopa2 = (p.PoreznaOsnovica - 4200) * grad.Porez2;
                p.UkupnoPorez = p.PoreznaStopa1 + p.PoreznaStopa2;
            }
            else
            {
                p.PoreznaStopa1 = (p.PoreznaOsnovica * grad.Porez1/100m);
                p.PoreznaStopa2 = 0;

                iznos -= p.PoreznaStopa1 - p.PoreznaStopa2;
                p.UkupnoPorez = p.PoreznaStopa1 + p.PoreznaStopa2;
            }
            p.UkupnoPorezPrirez = p.UkupnoPorez;
            p.Neto = iznos + p.Odbitak;

            p.DoprinosZdravstvo = p.Bruto * _config.GetValue<decimal>("DoprinosZdravstvo");
        }
    }
}
