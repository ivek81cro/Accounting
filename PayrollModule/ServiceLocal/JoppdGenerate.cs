using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using PayrollModule.ServiceLocal.EporeznaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollModule.ServiceLocal
{
    public class JoppdGenerate : IJoppdGenerate
    {
        private readonly ICompanyEndpoint _companyEndpoint;
        private List<sPrimateljiP> _pArr = new List<sPrimateljiP>();
        private PayrollArchiveModel _archive;

        public JoppdGenerate(ICompanyEndpoint companyEndpoint)
        {
            _companyEndpoint = companyEndpoint;
        }

        public async Task<sObrazacJOPPD> CreateJoppdEporezna(DateTime? date, string identifier, string formCreator,
            PayrollArchiveModel archive, List<JoppdEmployeeModel> joppdEmployee)
        {
            _archive = archive;

            AddRecipients(joppdEmployee);

            CompanyModel c = await _companyEndpoint.Get();
            sStranaA strA = new sStranaA()
            {
                DatumIzvjesca = (DateTime)date,
                OznakaIzvjesca = identifier,
                VrstaIzvjesca = tVrstaIzvjesca.Item1,
                IzvjesceSastavio = new sIzvjesceSastavio()
                {
                    Ime = formCreator.Split(' ')[0],
                    Prezime = formCreator.Split(' ')[1]
                },
                PodnositeljIzvjesca = new sPodnositeljIzvjesca()
                {
                    ItemsElementName = new[] { ItemsChoiceType.Naziv },
                    Items = new[] { c.Naziv },
                    Adresa = new sAdresa()
                    {
                        Ulica = c.Ulica,
                        Broj = c.Broj,
                        Mjesto = c.Mjesto
                    },
                    Email = c.Email,
                    OIB = c.Oib,
                    Oznaka = tOznakaPodnositelja.Item2
                },
                BrojOsoba = archive.Payrolls.Select(x => x.Oib).Distinct().Count().ToString(),
                BrojRedaka = _pArr.Count.ToString(),
                PredujamPoreza = new sPredujamPoreza()
                {
                    P1 = archive.Payrolls.Sum(x => x.UkupnoPorezPrirez),
                    P11 = archive.Payrolls.Sum(x => x.UkupnoPorezPrirez),
                    P12 = 0,
                    P2 = 0,
                    P3 = 0,
                    P4 = 0,
                    P5 = 0,
                    P6 = 0
                },
                Doprinosi = new sDoprinosi()
                {
                    GeneracijskaSolidarnost = new sGeneracijskaSolidarnost()
                    {
                        P1 = SumMio1Employees(_pArr),
                        P1Specified = true,
                        P2 = 0,
                        P3 = SumMio1Employer(_pArr),
                        P3Specified = true,
                        P4 = 0,
                        P5 = 0,
                        P6 = 0,
                        P7 = 0
                    },
                    KapitaliziranaStednja = new sKapitaliziranaStednja()
                    {
                        P1 = SumMio2Employees(_pArr),
                        P1Specified = true,
                        P2 = 0,
                        P3 = SumMio2Employer(_pArr),
                        P3Specified = true,
                        P4 = 0,
                        P5 = 0,
                        P6 = 0
                    },
                    ZdravstvenoOsiguranje = new sZdravstvenoOsiguranje()
                    {
                        P1 = SumHealthcareEmployee(_pArr),
                        P1Specified = true,
                        P2 = 0,
                        P3 = SumHealthcareEmployer(_pArr),
                        P3Specified = true,
                        P4 = 0,
                        P5 = 0,
                        P6 = 0,
                        P7 = 0,
                        P8 = 0,
                        P9 = 0,
                        P10 = 0,
                        P11 = 0,
                        P12 = 0
                    },
                    Zaposljavanje = new sZaposljavanje()
                    {
                        P1 = 0,
                        P2 = 0,
                        P3 = 0,
                        P4 = 0
                    }
                },
                IsplaceniNeoporeziviPrimici = _pArr.Sum(x => x.P152),
                IsplaceniNeoporeziviPrimiciSpecified = true,
                KamataMO2 = 0,
                UkupniNeoporeziviPrimici = 0,
                NaknadaZaposljavanjeInvalida = new sNaknadaZaposljavanjeInvalida()
                {
                    P1 = "0",
                    P2 = 0
                }
            };

            sJOPPDmetapodaci meta = new sJOPPDmetapodaci()
            {
                Datum = new sDatumTemeljni() { Value = (DateTime)date },
                Naslov = new sNaslovTemeljni() { Value = "Izvješće o primicima, porezu na dohodak i prirezu te doprinosima za obvezna osiguranja" },
                Autor = new sAutorTemeljni() { Value = formCreator },
                Format = new sFormatTemeljni() { Value = tFormat.textxml },
                Jezik = new sJezikTemeljni() { Value = tJezik.hrHR },
                Identifikator = new sIdentifikatorTemeljni() { Value = Guid.NewGuid().ToString() },
                Uskladjenost = new sUskladjenost() { Value = "ObrazacJOPPD-v1-1" },
                Tip = new sTipTemeljni() { Value = tTip.Elektroničkiobrazac },
                Adresant = new sAdresantTemeljni() { Value = "Ministarstvo Financija, Porezna uprava, Zagreb" }
            };
            sPrimateljiP[][] prim = { _pArr.ToArray() };

            sObrazacJOPPD sJoppd = new sObrazacJOPPD();
            sJoppd.StranaA = strA;
            sJoppd.StranaB = prim;
            sJoppd.Metapodaci = meta;

            return sJoppd;
        }

        private decimal SumHealthcareEmployee(List<sPrimateljiP> pArr)
        {
            decimal sum = 0;
            foreach (var p in pArr)
            {
                if (!p.P61.ToString().Contains("0032") && !p.P62.ToString().Contains("0101"))
                {
                    sum += p.P123;
                }
            }

            return sum;
        }

        private decimal SumHealthcareEmployer(List<sPrimateljiP> pArr)
        {
            decimal sum = 0;
            foreach (var p in pArr)
            {
                if (p.P61.ToString().Contains("0032") && p.P62.ToString().Contains("0101"))
                {
                    sum += p.P123;
                }
            }

            return sum;
        }

        private decimal SumMio2Employer(List<sPrimateljiP> pArr)
        {
            decimal sum = 0;
            foreach (var p in pArr)
            {
                if (p.P61.ToString().Contains("003") && p.P62.ToString().Contains("0101"))
                {
                    sum += p.P122;
                }
            }

            return sum;
        }

        private decimal SumMio2Employees(List<sPrimateljiP> pArr)
        {
            decimal sum = 0;
            foreach (var p in pArr)
            {
                if (!p.P61.ToString().Contains("0032") && !p.P62.ToString().Contains("0101"))
                {
                    sum += p.P122;
                }
            }

            return sum;
        }

        private decimal SumMio1Employer(List<sPrimateljiP> pArr)
        {
            decimal sum = 0;
            foreach (var p in pArr)
            {
                if (p.P61.ToString().Contains("0032") && p.P62.ToString().Contains("0101"))
                {
                    sum += p.P121;
                }
            }

            return sum;
        }

        private decimal SumMio1Employees(List<sPrimateljiP> pArr)
        {
            decimal sum = 0;
            foreach (var p in pArr)
            {
                if (!p.P61.ToString().Contains("0032") && !p.P62.ToString().Contains("0101"))
                {
                    sum += p.P121;
                }
            }

            return sum;
        }

        private void AddRecipients(List<JoppdEmployeeModel> joppdEmployee)
        {
            _pArr = new List<sPrimateljiP>();
            for (int i = 0; i < _archive.Payrolls.Count; i++)
            {
                var p = _archive.Payrolls[i];
                var e = joppdEmployee.Where(j => j.Oib == p.Oib).FirstOrDefault();
                _pArr.Add(new sPrimateljiP()
                {
                    P1 = _pArr.Count() + 1,
                    P2 = e.SifraPrebivalista,
                    P3 = e.SifraOpcineRada,
                    P4 = e.Oib,
                    P5 = p.Ime + " " + p.Prezime,
                    P61 = (tOznakaStjecatelja)Enum.Parse(typeof(tOznakaStjecatelja), "Item" + e.OznakaStjecatelja),
                    P62 = (tOznakaPrimici)Enum.Parse(typeof(tOznakaPrimici), "Item" + e.OznakaPrimitka),
                    P71 = (tOznakaMO)Enum.Parse(typeof(tOznakaMO), e.DodatniMio),
                    P72 = (tOznakaInvaliditet)Enum.Parse(typeof(tOznakaInvaliditet), "Item" + e.ObvezaInvaliditet),
                    P8 = (tOznakaMjesec)Enum.Parse(typeof(tOznakaMjesec), "Item" + e.PrviZadnjiMjesec),
                    P9 = (tOznakaRadnoVrijeme)Enum.Parse(typeof(tOznakaRadnoVrijeme), "Item" + e.PunoNepunoRadnoVrijeme),
                    P10 = IsPoslodavac(e) ?
                        int.Parse((Convert.ToDateTime(_archive.Calculation.DatumDo) -
                            Convert.ToDateTime(_archive.Calculation.DatumOd)).Days.ToString()) + 1 :
                        _archive.Calculation.SatiRada,
                    P100 = IsPoslodavac(e) ? 0 : _archive.Calculation.SatiPraznika,
                    P101 = Convert.ToDateTime(_archive.Calculation.DatumOd),
                    P102 = Convert.ToDateTime(_archive.Calculation.DatumDo),
                    P11 = p.Bruto,
                    P12 = p.Bruto,
                    P121 = p.Mio1,
                    P122 = p.Mio2,
                    P123 = p.DoprinosZdravstvo,
                    P124 = 0,
                    P125 = 0,
                    P126 = 0,
                    P127 = 0,
                    P129 = 0,
                    P131 = 0,
                    P132 = p.Mio1 + p.Mio2,
                    P133 = p.Dohodak,
                    P134 = p.Odbitak,
                    P135 = p.PoreznaOsnovica,
                    P141 = p.UkupnoPorez,
                    P142 = p.Prirez,
                    P161 = (tOznakaNacinaIsplate)Enum.Parse(typeof(tOznakaNacinaIsplate), e.NacinIsplate),
                    P162 = p.Neto,
                    P17 = IsPoslodavac(e) ? 0 : p.Bruto
                });
                

                var supplements = _archive.Supplements.Where(x => x.Oib == p.Oib);
                AddSupplements(supplements, e, p, _pArr.Last());
            }
        }

        private void AddSupplements(IEnumerable<PayrollArchiveSupplementModel> supplements, 
            JoppdEmployeeModel e, PayrollArchivePayrollModel p, sPrimateljiP sPrimateljiP)
        {
            for(int i = 0; i<supplements.Count(); i++)
            {
                if (i == 0 && sPrimateljiP.P11 != 0 && (sPrimateljiP.P61 != tOznakaStjecatelja.Item0032 || sPrimateljiP.P62 != tOznakaPrimici.Item0101))
                {
                    sPrimateljiP.P151 = (tOznakaNeoporezivogPrimitka)Enum.Parse(typeof(tOznakaNeoporezivogPrimitka), supplements.ElementAt(i).Sifra);
                    sPrimateljiP.P152 = supplements.ElementAt(i).Iznos;
                    sPrimateljiP.P162 += sPrimateljiP.P152;
                }
                else
                {
                    if (sPrimateljiP.P11 == 0)
                    {
                        _pArr.Remove(sPrimateljiP);
                    }

                    _pArr.Add(new sPrimateljiP()
                    {
                        P1 = _pArr.Count() + 1,
                        P2 = e.SifraPrebivalista,
                        P3 = e.SifraOpcineRada,
                        P4 = e.Oib,
                        P5 = p.Ime + " " + p.Prezime,
                        P61 = 0,
                        P62 = 0,
                        P71 = 0,
                        P72 = 0,
                        P8 = 0,
                        P9 = 0,
                        P10 = 0,
                        P100 = 0,
                        P101 = Convert.ToDateTime(_archive.Calculation.DatumOd),
                        P102 = Convert.ToDateTime(_archive.Calculation.DatumDo),
                        P11 = 0,
                        P12 = 0,
                        P121 = 0,
                        P122 = 0,
                        P123 = 0,
                        P124 = 0,
                        P125 = 0,
                        P126 = 0,
                        P127 = 0,
                        P129 = 0,
                        P131 = 0,
                        P132 = 0,
                        P133 = 0,
                        P134 = 0,
                        P135 = 0,
                        P141 = 0,
                        P142 = 0,
                        P151 = (tOznakaNeoporezivogPrimitka)Enum.Parse(typeof(tOznakaNeoporezivogPrimitka), supplements.ElementAt(i).Sifra),
                        P152 = supplements.ElementAt(i).Iznos,
                        P161 = (tOznakaNacinaIsplate)Enum.Parse(typeof(tOznakaNacinaIsplate), e.NacinIsplate),
                        P162 = supplements.ElementAt(i).Iznos,
                        P17 = 0
                    });
                }
            }
        }

        private bool IsPoslodavac(JoppdEmployeeModel e)
        {
            if (e.OznakaPrimitka.Contains("0101"))
            {
                return true;
            }

            return false;
        }
    }
}
