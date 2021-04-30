using AccountingUI.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class FixedAssetModel : ValidationBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _naziv;
        [Required]
        public string Naziv
        {
            get { return _naziv; }
            set { SetProperty(ref _naziv, value); }
        }

        private DateTime _datumNabave;
        public DateTime DatumNabave
        {
            get { return _datumNabave; }
            set { SetProperty(ref _datumNabave, value); }
        }

        private decimal _kolicina;
        public decimal Kolicina
        {
            get { return _kolicina; }
            set { SetProperty(ref _kolicina, value); }
        }

        private string _lokacija;
        public string Lokacija
        {
            get { return _lokacija; }
            set { SetProperty(ref _lokacija, value); }
        }

        private int _invBroj;
        public int InvBroj
        {
            get { return _invBroj; }
            set { SetProperty(ref _invBroj, value); }
        }

        private string _dobavljac;
        [Required]
        public string Dobavljac
        {
            get { return _dobavljac; }
            set { SetProperty(ref _dobavljac, value); }
        }

        private string _dokument;
        [Required]
        public string Dokument
        {
            get { return _dokument; }
            set { SetProperty(ref _dokument, value); }
        }

        private DateTime _datumUporabe;
        public DateTime DatumUporabe
        {
            get { return _datumUporabe; }
            set { SetProperty(ref _datumUporabe, value); }
        }

        private decimal _nabavnaVrijednost;
        [Required]
        public decimal NabavnaVrijednost
        {
            get { return _nabavnaVrijednost; }
            set { SetProperty(ref _nabavnaVrijednost, value); }
        }

        private string _skupina;
        public string Skupina
        {
            get { return _skupina; }
            set { SetProperty(ref _skupina, value); }
        }

        private decimal _vijekTrajanja;
        public decimal VijekTrajanja
        {
            get { return _vijekTrajanja; }
            set { SetProperty(ref _vijekTrajanja, value); }
        }

        private decimal _stopaOtpisa;
        public decimal StopaOtpisa
        {
            get { return _stopaOtpisa; }
            set { SetProperty(ref _stopaOtpisa, value); }
        }

        private string _sintetickiKonto;
        public string SintetickiKonto
        {
            get { return _sintetickiKonto; }
            set { SetProperty(ref _sintetickiKonto, value); }
        }

        private string _kontoOtpisa;
        public string KontoOtpisa
        {
            get { return _kontoOtpisa; }
            set { SetProperty(ref _kontoOtpisa, value); }
        }

        private decimal _iznosOtpisa;
        public decimal IznosOtpisa
        {
            get { return _iznosOtpisa; }
            set { SetProperty(ref _iznosOtpisa, value); }
        }

        private decimal _sadasnjaVrijednost;
        public decimal SadasnjaVrijednost
        {
            get { return _sadasnjaVrijednost; }
            set { SetProperty(ref _sadasnjaVrijednost, value); }
        }

        private DateTime _datumRashodovanja;
        public DateTime DatumRashodovanja
        {
            get { return _datumRashodovanja; }
            set { SetProperty(ref _datumRashodovanja, value); }
        }
    }
}
