using System;

namespace Accounting.DataManager.Models
{
    public class PayrollArchiveHeaderModel
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public string Opis { get; set; }

        public DateTime DatumOd { get; set; }

        public DateTime DatumDo { get; set; }

        public int SatiRada { get; set; }

        public int SatiPraznika { get; set; }

        public DateTime DatumObracuna { get; set; }

        public bool Knjizen { get; set; }
    }
}
