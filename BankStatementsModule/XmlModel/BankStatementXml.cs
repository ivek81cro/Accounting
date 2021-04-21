using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankkStatementsModule.XmlModel
{
    [XmlRoot("Izvodi")]
    class BankStatementXml
    { 
        [XmlElement("Banka")]
        public sBanka Banka { get; set; }
        [XmlElement("OIB")]
        public string OIB { get; set; }
        [XmlElement("MaticniBroj")]
        public string MaticniBroj { get; set; }
        [XmlElement("Podbroj")]
        public string Podbroj { get; set; }
        [XmlElement("Izvod")]
        public sIzvod Izvod { get; set; }
    }

    [XmlType("PNBPrimatelja")]
    public class sPNBPrimatelja
    {
        [XmlElement("Model")]
        public string Model { get; set; }
        [XmlElement("Broj")]
        public string Broj { get; set; }
    }

    [XmlType("PNBPlatitelja")]
    public class sPNBPlatitelja
    {
        [XmlElement("Model")]
        public string Model { get; set; }
        [XmlElement("Broj")]
        public string Broj { get; set; }
    }

    [XmlType("IznosPrometa")]
    public class sIznosPrometa
    {
        [XmlElement("Oznaka")]
        public string Oznaka { get; set; }
        [XmlElement("Iznos")]
        public string Iznos { get; set; }
    }

    [XmlType("Promet")]
    public class sPromet
    {
        [XmlElement("Referenca")]
        public string Referenca { get; set; }
        [XmlElement("Referenca2")]
        public string Referenca2 { get; set; }
        [XmlElement("Referenca3")]
        public string Referenca3 { get; set; }
        [XmlElement("DatumKnjizenja")]
        public string DatumKnjizenja { get; set; }
        [XmlElement("DatumIzvrsenja")]
        public string DatumIzvrsenja { get; set; }
        [XmlElement("Naziv")]
        public string Naziv { get; set; }
        [XmlElement("Adresa")]
        public string Adresa { get; set; }
        [XmlElement("Sjediste")]
        public string Sjediste { get; set; }
        [XmlElement("PartijaPrometa")]
        public string PartijaPrometa { get; set; }
        [XmlElement("ValutaPokrica")]
        public string ValutaPokrica { get; set; }
        [XmlElement("IznosValutePokrica")]
        public string IznosValutePokrica { get; set; }
        [XmlElement("Tecaj")]
        public string Tecaj { get; set; }
        [XmlElement("SifraNamjene")]
        public string Sifranamjene { get; set; }
        [XmlElement("OpisPlacanja")]
        public string OpisPlacanja { get; set; }
        [XmlElement("PNBPrimatelja")]
        public sPNBPrimatelja PNBPrimatelja { get; set; }
        [XmlElement("PNBPlatitelja")]
        public sPNBPlatitelja PNBPlatitelja { get; set; }
        [XmlElement("IznosPrometa")]
        public sIznosPrometa IznosPrometa { get; set; }
    }

    [XmlType("Prometi")]
    public class sPrometi
    {
        [XmlElement("Promet")]
        public List<sPromet> Promet { get; set; } = new List<sPromet>();
    }

    [XmlType("DStrana")]
    public class sDStrana
    {
        [XmlElement("UkupnaSuma")]
        public string UkupnaSuma { get; set; }
        [XmlElement("UkupanBroj")]
        public string UkupanBroj { get; set; }
    }

    [XmlType("PStrana")]
    public class sPStrana
    {
        [XmlElement("UkupnaSuma")]
        public string UkupnaSuma { get; set; }
        [XmlElement("UkupanBroj")]
        public string UkupanBroj { get; set; }
    }

    [XmlType("Sekcija")]
    public class sSekcija
    {
        [XmlElement("Partija")]
        public string Partija { get; set; }
        [XmlElement("TipPartije")]
        public string TipPartije { get; set; }
        [XmlElement("OznakaValute")]
        public string OznakaValute { get; set; }
        [XmlElement("PrethodnoStanje")]
        public string PrethodnoStanje { get; set; }
        [XmlElement("PrethodniRBRIzvoda")]
        public string PrethodniRBRIzvoda { get; set; }
        [XmlElement("PrethodniIzvodDatum")]
        public string PrethodniIzvodDatum { get; set; }
        [XmlElement("Prometi")]
        public sPrometi Prometi { get; set; }
        [XmlElement("DStrana")]
        public sDStrana DStrana { get; set; }
        [XmlElement("PStrana")]
        public sPStrana PStrana { get; set; }
        [XmlElement("NovoStanje")]
        public string NovoStanje { get; set; }
        [XmlElement("Prekoracenje")]
        public string Prekoracenje { get; set; }
    }

    [XmlType("Sekcije")]
    public class sSekcije
    {
        [XmlElement("Sekcija")]
        public sSekcija Sekcija { get; set; }
    }

    [XmlType("IBAN")]
    public class sIBAN
    {
        [XmlElement("IBANBroj")]
        public string IBANBroj { get; set; }
    }

    [XmlType("Izvod")]
    public class sIzvod
    {
        [XmlElement("Naziv")]
        public string Naziv { get; set; }
        [XmlElement("Adresa")]
        public string Adresa { get; set; }
        [XmlElement("BrojPoste")]
        public string BrojPoste { get; set; }
        [XmlElement("NazivPoste")]
        public string NazivPoste { get; set; }
        [XmlElement("OJBanke")]
        public string OJBanke { get; set; }
        [XmlElement("RedniBroj")]
        public string RedniBroj { get; set; }
        [XmlElement("DatumIzvoda")]
        public string DatumIzvoda { get; set; }
        [XmlElement("IBAN")]
        public sIBAN IBAN { get; set; }
        [XmlElement("Sekcije")]
        public sSekcije Sekcije { get; set; }
    }

    [XmlType("Banka")]
    public class sBanka
    {
        [XmlElement("Naziv")]
        public string Naziv { get; set; }
        [XmlElement("OIB")]
        public string OIB { get; set; }
        [XmlElement("Racun")]
        public string Racun { get; set; }
        [XmlElement("IBAN")]
        public sIBAN IBAN { get; set; }
        [XmlElement("BIC")]
        public string BIC { get; set; }
    }
}
