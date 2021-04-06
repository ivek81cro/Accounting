using System;

namespace Accounting.DataManager.Models
{
    public class PayrollAccountingModel
    {
        public int Id { get; set; }

        public string UniqueIdentifier { get; set; }

        public string Opis { get; set; }

        public DateTime DatumOd { get; set; }

        public DateTime DatumDo { get; set; }

        public int SatiRada { get; set; }

        public int SatiPraznika { get; set; }

        public DateTime DatumObracuna { get; set; }
    }
}
