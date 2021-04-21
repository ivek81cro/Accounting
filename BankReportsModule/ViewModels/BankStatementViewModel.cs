using AccountingUI.Core.Models;
using AccountingUI.Core.TabControlRegion;
using BankkStatementsModule.XmlModel;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BankkStatementsModule.ViewModels
{
    public class BankStatementViewModel : ViewModelBase
    {
        private readonly IDialogService _showDialog;

        public BankStatementViewModel(IDialogService showDialog)
        {
            _showDialog = showDialog;

            LoadDataCommand = new DelegateCommand(OpenStatementFile);
        }

        private string _path;
        private BankStatementXml _fileXml = new();

        public DelegateCommand LoadDataCommand { get; private set; }

        private BankReportModel _reportHeader;
        public BankReportModel ReportHeader
        {
            get { return _reportHeader; }
            set { SetProperty(ref _reportHeader, value); }
        }

        private List<BankReportItemModel> reportItems = new();
        public List<BankReportItemModel> ReportItems
        {
            get { return reportItems; }
            set { SetProperty(ref reportItems, value); }
        }

        public void LoadReports()
        {

        }

        private void OpenStatementFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "XML file|*.xml",
                Title = "Otvori xml datoteku izvoda"
            };
            openFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (openFileDialog1.FileName != "")
            {
                _path = openFileDialog1.FileName;
            }
            if (_path != "")
                DeserializeIzvodXml();
        }

        private void DeserializeIzvodXml()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("windows-1250");
            StreamReader reader = new StreamReader(_path, encoding);
            XmlSerializer x = new XmlSerializer(_fileXml.GetType());
            _fileXml = (BankStatementXml)x.Deserialize(reader);
            reader.Close();

            CreateStatementHeader();
        }

        private void CreateStatementHeader()
        {
            ReportHeader = new BankReportModel
            {
                DatumIzvoda = DateTime.ParseExact(_fileXml.Izvod.DatumIzvoda, "yyyyMMdd", CultureInfo.InvariantCulture),
                RedniBroj = int.Parse(_fileXml.Izvod.RedniBroj),
                SumaPotrazna = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.PStrana.UkupnaSuma),
                SumaDugovna = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.DStrana.UkupnaSuma),
                StanjePrethodnogIzvoda = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.PrethodnoStanje),
                NovoStanje = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.NovoStanje)
            };

            CreateReportItemsList();

            OpenNewReportDialog();
        }

        private void CreateReportItemsList()
        {
            foreach (var prometStavka in _fileXml.Izvod.Sekcije.Sekcija.Prometi.Promet)
            {
                ReportItems.Add(
                    new BankReportItemModel()
                    {
                        Dugovna = prometStavka.IznosPrometa.Oznaka == "D" ? decimal.Parse(prometStavka.IznosPrometa.Iznos) : 0,
                        Potrazna = prometStavka.IznosPrometa.Oznaka == "P" ? decimal.Parse(prometStavka.IznosPrometa.Iznos) : 0,
                        Strana = prometStavka.IznosPrometa.Oznaka,
                        Naziv = prometStavka.Naziv,
                        Opis = prometStavka.OpisPlacanja
                    });
            }
        }

        private void OpenNewReportDialog()
        {
            var param = new DialogParameters();
            param.Add("header", ReportHeader);
            param.Add("itemsList", ReportItems);
            _showDialog.ShowDialog("IndividualReportDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
    }
}
