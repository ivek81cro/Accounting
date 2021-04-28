using System;

namespace Accounting.DataManager.Models
{
    public class VatModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal IraOsnovica0 { get; set; }
        public decimal IraOsnovica5 { get; set; }
        public decimal IraOsnovica10 { get; set; }
        public decimal IraOsnovica13 { get; set; }
        public decimal IraOsnovica25 { get; set; }
        public decimal Pdv5 { get; set; }
        public decimal Pdv10 { get; set; }
        public decimal Pdv13 { get; set; }
        public decimal Pdv25 { get; set; }
        public decimal UraOsnovica0 { get; set; }
        public decimal UraOsnovica5 { get; set; }
        public decimal UraOsnovica10 { get; set; }
        public decimal UraOsnovica13 { get; set; }
        public decimal UraOsnovica25 { get; set; }
        public decimal PretporezT5 { get; set; }
        public decimal PretporezT10 { get; set; }
        public decimal PretporezT13 { get; set; }
        public decimal PretporezT25 { get; set; }
        public decimal Neoporezivo { get; set; }
    }
}
