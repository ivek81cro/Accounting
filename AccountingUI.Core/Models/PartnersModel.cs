using AccountingUI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
	public class PartnersModel : ValidationBindableBase
	{
		public int Id { get; set; }
        private string _oib;
        [Required]
		public string Oib
        {
            get { return _oib; }
            set 
			{
				SetProperty(ref _oib, value);
			}
        }
        private string _naziv;
		[Required]
        public string Naziv
        {
            get { return _naziv; }
            set 
            { 
                SetProperty(ref _naziv, value.ToUpper());
            }
        }
        public string Ulica { get; set; }
		public string Broj { get; set; }
		public string Posta { get; set; }
		public string Mjesto { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
        public string Email { get; set; }
        public string Iban { get; set; }
		public string Mbo { get; set; }
		public string KontoK { get; set; }
		public string KontoD { get; set; }

        public void Reset()
        {
            Id = 0;
            Oib = "";
            Naziv = "";
            Posta = "";
            Mjesto = "";
            Broj = "";
            Ulica = "";
            Telefon = "";
            Fax = "";
            Email = "";
            Mbo = "";
            KontoD = "";
            KontoK = "";
            Iban = "";
        }
    }
}
