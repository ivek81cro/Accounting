using System;

namespace Accounting.DataManager.Models
{
    public class LocoOrderModel
    {
        public int Id { get; set; }

        public int ZaposlenikId { get; set; }

        public DateTime Datum { get; set; }

        public string MarkaVozila { get; set; }

        public string Registracija { get; set; }

        public int PocetnoStanje { get; set; }

        public int ZavrsnoStanje { get; set; }

        public string Relacija { get; set; }

        public int PrijedeniKilometri { get; set; }

        public string Opis { get; set; }

        public int ObracunId { get; set; }
    }
}
