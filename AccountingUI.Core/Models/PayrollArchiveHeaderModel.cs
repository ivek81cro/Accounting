using AccountingUI.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class PayrollArchiveHeaderModel : ValidationBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _uniqueId;
        public string UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        private string _opis;
        [Required]
        public string Opis
        {
            get { return _opis; }
            set { SetProperty(ref _opis, value); }
        }

        private DateTime? _datumOd;
        public DateTime? DatumOd
        {
            get { return _datumOd; }
            set { SetProperty(ref _datumOd, value); }
        }

        private DateTime? _datumDo;
        public DateTime? DatumDo
        {
            get { return _datumDo; }
            set { SetProperty(ref _datumDo, value); }
        }

        private int _satiRada;
        public int SatiRada
        {
            get { return _satiRada; }
            set { SetProperty(ref _satiRada, value); }
        }

        private int _satiPraznika;
        public int SatiPraznika
        {
            get { return _satiPraznika; }
            set { SetProperty(ref _satiPraznika, value); }
        }

        private DateTime? _datumObracuna;
        public DateTime? DatumObracuna
        {
            get { return _datumObracuna; }
            set { SetProperty(ref _datumObracuna, value); }
        }

        private bool _knjizen;
        public bool Knjizen
        {
            get { return _knjizen; }
            set { SetProperty(ref _knjizen, value); }
        }

        public void CreateUniqueIdentifier()
        {
            UniqueId = DatumOd.ToString().Replace(".", "").Replace(":", "").Replace(" ","").Trim() + "-" +
                DatumDo.ToString().Replace(".", "").Replace(":", "").Replace(" ", "").Trim() + "-" +
                DatumObracuna.ToString().Replace(".", "").Replace(":", "").Replace(" ", "").Trim();
        }
    }
}
