using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using BookUraModule.ModelsLocal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookUraModule.ServiceLocal
{
    public class DataForXml : IDataForXml
    {
        private readonly ICompanyEndpoint _companyEndpoint;

        private sObrazacURA _ura = new();
        private List<BookUraRestModel> _uraList = new();
        public DataForXml(ICompanyEndpoint companyEndpoint)
        {
            _companyEndpoint = companyEndpoint;
        }

        public async Task<sObrazacURA> GenerateXml(List<BookUraRestModel> uraList, string autor, DateTime[] period)
        {
            _uraList = uraList;
            CompanyModel company = await _companyEndpoint.Get();
            _ura = new sObrazacURA
            {
                Metapodaci = new sURAmetapodaci
                {
                    Naslov = new sNaslovTemeljni()
                    {
                        Value = "Knjiga primljenih (ulaznih) računa"
                    },
                    Autor = new sAutorTemeljni { Value = autor },
                    Datum = new sDatumTemeljni { Value = DateTime.Now },
                    Format = new sFormatTemeljni()
                    {
                        Value = tFormat.textxml
                    },
                    Jezik = new sJezikTemeljni()
                    {
                        Value = tJezik.hrHR
                    },
                    Identifikator = new sIdentifikatorTemeljni()
                    {
                        Value = Guid.NewGuid().ToString()
                    },
                    Uskladjenost = new sUskladjenost()
                    {
                        Value = "ObrazacURA-v1-0"
                    },
                    Tip = new sTipTemeljni()
                    {
                        Value = tTip.Elektroničkiobrazac
                    },
                    Adresant = new sAdresantTemeljni()
                    {
                        Value = "Ministarstvo Financija, Porezna uprava, Zagreb"
                    }
                },
                Zaglavlje = new sZaglavlje
                {
                    Razdoblje = new sRazdoblje
                    {
                        DatumOd = period[0],
                        DatumDo = period[1]
                    },
                    Obveznik = new sPorezniObveznik
                    {
                        ItemElementName = ItemChoiceType.OIB,
                        Item = company.Oib,
                        ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.Naziv },
                        Items = new string[] { company.Naziv },
                        Adresa = new sAdresa()
                        {
                            Mjesto = company.Mjesto,
                            Ulica = company.Ulica,
                            Broj = company.Broj
                        },
                        PodrucjeDjelatnosti = "G",
                        SifraDjelatnosti = company.SifraDjelatnosti.Replace(".", "")
                    },
                    ObracunSastavio = new sIspunjavatelj()
                    {
                        Ime = autor.Split(' ')[0],
                        Prezime = autor.Split(' ')[1]
                    }
                },
                Tijelo = new sTijelo()
                {
                    Racuni = GeneratBody(),
                    Ukupno = new sRacuniUkupno
                    {
                        U19 = uraList.Sum(x => x.NeMozeSeOdbiti), 
                        U8 = uraList.Sum(x => x.PoreznaOsnovica5),
                        U9 = uraList.Sum(x => x.PoreznaOsnovica13),
                        U10 = uraList.Sum(x => x.PoreznaOsnovica25),
                        U11 = uraList.Sum(x => x.IznosSPorezom),
                        U12 = uraList.Sum(x => x.UkupniPretporez),
                        U13 = uraList.Sum(x => x.PretporezT5),
                        U14 = 0,
                        U15 = uraList.Sum(x => x.PretporezT13),
                        U16 = 0,
                        U17 = uraList.Sum(x => x.PretporezT25),
                        U18 = 0,
                    }
                }
            };

            return _ura;
        }

        private sRacun[] GeneratBody()
        {
            List<sRacun> racuni = new();

            foreach (var item in _uraList)
            {
                racuni.Add(
                    new sRacun
                    {
                        R1 = item.RedniBroj.ToString(),
                        R2 = item.BrojRacuna,
                        R3 = item.Datum,
                        R4 = item.NazivDobavljaca,
                        R5 = item.SjedisteDobavljaca,
                        R6 = 1,
                        R7 = item.OIB,
                        R19 = item.NeMozeSeOdbiti,
                        R8 = item.PoreznaOsnovica5,
                        R9 = item.PoreznaOsnovica13,
                        R10 = item.PoreznaOsnovica25,
                        R11 = item.IznosBezPoreza + item.UkupniPretporez,
                        R12 = item.UkupniPretporez,
                        R13 = item.PretporezT5,
                        R14 = 0,
                        R15 = item.PretporezT13,
                        R16 = 0,
                        R17 = item.PretporezT25,
                        R18 = 0
                    });
            }

            return racuni.ToArray();
        }
    }
}
