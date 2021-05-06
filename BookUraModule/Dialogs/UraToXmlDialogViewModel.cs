using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using BookUraModule.ModelsLocal;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BookUraModule.Dialogs
{
    public class UraToXmlDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ICompanyEndpoint _companyEndpoint;

        private sObrazacURA _ura = new();

        public UraToXmlDialogViewModel(ICompanyEndpoint companyEndpoint)
        {
            _companyEndpoint = companyEndpoint;

            GenerateXmlCommand = new DelegateCommand(GenerateXml);
        }

        public DelegateCommand GenerateXmlCommand { get; private set; }

        public string Title => "XML za ePoreznu";

        public event Action<IDialogResult> RequestClose;

        private ObservableCollection<BookUraRestModel> _uraList;
        public ObservableCollection<BookUraRestModel> UraList
        {
            get { return _uraList; }
            set { SetProperty(ref _uraList, value); }
        }

        private string _autor;
        public string Autor
        {
            get { return _autor; }
            set { SetProperty(ref _autor, value); }
        }

        private DateTime[] _period;
        public DateTime[] Period
        {
            get { return _period; }
            set { SetProperty(ref _period, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var list = parameters.GetValue<List<BookUraRestModel>>("collection");
            UraList = new ObservableCollection<BookUraRestModel>(list);
            Period = parameters.GetValue<DateTime[]>("period");
        }

        private async void GenerateXml()
        {
            CompanyModel company = await _companyEndpoint.Get();
            _ura = new sObrazacURA
            {
                Metapodaci = new sURAmetapodaci
                {
                    Naslov = new sNaslovTemeljni()
                    {
                        Value = "Knjiga primljenih (ulaznih) računa"
                    },
                    Autor = new sAutorTemeljni { Value = Autor },
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
                        DatumOd = Period[0],
                        DatumDo = Period[1]
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
                        Ime = Autor.Split(' ')[0],
                        Prezime = Autor.Split(' ')[1]
                    }
                },
                Tijelo = new sTijelo()
                {
                    Racuni = GeneratBody(),
                    Ukupno = new sRacuniUkupno
                    {
                        U8 = UraList.Sum(x=>x.PoreznaOsnovica5),
                        U9 = UraList.Sum(x => x.PoreznaOsnovica13),
                        U10 = UraList.Sum(x => x.PoreznaOsnovica25),
                        U11 = UraList.Sum(x => x.IznosSPorezom),
                        U12 = UraList.Sum(x => x.UkupniPretporez),
                        U13 = UraList.Sum(x => x.PretporezT5),
                        U14=0,
                        U15 = UraList.Sum(x => x.PretporezT13),
                        U16=0,
                        U17= UraList.Sum(x => x.PretporezT25),
                        U18=0
                    }
                }
            };

            SaveToFile();
        }

        private sRacun[] GeneratBody()
        {
            List<sRacun> racuni = new();

            foreach(var item in UraList)
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
                        R8 = item.PoreznaOsnovica5,
                        R9 = item.PoreznaOsnovica13,
                        R10 = item.PoreznaOsnovica25,
                        R11 = item.IznosSPorezom,
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

        private void SaveToFile()
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "XML file|*.xml",
                Title = "Spremi XML datoteku"
            };
            Nullable<bool> result = sfd.ShowDialog();
            string path;
            if (result != null && result == true)
            {
                path = sfd.FileName;

                TextWriter txtWriter = new StreamWriter(path);
                XmlSerializer x = new XmlSerializer(_ura.GetType());
                x.Serialize(txtWriter, _ura);
                txtWriter.Close();
            }
        }
    }
}
