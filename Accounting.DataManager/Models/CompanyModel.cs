namespace Accounting.DataManager.Models
{
    public class CompanyModel
    {	
		public int Id { get; set; }
		public string Oib { get; set; }
		public string Naziv { get; set; }
		public string Ulica { get; set; }
		public string Broj { get; set; }
		public string Posta { get; set; }
		public string Mjesto { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public string Iban { get; set; }
		public string VrstaDjelatnosti { get; set; }
		public string SifraDjelatnosti { get; set; }
		public string NazivDjelatnosti { get; set; }
		public string Mbo { get; set; }
	}
}
