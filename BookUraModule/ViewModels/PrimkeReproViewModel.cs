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
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace BookUraModule.ViewModels
{
    public class PrimkeReproViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookUraReproEndpoint _bookUraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;
        private readonly IAccountPairsEndpoint _accoutPairsEndpoint;
        private readonly IProcessToJournalService _processToJournalService;

        private readonly string _bookName;

        private bool _loaded = false;
        private int _maxPrimka;

        public PrimkeReproViewModel(IXlsFileReader xlsFileReader,
                                    IBookUraReproEndpoint bookUraEndpoint,
                                    IDialogService showDialog,
                                    IBookAccountSettingsEndpoint settingsEndpoint,
                                    IAccountPairsEndpoint accoutPairsEndpoint,
                                    IProcessToJournalService processToJournalService)
        {
            _xlsFileReader = xlsFileReader;
            _bookUraEndpoint = bookUraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;
            _accoutPairsEndpoint = accoutPairsEndpoint;
            _processToJournalService = processToJournalService;
            
            _bookName = "Primke repromaterijala";

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSavePrimke);
            FilterDataCommand = new DelegateCommand(FilterPrimke);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess); 
            UnmarkProcessedCommand = new DelegateCommand(UnmarkProcessed, CanUnmark);
        }

        #region DelegateCommands
        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand FilterDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        public DelegateCommand UnmarkProcessedCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<BookUraPrimkaReproModel> _uraPrimke;
        public ObservableCollection<BookUraPrimkaReproModel> UraPrimke
        {
            get { return _uraPrimke; }
            set
            {
                SetProperty(ref _uraPrimke, value);
                SaveDataCommand.RaiseCanExecuteChanged();
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
                UnmarkProcessedCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set
            {
                SetProperty(ref _dateTo, value);
                UnmarkProcessedCommand.RaiseCanExecuteChanged();
            }
        }

        private string _filterName;
        public string FilterName
        {
            get { return _filterName; }
            set
            { SetProperty(ref _filterName, value); }
        }

        private BookUraPrimkaReproModel _selectedUraPrimke;
        public BookUraPrimkaReproModel SelectedUraPrimke
        {
            get { return _selectedUraPrimke; }
            set
            {
                SetProperty(ref _selectedUraPrimke, value);
                ProcessItemCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _automaticProcess;
        public bool AutomaticProcess
        {
            get { return _automaticProcess; }
            set { SetProperty(ref _automaticProcess, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        #endregion

        public async void LoadPrimke()
        {
            IsLoading = true;
            StatusMessage = "Učitavam podatke iz baze...";
            var primke = await _bookUraEndpoint.GetAll();
            StatusMessage = "";
            UraPrimke = new ObservableCollection<BookUraPrimkaReproModel>(primke);
            FilterPrimke();
            LoadAccountingSettings();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private void DatagridLoaded() => IsLoading = false;

        #region Filtering datagrid
        private void FilterPrimke()
        {
            _filteredView = CollectionViewSource.GetDefaultView(UraPrimke);
            _filteredView.Filter = o => FilterData((BookUraPrimkaReproModel)o);
        }

        private bool FilterData(BookUraPrimkaReproModel o)
        {
            if (FilterName != null && (DateFrom == null || DateTo == null))
            {
                return o.NazivDobavljaca.ToLower().Contains(FilterName.ToLower());
            }
            else if (FilterName != null && (DateFrom != null || DateTo != null))
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

        #region Load data from file
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
            UraPrimke = new ObservableCollection<BookUraPrimkaReproModel>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                if (!int.TryParse(row[0].ToString(), out _))
                {
                    continue;
                }
                AddDataToList(row);
            }
        }

        private void AddDataToList(DataRow val)
        {
            UraPrimke.Add(new BookUraPrimkaReproModel
            {
                DatumKnjizenja = DateTime.Parse(val[1].ToString()),
                BrojPrimke = int.Parse(val[2].ToString()),
                Storno = val[3].ToString() == "*",
                NettoNabavnaVrijednost = decimal.Parse(val[4].ToString()),
                NazivDobavljaca = val[5].ToString(),
                BrojRacuna = val[6].ToString(),
                FakturnaVrijednost = decimal.Parse(val[7].ToString()),
                DatumRacuna = DateTime.Parse(val[8].ToString()),
                Otpremnica = val[9].ToString() == "DA",
                DospijecePlacanja = DateTime.Parse(val[10].ToString()),
                NabavnaVrijednost = decimal.Parse(val[11].ToString()),
                Rabat = decimal.Parse(val[12].ToString()),
                Pretporez = decimal.Parse(val[13].ToString()),
                VeleprodajniRabat = decimal.Parse(val[14].ToString()),
                CassaSconto = decimal.Parse(val[15].ToString()),
                PorezniBroj = val[16].ToString(),
                BrojUKnjiziUra = int.Parse(val[17].ToString())
            });
        }
        #endregion

        #region Save data to database
        private bool CanSavePrimke()
        {
            return UraPrimke != null && _loaded;
        }

        private async void SaveToDatabase()
        {
            IEnumerable<BookUraPrimkaReproModel> primke = UraPrimke.Where(x => x.BrojUKnjiziUra > _maxPrimka);
            var list = new List<BookUraPrimkaReproModel>(primke);

            StatusMessage = "Zapisujem u bazu podataka...";
            await _bookUraEndpoint.PostPrimke(list);
            StatusMessage = ""; ;

            _loaded = false;
            LoadPrimke();
        }
        #endregion

        #region Load accounting settings
        private void OpenAccountsSettings()
        {
            var list = new List<string>() {"Fakturna vrijednost", "Nabavna vrijednost", "Rabat", "Netto nabavna vrijednost",
                "Pretporez", "Veleprodajni rabat", "Cassa sconto"};
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

        #region Book items processing
        private Dictionary<string, decimal> MapColumnToPropertyValue()
        {
            var primka = SelectedUraPrimke;
            var item = new Dictionary<string, decimal>();
            item.Add("Id", primka.Id);
            item.Add("Netto nabavna vrijednost", primka.NettoNabavnaVrijednost);
            item.Add("Fakturna vrijednost", primka.FakturnaVrijednost);
            item.Add("Nabavna vrijednost", primka.NabavnaVrijednost);
            item.Add("Rabat", primka.Rabat);
            item.Add("Pretporez", primka.Pretporez);
            item.Add("Veleprodajni rabat", primka.VeleprodajniRabat);
            item.Add("Cassa sconto", primka.CassaSconto);

            return item;
        }

        private async Task<List<AccountingJournalModel>> CreateJournalEntries()
        {
            var pairs = await _accoutPairsEndpoint.GetByBookName(_bookName);

            var mappings = MapColumnToPropertyValue();
            var entry = SelectedUraPrimke;
            var entries = new List<AccountingJournalModel>();
            foreach (var setting in AccountingSettings)
            {
                string account = null;
                if (setting.Account == "22")
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
                SelectedUraPrimke = (BookUraPrimkaReproModel)item;
                var entries = await CreateJournalEntries();
                if (!await _processToJournalService.ProcessEntries(entries))
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
            _showDialog.ShowDialog("ProcessToJournal", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    _bookUraEndpoint.MarkAsProcessed(SelectedUraPrimke.BrojUKnjiziUra);
                }
            });
        }
        #endregion

        #region Remove processed checked status
        private bool CanUnmark()
        {
            return DateFrom != null && DateTo != null;
        }

        private void UnmarkProcessed()
        {
            foreach (object item in _filteredView)
            {
                SelectedUraPrimke = (BookUraPrimkaReproModel)item;
                SelectedUraPrimke.Knjizen = false;
            }
        }
        #endregion
    }
}
