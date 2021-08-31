using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using BankkStatementsModule.XmlModel;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BankkStatementsModule.ViewModels
{
    public class BankStatementViewModel : ViewModelBase
    {
        private readonly IDialogService _showDialog;
        private readonly IBankReportEndpoint _bankReportEndpoint;

        private string _path;
        private string _bookName = "Izvodi";
        private bool _isDelete = false;

        private BankStatementXml _fileXml = new();

        public BankStatementViewModel(IDialogService showDialog,
                                      IBankReportEndpoint bankReportEndpoint)
        {
            _showDialog = showDialog;
            _bankReportEndpoint = bankReportEndpoint;

            LoadDataCommand = new DelegateCommand(OpenStatementFile);
            LoadReportCommand = new DelegateCommand(LoadReport);
            ChangeDataCommand = new DelegateCommand(ChangeData);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess);
            DeleteReportCommand = new DelegateCommand(DeleteReport, CanDelete);
            OpenCardCommand = new DelegateCommand(OpenBalanceCard);

            LoadReports();
        }

        #region Delegate commands
        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand LoadReportCommand { get; private set; }
        public DelegateCommand ChangeDataCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        public DelegateCommand DeleteReportCommand { get; private set; }
        public DelegateCommand OpenCardCommand { get; private set; }
        #endregion

        #region Properties
        private BankReportModel _reportHeader;
        public BankReportModel SelectedReportHeader
        {
            get { return _reportHeader; }
            set { SetProperty(ref _reportHeader, value); }
        }

        private ObservableCollection<BankReportModel> _reportHeaders;
        public ObservableCollection<BankReportModel> ReportHeaders
        {
            get { return _reportHeaders; }
            set { SetProperty(ref _reportHeaders, value); }
        }

        private List<BankReportItemModel> _reportItems;
        public List<BankReportItemModel> ReportItems
        {
            get { return _reportItems; }
            set { SetProperty(ref _reportItems, value); }
        }

        private BankReportItemModel _selectedReportItem;
        public BankReportItemModel SelectedReportItem
        {
            get { return _selectedReportItem; }
            set { SetProperty(ref _selectedReportItem, value); }
        }

        private decimal _sumDugovna;
        public decimal SumDugovna
        {
            get { return _sumDugovna; }
            set { SetProperty(ref _sumDugovna, value); }
        }

        private decimal _sumPotrazna;
        public decimal SumPotrazna
        {
            get { return _sumPotrazna; }
            set { SetProperty(ref _sumPotrazna, value); }
        }
        #endregion

        #region Load reports list
        public async void LoadReports()
        {
            var list = await _bankReportEndpoint.GetAllHeaders();
            ReportHeaders = new ObservableCollection<BankReportModel>(list);
        }
        #endregion

        #region Open XML file, deserialize
        private void OpenStatementFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "XML file|*.xml",
                Title = "Otvori xml datoteku izvoda"
            };
            bool result = (bool)openFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (result)
            {
                _path = openFileDialog1.FileName;
                DeserializeIzvodXml();
            }
        }

        private void DeserializeIzvodXml()
        {
            if (_path != null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var encoding = Encoding.GetEncoding("windows-1250");
                StreamReader reader = new StreamReader(_path, encoding);
                XmlSerializer x = new XmlSerializer(_fileXml.GetType());
                _fileXml = (BankStatementXml)x.Deserialize(reader);
                reader.Close();

                CreateStatementHeader();
            }
        }

        private void CreateStatementHeader()
        {
            SelectedReportHeader = new BankReportModel
            {
                DatumIzvoda = DateTime.ParseExact(_fileXml.Izvod.DatumIzvoda, "yyyyMMdd", CultureInfo.InvariantCulture),
                RedniBroj = int.Parse(_fileXml.Izvod.RedniBroj),
                SumaPotrazna = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.PStrana.UkupnaSuma),
                SumaDugovna = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.DStrana.UkupnaSuma),
                StanjePrethodnogIzvoda = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.PrethodnoStanje),
                NovoStanje = decimal.Parse(_fileXml.Izvod.Sekcije.Sekcija.NovoStanje)
            };

            CreateReportItemsList();

            OpenIndividualReportDialog();
        }

        private void CreateReportItemsList()
        {
            _reportItems = new();
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

        private void OpenIndividualReportDialog()
        {
            bool exists = ReportHeaders.Where(x => x.RedniBroj == SelectedReportHeader.RedniBroj).Count() > 0;
            var param = new DialogParameters();
            param.Add("header", SelectedReportHeader);
            param.Add("itemsList", ReportItems);
            param.Add("exists", exists);
            _showDialog.ShowDialog("IndividualReportDialog", param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoadReports();
                    SelectedReportHeader = null;
                    ReportItems = null;
                }
            });
        }
        #endregion

        #region Load selected report details
        private async void LoadReport()
        {
            if (SelectedReportHeader != null && SelectedReportHeader.Id > 0)
            {
                ReportItems = await _bankReportEndpoint.GetItems(SelectedReportHeader.Id);
                SumSides();
                ProcessItemCommand.RaiseCanExecuteChanged();
                DeleteReportCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Processing to journal
        private List<AccountingJournalModel> CreateJournalEntries()
        {
            var entries = new List<AccountingJournalModel>();
            string isStorno = _isDelete ? "STORNO " : "";
            entries.Add(new AccountingJournalModel
            {
                Broj = SelectedReportHeader.RedniBroj,
                Dokument = $"{isStorno}Izvod br.:" + SelectedReportHeader.RedniBroj,
                Datum = SelectedReportHeader.DatumIzvoda,
                Opis = "Žiro račun: Izvod br. " + SelectedReportHeader.RedniBroj,
                Konto = "1000",
                Dugovna = SelectedReportHeader.SumaPotrazna,
                Potrazna = SelectedReportHeader.SumaDugovna,
                Valuta = "HRK",
                VrstaTemeljnice = _bookName
            });
            foreach (var entry in ReportItems)
            {
                entries.Add(new AccountingJournalModel
                {
                    Broj = SelectedReportHeader.RedniBroj,
                    Dokument = $"{isStorno}Izvod br.:" + SelectedReportHeader.RedniBroj,
                    Datum = SelectedReportHeader.DatumIzvoda,
                    Opis = entry.Opis,
                    Konto = entry.Konto,
                    Dugovna = entry.Dugovna,
                    Potrazna = entry.Potrazna,
                    Valuta = "HRK",
                    VrstaTemeljnice = _bookName
                });
            }
            return entries;
        }

        private bool CanProcess() => SelectedReportHeader != null && !SelectedReportHeader.Knjizen && ReportItems != null;

        private void ProcessItem()
        {
            var entries = CreateJournalEntries();
            var parameters = new DialogParameters();
            parameters.Add("entries", entries);
            _showDialog.ShowDialog("ProcessToJournal", parameters, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedReportHeader.Knjizen = true;
                    await _bankReportEndpoint.UpdateHeader(SelectedReportHeader);
                    if (!_isDelete)
                    {
                        SelectedReportItem = null;
                        ReportItems = null;
                    }
                }
            });
            if(!_isDelete)
            {
                LoadReports();
                ReportItems = null;
            }
            _isDelete = false;
        }
        #endregion

        #region Delete report
        private bool CanDelete()
        {
            return SelectedReportHeader != null;
        }

        private void DeleteReport()
        {
            var parameters = new DialogParameters();
            string messageParam = SelectedReportHeader.Knjizen ? 
                "Brisanjem KNJIŽENOG izvoda podaci knjiženi na temeljnicu ostaju, " +
                "napravit će se ispravak i na temeljnici storniranjem ovog izvoda.\n\n" +
                "Želite li svejedno brisati odabrani izvod?" : 
                "Jeste li sigurni da želite brisati odabrani izvod?";
            parameters.Add("message", messageParam);
            _showDialog.Show("AreYouSureView", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    if (SelectedReportHeader.Knjizen)
                    {
                        SelectedReportHeader.SumaDugovna *= -1;
                        SelectedReportHeader.SumaPotrazna *= -1;
                        foreach (var entry in ReportItems)
                        {
                            entry.Dugovna *= -1.0m;
                            entry.Potrazna *= -1.0m;
                        }
                        _isDelete = true;
                        ProcessItem();
                    }
                    _bankReportEndpoint.Delete(SelectedReportHeader.Id);
                    ReportHeaders.Remove(SelectedReportHeader);
                    ReportItems = null;
                }
            });
        }
        #endregion

        private void OpenBalanceCard()
        {
            var parameters = new DialogParameters();
            parameters.Add("accountNumber", SelectedReportItem.Konto);
            _showDialog.Show("BalanceCardDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }

        private void SumSides()
        {
            SumDugovna = ReportItems.Sum(x => x.Dugovna);
            SumPotrazna = ReportItems.Sum(x => x.Potrazna);
        }

        private void ChangeData()
        {
            if (SelectedReportHeader != null && SelectedReportHeader.Id > 0 && ReportItems != null)
            {
                OpenIndividualReportDialog();
            }
        }
    }
}
