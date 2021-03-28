using AccountingUI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class CityModel : ValidationBindableBase
    {
        public int Id { get; set; }
        private string _mjesto;

        [Required]
        public string Mjesto
        {
            get { return _mjesto; }
            set { SetProperty(ref _mjesto, value); }
        }

        private string _zupanija;
        [Required]
        public string Zupanija
        {
            get { return _zupanija; }
            set { SetProperty(ref _zupanija, value); }
        }

        public string Drzava { get; set; }
        public string Posta { get; set; }

        private decimal _prirez;
        [Required]
        public decimal Prirez
        {
            get { return _prirez; }
            set { SetProperty(ref _prirez, value); }
        }

        public string Sifra { get; set; }

        public void Reset()
        {
            Id = 0;
            Mjesto = "";
            Zupanija = "";
            Posta = "";
            Drzava = "";
            Sifra = "";
            Prirez = 0.00m;
        }
    }
}
