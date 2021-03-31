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

        public void Calculate(PayrollModel p, decimal prirez, decimal osobniOdbitak)
        {
            decimal iznos = p.Bruto;
            if (p.SamoPrviStupMio)
            {
                iznos -= p.Mio1 = iznos * (_config.GetValue<decimal>("Mio1") + _config.GetValue<decimal>("Mio2"));
                p.Mio2 = 0;
            }
            else
            {
                p.Mio1 = iznos * _config.GetValue<decimal>("Mio1");
                p.Mio2 = iznos * _config.GetValue<decimal>("Mio2");
                iznos -= p.Mio1 + p.Mio2;
            }
            p.Dohodak = iznos;
            iznos -= p.Odbitak = (_config.GetValue<decimal>("Odbitak") * _config.GetValue<decimal>("KoefOdbitka")) + 
                osobniOdbitak * _config.GetValue<decimal>("Odbitak");
            if (iznos < 0)
            {
                iznos = 0;
                p.Odbitak = p.Dohodak;
            }
            p.PoreznaOsnovica = iznos;

            if (p.PoreznaOsnovica > 30000)
            {
                iznos -= p.PoreznaStopa1 = 30000.0m * _config.GetValue<decimal>("PorezDohodak1");
                iznos -= p.PoreznaStopa1 = (p.PoreznaOsnovica - 30000) * _config.GetValue<decimal>("PorezDohodak2");
            }
            else
            {
                iznos -= p.PoreznaStopa1 = p.PoreznaOsnovica * _config.GetValue<decimal>("PorezDohodak1");
                p.PoreznaStopa2 = 0;
            }
            iznos -= p.Prirez = (p.UkupnoPorez = p.PoreznaStopa1 + p.PoreznaStopa2) * prirez / 100;
            p.UkupnoPorezPrirez = p.UkupnoPorez + p.Prirez;
            p.Neto = iznos + p.Odbitak;

            p.DoprinosZdravstvo = p.Bruto * _config.GetValue<decimal>("DoprinosZdravstvo");
        }
    }
}
