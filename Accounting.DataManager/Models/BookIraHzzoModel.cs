using System;

namespace Accounting.DataManager.Models
{
    public class BookIraHzzoModel
    {
        public int Id { get; set; }

        public DateTime DatumPlacanja { get; set; }

        public string Dokument { get; set; }

        public string OriginalniBroj { get; set; }

        public DateTime DatumDokumenta { get; set; }

        public string Program { get; set; }

        public string Opis { get; set; }

        public decimal IznosRacuna { get; set; }

        public decimal PlaceniIznos { get; set; }

        public bool Povezan { get; set; }
    }
}
