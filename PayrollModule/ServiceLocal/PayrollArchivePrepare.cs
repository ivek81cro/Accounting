using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollModule.ServiceLocal
{
    public class PayrollArchivePrepare : IPayrollArchivePrepare
    {
        private readonly IPayrollArchiveEndpoint _archiveEndpoint;
        private readonly IAccountPairsEndpoint _accoutPairsEndpoint;
        private readonly string _bookName;

        private PayrollArchiveModel _archive;
        private List<PayrollArchivePayrollModel> _payrolls;
        private List<PayrollArchiveSupplementModel> _supplements;

        private List<PayrollArchivePayrollModel> Payrolls = new();
        private List<PayrollArchiveSupplementModel> Supplements = new();

        public PayrollArchivePrepare(IPayrollArchiveEndpoint archiveEndpoint,
                                     IAccountPairsEndpoint accoutPairsEndpoint)
        {
            _archiveEndpoint = archiveEndpoint;
            _accoutPairsEndpoint = accoutPairsEndpoint;
            _bookName = "Plaća";
        }

        public PayrollArchiveModel Process(List<PayrollArchivePayrollModel> payrollCalculations,
                                           List<PayrollArchiveSupplementModel> supplementCalculations,
                                           PayrollArchiveHeaderModel payrollAccounting)
        {
            _archive = new();
            _payrolls = payrollCalculations;
            _supplements = supplementCalculations;

            if (_payrolls != null)
            {
                AddPayrolls();
            }
            else
            {
                AddSupplementsOnly();
            }

            payrollAccounting.CreateUniqueIdentifier();
            _archive.Header = payrollAccounting;

            return _archive;
        }

        private void AddPayrolls()
        {
            foreach (var pay in _payrolls)
            {
                if (pay.IsChecked)
                {
                    _archive.Payrolls.Add(pay);
                    if (_supplements != null)
                    {
                        AddSupplement(pay.Oib);
                    }
                    else
                    {
                        _archive.Supplements = null;
                    }
                }
            }
        }

        private void AddSupplement(string oib)
        {
            foreach (var sup in _supplements)
            {
                if (sup.Oib == oib && sup.IsChecked)
                {
                    _archive.Supplements.Add(sup);
                }
            }
        }

        private void AddSupplementsOnly()
        {
            foreach (var sup in _supplements)
            {
                if (sup.IsChecked)
                {
                    _archive.Supplements.Add(sup);
                }
            }
        }

        public async Task<bool> SaveToDatabase(PayrollArchiveModel archive)
        {
            _archive = archive;
            bool result = await _archiveEndpoint.IfIdentifierExists(_archive.Header.UniqueId);
            if (!result)
            {
                await _archiveEndpoint.PostToArchive(_archive);
                return true;
            }
            return false;
        }

        private Dictionary<string, decimal> MapColumnToPropertyValue()
        {
            var pay = Payrolls;
            var item = new Dictionary<string, decimal>();
            item.Add("Bruto", pay.Sum(x => x.Bruto));
            item.Add("MIO I.", pay.Sum(x => x.Mio1));
            item.Add("MIO II.", pay.Sum(x => x.Mio2));
            item.Add("Dohodak", pay.Sum(x => x.Dohodak));
            item.Add("Odbitak", pay.Sum(x => x.Odbitak));
            item.Add("Osnovica", pay.Sum(x => x.PoreznaOsnovica));
            item.Add("Por. stopa I.", pay.Sum(x => x.PoreznaStopa1));
            item.Add("Por. stopa II.", pay.Sum(x => x.PoreznaStopa2));
            item.Add("Ukupno porezi", pay.Sum(x => x.UkupnoPorez));
            item.Add("Prirez", pay.Sum(x => x.Prirez));
            item.Add("Por. i prirez", pay.Sum(x => x.UkupnoPorezPrirez));
            item.Add("Neto", pay.Sum(x => x.Neto));
            item.Add("Dop. zdravstvo", pay.Sum(x => x.DoprinosZdravstvo));

            return item;
        }

        public async Task<List<AccountingJournalModel>> CreateJournalEntries(List<PayrollArchivePayrollModel> payrolls,
                                                                             PayrollArchiveHeaderModel selectedArchive,
                                                                             List<BookAccountsSettingsModel> accountingSettings,
                                                                             List<PayrollArchiveSupplementModel> supplements)
        {
            Payrolls = payrolls;
            Supplements = supplements;
            var pairs = await _accoutPairsEndpoint.GetByBookName(_bookName);

            var mappings = MapColumnToPropertyValue();
            var entry = selectedArchive;
            List<AccountingJournalModel> entries = new();
            foreach (var setting in accountingSettings)
            {
                var value = mappings.GetValueOrDefault(setting.Name);
                value *= setting.Prefix ? (-1) : 1;
                if (value != 0)
                {
                    entries.Add(new AccountingJournalModel
                    {
                        Broj = 0,
                        Dokument = entry.Opis,
                        Datum = entry.DatumObracuna,
                        Opis = setting.Name,
                        Konto = setting.Account,
                        Dugovna = setting.Side == "Dugovna" ? value : 0,
                        Potrazna = setting.Side == "Potražna" ? value : 0,
                        Valuta = "HRK",
                        VrstaTemeljnice = _bookName
                    });
                }
            }

            if (supplements.Count > 0)
            {
                var supplSum = supplements.Sum(x => x.Iznos);
                await AddSupplementEntries(entry, entries, supplSum);
            }

            return entries;
        }

        private async Task AddSupplementEntries(PayrollArchiveHeaderModel entry,
                                                List<AccountingJournalModel> entries,
                                                decimal supplSum)
        {
            entries.Add(new AccountingJournalModel
            {
                Broj = 0,
                Dokument = entry.Opis,
                Datum = entry.DatumObracuna,
                Opis = "Dodaci ukupno",
                Konto = "2300",
                Dugovna = 0,
                Potrazna = supplSum,
                Valuta = "HRK",
                VrstaTemeljnice = _bookName
            });

            List<PayrollArchiveSupplementModel> supplements = new();
            supplements = Supplements
                .GroupBy(x => x.Sifra)
                .Select(y => new PayrollArchiveSupplementModel
                {
                    Sifra = y.First().Sifra,
                    Naziv = y.First().Naziv,
                    Iznos = y.Sum(z => z.Iznos)
                }).ToList();

            foreach (var sup in supplements)
            {
                entries.Add(new AccountingJournalModel
                {
                    Broj = 0,
                    Dokument = entry.Opis,
                    Datum = entry.DatumObracuna,
                    Opis = sup.Naziv,
                    Konto = await FindLinkedAccount(entry.Opis, sup.Naziv),
                    Dugovna = sup.Iznos,
                    Potrazna = 0,
                    Valuta = "HRK",
                    VrstaTemeljnice = _bookName
                });

            }
        }

        private async Task<string> FindLinkedAccount(string opis, string naziv)
        {
            var pairs = await _accoutPairsEndpoint.GetByBookName(_bookName);
            string result = null;
            if (pairs.Count != 0)
            {
                foreach (var pair in pairs)
                {
                    if (naziv.Contains(pair.Description))
                    {
                        return result = pair.Account;
                    }
                }
            }

            return result;
        }
    }
}
