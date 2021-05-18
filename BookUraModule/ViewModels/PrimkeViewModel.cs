using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookUraModule.ViewModels
{
    public class PrimkeViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookUraEndpoint _bookUraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;
        private readonly IAccountPairsEndpoint _accoutPairsEndpoint;
        private readonly IProcessToJournalService _processToJournalService;

        private readonly string _bookName;

        private bool _loaded = false;
        private int _maxPrimka;

        public PrimkeViewModel(IXlsFileReader xlsFileReader,
                               IBookUraEndpoint bookUraEndpoint,
                               IDialogService showDialog,
                               IBookAccountSettingsEndpoint settingsEndpoint,
                               IAccountPairsEndpoint accoutPairsEndpoint,
                               IProcessToJournalService processToJournalService)
        {
            _xlsFileReader = xlsFileReader;
            _bookUraEndpoint = bookUraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;
            _bookName = "Primke robe";
            _accoutPairsEndpoint = accoutPairsEndpoint;
            _processToJournalService = processToJournalService;

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSavePrimke);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
            FilterDataCommand = new DelegateCommand(FilterPrimke);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess);
        }

        #region DelegateCommands
        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }
        public DelegateCommand FilterDataCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<BookUraPrimkaModel> _uraPrimke;
        public ObservableCollection<BookUraPrimkaModel> UraPrimke
        {
            get { return _uraPrimke; }
            set
            { 
                SetProperty(ref _uraPrimke, value);
                SaveDataCommand.RaiseCanExecuteChanged();
            }
        }

        private BookUraPrimkaModel _selectedUraPrimke;
        public BookUraPrimkaModel SelectedUraPrimke
        {
            get { return _selectedUraPrimke; }
            set 
            { 
                SetProperty(ref _selectedUraPrimke, value);
                ProcessItemCommand.RaiseCanExecuteChanged();
            }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        private List<BookAccountsSettingsModel> _accountingSettings;
        public List<BookAccountsSettingsModel> AccountingSettings
        {
            get { return _accountingSettings; }
            set { SetProperty(ref _accountingSettings, value); }
        }

        private ICollectionView _filteredView;
        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set 
            { 
                SetProperty(ref _dateFrom, value);
            }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set 
            { 
                SetProperty(ref _dateTo, value);
            }
        }

        private string _filterName;
        public string FilterName
        {
            get { return _filterName; }
            set 
            { SetProperty(ref _filterName, value); }
        }

        private bool _automaticProcess;
        public bool AutomaticProcess
        {
            get { return _automaticProcess; }
            set { SetProperty(ref _automaticProcess, value); }
        }
        #endregion

        public async void LoadPrimke()
        {
            StatusMessage = "Učitavam podatke iz baze...";
            var primke = await _bookUraEndpoint.GetAll();
            StatusMessage = "";
            UraPrimke = new ObservableCollection<BookUraPrimkaModel>(primke);

            LoadAccountingSettings();
        }

        #region Load accounting settings
        private void OpenAccountsSettings()
        {
            var list = new List<string>() {"Maloprodajna vrijednost", "Fakturna vrijednost", "Maloprodajna marža", "Iznos PDV-a",
            "Vrijednost bez poreza", "Nabavna vrijednost", "Maloprodajni rabat", "NettoNabavna vrijednost", "Pretporez", "Veleprodajni rabat",
            "Cassa sconto", "Netto ruc", "Povratna naknada"};
            var parameters = new DialogParameters();
            parameters.Add("columnsList", list);
            parameters.Add("bookName", _bookName);
            _showDialog.ShowDialog("AccountsLinkDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                }
            });
            LoadAccountingSettings();
        }

        private async void LoadAccountingSettings()
        {
            AccountingSettings = await _settingsEndpoint.GetByBookName(_bookName);
        }
        #endregion

        #region Filtering datagrid
        private void FilterPrimke()
        {
            _filteredView = CollectionViewSource.GetDefaultView(UraPrimke);
            _filteredView.Filter = o => FilterData((BookUraPrimkaModel)o);
        }

        private bool FilterData(BookUraPrimkaModel o)
        {
            if (FilterName != null && (DateFrom == null || DateTo == null))
            {
                return o.NazivDobavljaca.ToLower().Contains(FilterName.ToLower());
            }
            else if(FilterName != null && (DateFrom != null || DateTo != null))
            {
                return o.NazivDobavljaca.ToLower().Contains(FilterName.ToLower()) && 
                    o.DatumKnjizenja >= DateFrom && o.DatumKnjizenja <= DateTo;
            }
            else if (FilterName == null && (DateFrom != null || DateTo != null))
            {
                return o.DatumKnjizenja >= DateFrom && o.DatumKnjizenja <= DateTo;
            }
            else
            {
                return true;
            }
        }
        #endregion
        
        #region Loading data from excel
        private void LoadDataFromFile()
        {
            _maxPrimka = UraPrimke.Count > 0 ? UraPrimke.Max(y => y.BrojUKnjiziUra) : 0;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Xlsx Files *.xlsx|*.xlsx|Xls Files *.xls|*.xls|Csv files *.csv|*.csv",
                FilterIndex = 1,
                Multiselect = false
            };

            Nullable<bool> result = ofd.ShowDialog();
            if (result != null && result == true)
            {
                FilePath = ofd.FileName;
                var data = _xlsFileReader.Convert(FilePath, _bookName);
                if (data != null)
                {
                    FromStringToList(data);
                    _loaded = true;
                }
            }
        }

        private void FromStringToList(DataSet data)
        {
            UraPrimke = new ObservableCollection<BookUraPrimkaModel>();
            foreach(DataRow row in data.Tables[0].Rows)
            {
                if(!int.TryParse(row[0].ToString(), out _))
                {
                    continue;
                }
                AddDataToList(row);
            }
        }

        private void AddDataToList(DataRow val)
        {
            UraPrimke.Add(new BookUraPrimkaModel
            {
                DatumKnjizenja = DateTime.Parse(val[1].ToString()),
                BrojPrimke = int.Parse(val[2].ToString()),
                Storno = val[3].ToString() == "*",
                MaloprodajnaVrijednost = decimal.Parse(val[4].ToString()),
                NazivDobavljaca = val[5].ToString(),
                BrojRacuna = val[6].ToString(),
                DatumRacuna = DateTime.Parse(val[7].ToString()),
                Otpremnica = val[8].ToString() == "DA",
                DospijecePlacanja = DateTime.Parse(val[9].ToString()),
                FakturnaVrijednost = decimal.Parse(val[10].ToString()),
                MaloprodajnaMarza = decimal.Parse(val[11].ToString()),
                IznosPdv = decimal.Parse(val[12].ToString()),
                VrijednostBezPoreza = decimal.Parse(val[13].ToString()),
                NabavnaVrijednost = decimal.Parse(val[14].ToString()),
                MaloprodajniRabat = decimal.Parse(val[15].ToString()),
                NettoNabavnaVrijednost = decimal.Parse(val[16].ToString()),
                Pretporez = decimal.Parse(val[17].ToString()),
                VeleprodajniRabat = decimal.Parse(val[18].ToString()),
                CassaSconto = decimal.Parse(val[19].ToString()),
                NettoRuc = decimal.Parse(val[20].ToString()),
                PovratnaNaknada = decimal.Parse(val[21].ToString()),
                PorezniBroj = val[22].ToString(),
                BrojUKnjiziUra = int.Parse(val[23].ToString())
            });
        }
        #endregion

        #region Saving to database
        private bool CanSavePrimke()
        {
            return UraPrimke != null && _loaded;
        }

        private async void SaveToDatabase()
        {
            IEnumerable<BookUraPrimkaModel> primke = UraPrimke.Where(x=>x.BrojUKnjiziUra > _maxPrimka);
            var list = new List<BookUraPrimkaModel>(primke);

            StatusMessage = "Zapisujem u bazu podataka...";
            await _bookUraEndpoint.PostPrimke(list);
            StatusMessage = ""; ;

            _loaded = false;
            LoadPrimke();
        }
        #endregion

        #region Book items processing        
        private Dictionary<string, decimal> MapColumnToPropertyValue()
        {
            var primka = SelectedUraPrimke;
            var item = new Dictionary<string, decimal>();
            item.Add("Maloprodajna vrijednost", primka.MaloprodajnaVrijednost);
            item.Add("Fakturna vrijednost", primka.FakturnaVrijednost);
            item.Add("Maloprodajna marža", primka.MaloprodajnaMarza);
            item.Add("Iznos PDV-a", primka.IznosPdv);
            item.Add("Vrijednost bez poreza", primka.VrijednostBezPoreza);
            item.Add("Nabavna vrijednost", primka.NabavnaVrijednost);
            item.Add("Maloprodajni rabat", primka.MaloprodajniRabat);
            item.Add("NettoNabavna vrijednost", primka.NettoNabavnaVrijednost);
            item.Add("Pretporez", primka.Pretporez);
            item.Add("Veleprodajni rabat", primka.VeleprodajniRabat);
            item.Add("Cassa sconto", primka.CassaSconto);
            item.Add("Netto ruc", primka.NettoRuc);
            item.Add("Povratna naknada", primka.PovratnaNaknada);

            return item;
        }

        private async Task<List<AccountingJournalModel>> CreateJournalEntries()
        {
            var pairs = await _accoutPairsEndpoint.GetByBookName(_bookName);
            
            var mappings = MapColumnToPropertyValue();
            var entry = SelectedUraPrimke;
            var entries = new List<AccountingJournalModel>();
            foreach(var setting in AccountingSettings)
            {
                string account = null;
                if(setting.Account == "22")
                {
                    account = FindLinkedAccount(pairs, setting);
                }
                else
                {
                    account = setting.Account;
                }
                var value = mappings.GetValueOrDefault(setting.Name);
                value *= setting.Prefix ? (-1) : 1;
                if (value != 0)
                {
                    entries.Add(new AccountingJournalModel
                    {
                        Broj = entry.BrojUKnjiziUra,
                        Dokument = entry.NazivDobavljaca + ":Račun br.:" + entry.BrojRacuna,
                        Datum = entry.DatumKnjizenja,
                        Opis = setting.Name,
                        Konto = account,
                        Dugovna = setting.Side == "Dugovna" ? value : 0,
                        Potrazna = setting.Side == "Potražna" ? value : 0,
                        Valuta = "HRK",
                        VrstaTemeljnice = _bookName
                    });
                }
            }
            return entries;
        }

        private string FindLinkedAccount(List<AccountPairModel> pairs, BookAccountsSettingsModel setting)
        {
            string result = null;
            if (pairs.Count != 0)
            {
                result = pairs.Where(
                    p => p.Name == SelectedUraPrimke.NazivDobavljaca
                    && p.Description == setting.Name).DefaultIfEmpty(new AccountPairModel()).FirstOrDefault().Account;
            }

            return result;
        }

        private bool CanProcess()
        {
            return SelectedUraPrimke != null && !SelectedUraPrimke.Knjizen;
        }

        private async void ProcessItem()
        {
            if (AutomaticProcess)
            {
                await ProcessToJournalAutomatic();
            }
            else
            {
                await SendToProcessingDialog();
            }
        }

        private async Task ProcessToJournalAutomatic()
        {
            foreach (var item in _filteredView)
            {
                SelectedUraPrimke = (BookUraPrimkaModel)item;
                var entries = await CreateJournalEntries();
                if(!await _processToJournalService.ProcessEntries(entries))
                {
                    AutomaticProcess = false;
                    await SendToProcessingDialog();
                    break;
                }
                else
                {
                    SelectedUraPrimke.Knjizen = true;
                    await _bookUraEndpoint.MarkAsProcessed(SelectedUraPrimke.BrojUKnjiziUra);
                }
            }
        }

        private async Task SendToProcessingDialog()
        {
            var entries = await CreateJournalEntries();
            var parameters = new DialogParameters();
            parameters.Add("entries", entries);
            parameters.Add("automatic", AutomaticProcess);
            _showDialog.ShowDialog("ProcessToJournal", parameters, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedUraPrimke.Knjizen = true;
                    await _bookUraEndpoint.MarkAsProcessed(SelectedUraPrimke.BrojUKnjiziUra);
                }
            });
        }
        #endregion
    }
}
