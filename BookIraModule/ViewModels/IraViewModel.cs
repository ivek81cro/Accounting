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

namespace BookIraModule.ViewModels
{
    public class IraViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookIraEndpoint _bookIraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;
        private readonly IAccountPairsEndpoint _accoutPairsEndpoint;
        private readonly IProcessToJournalService _processToJournalService;

        private readonly string _bookName;

        private bool _loaded = false;
        private int _maxRedniBroj;

        public IraViewModel(IXlsFileReader xlsFileReader,
                            IBookIraEndpoint bookIraEndpoint,
                            IDialogService showDialog,
                            IBookAccountSettingsEndpoint settingsEndpoint,
                            IAccountPairsEndpoint accoutPairsEndpoint,
                            IProcessToJournalService processToJournalService)
        {
            _xlsFileReader = xlsFileReader;
            _bookIraEndpoint = bookIraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;
            _accoutPairsEndpoint = accoutPairsEndpoint;
            _processToJournalService = processToJournalService;

            _bookName = "Knjiga izlaznih računa";

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSaveItems);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
            FilterDataCommand = new DelegateCommand(FilterPrimke);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess);
            CalculationsReportCommand = new DelegateCommand(ShowCalculationDialog);
            UnmarkProcessedCommand = new DelegateCommand(UnmarkProcessed, CanUnmark);
            OpenEditCommand = new DelegateCommand(EditSelectedRow, CanEditRow);

            LoadIra();
        }

        #region Delegate commands
        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }
        public DelegateCommand FilterDataCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        public DelegateCommand CalculationsReportCommand { get; private set; }
        public DelegateCommand UnmarkProcessedCommand { get; private set; }
        public DelegateCommand OpenEditCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<BookIraModel> _iraItems;
        public ObservableCollection<BookIraModel> IraItems
        {
            get { return _iraItems; }
            set
            {
                SetProperty(ref _iraItems, value);
                SaveDataCommand.RaiseCanExecuteChanged();
            }
        }

        private BookIraModel _selectedIra;
        public BookIraModel SelectedIra
        {
            get { return _selectedIra; }
            set
            {
                SetProperty(ref _selectedIra, value);
                ProcessItemCommand.RaiseCanExecuteChanged();
            }
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

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        private List<BookAccountsSettingsModel> _accountingSettings;
        public List<BookAccountsSettingsModel> AccountingSettings
        {
            get { return _accountingSettings; }
            set { SetProperty(ref _accountingSettings, value); }
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

        #region Data loading
        public async void LoadIra()
        {
            IsLoading = true;
            var primke = await _bookIraEndpoint.GetAll();
            IraItems = new ObservableCollection<BookIraModel>(primke);
            FilterPrimke();
            LoadAccountingSettings();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private void DatagridLoaded() => IsLoading = false;
        #endregion

        #region Load accounting settings
        private void OpenAccountsSettings()
        {
            var list = new List<string>() { "Iznos s Pdv", "Oslobođeno Pdv-a EU", "Oslobođeno Pdv-a ostalo", "Prolazna stavka", "Osnovica 0%",
                "Osnovica 5%", "Pdv 5%", "Osnovica 10%", "Pdv 10%", "Osnovica 13%", "Pdv 13%", "Osnovica 25%", "Pdv ukupno", "Ukupno uplaćeno"};
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
            _filteredView = CollectionViewSource.GetDefaultView(IraItems);
            _filteredView.Filter = o => FilterData((BookIraModel)o);
        }

        private bool FilterData(BookIraModel o)
        {
            if (FilterName != null && (DateFrom == null || DateTo == null))
            {
                return o.NazivISjedisteKupca.ToLower().Contains(FilterName.ToLower());
            }
            else if (FilterName != null && (DateFrom != null || DateTo != null))
            {
                return o.NazivISjedisteKupca.ToLower().Contains(FilterName.ToLower()) &&
                    o.Datum >= DateFrom && o.Datum <= DateTo;
            }
            else if (FilterName == null && (DateFrom != null || DateTo != null))
            {
                return o.Datum >= DateFrom && o.Datum <= DateTo;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Load data from file
        private async void LoadDataFromFile()
        {
            _maxRedniBroj = IraItems.Count > 0 ? IraItems.Max(y => y.RedniBroj) : 0;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Xlsx Files *.xlsx|*.xlsx|Xls Files *.xls|*.xls|Csv files *.csv|*.csv",
                FilterIndex = 1,
                Multiselect = false
            };

            Nullable<bool> result = ofd.ShowDialog();
            if (result != null && result == true)
            {
                IsLoading = true;
                FilePath = ofd.FileName;
                var data = _xlsFileReader.Convert(FilePath, _bookName);
                if (data != null)
                {
                    FromStringToList(data);
                    _loaded = true;
                }
                await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
            }
        }

        private void FromStringToList(DataSet data)
        {
            IraItems = new ObservableCollection<BookIraModel>();
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
            IraItems.Add(new BookIraModel
            {
                RedniBroj = int.Parse(val[1].ToString()),
                BrojRacuna = val[2].ToString(),
                Storno = val[3].ToString() == "1",
                IzRacuna = int.Parse(val[4].ToString()),
                Datum = DateTime.Parse(val[5].ToString()),
                Dospijece = DateTime.Parse(val[6].ToString()),
                DatumZadnjeUplate = val[7].ToString() == "" ? null : DateTime.Parse(val[7].ToString()),
                NazivISjedisteKupca = val[8].ToString(),
                Oib = val[9].ToString(),
                IznosSPdv = decimal.Parse(val[10].ToString()),
                OslobodjenoPdvEU = decimal.Parse(val[11].ToString()),
                OslobodjenoPdvOstalo = decimal.Parse(val[12].ToString()),
                ProlaznaStavka = decimal.Parse(val[13].ToString()),
                PoreznaOsnovica0 = decimal.Parse(val[14].ToString()),
                PoreznaOsnovica5 = decimal.Parse(val[15].ToString()),
                Pdv5 = decimal.Parse(val[16].ToString()),
                PoreznaOsnovica10 = decimal.Parse(val[17].ToString()),
                Pdv10 = decimal.Parse(val[18].ToString()),
                PoreznaOsnovica13 = decimal.Parse(val[19].ToString()),
                Pdv13 = decimal.Parse(val[20].ToString()),
                PoreznaOsnovica23 = decimal.Parse(val[21].ToString()),
                Pdv23 = decimal.Parse(val[22].ToString()),
                PoreznaOsnovica25 = decimal.Parse(val[23].ToString()),
                Pdv25 = decimal.Parse(val[24].ToString()),
                UkupniPdv = decimal.Parse(val[25].ToString()),
                UkupnoUplaceno = decimal.Parse(val[26].ToString()),
                PreostaloZaUplatit = decimal.Parse(val[27].ToString()),
                NapomenaORacunu = val[28].ToString(),
                ZaprimljenUHzzo = val[29].ToString() == "" ? null : DateTime.Parse(val[29].ToString()),
                DanaOdZaprimanja = int.Parse(val[30].ToString()),
                DanaNeplacanja = int.Parse(val[31].ToString())
            });
        }
        #endregion

        #region Save to database
        private bool CanSaveItems()
        {
            return IraItems != null && _loaded;
        }

        private async void SaveToDatabase()
        {
            IEnumerable<BookIraModel> primke = IraItems.Where(x => x.RedniBroj > _maxRedniBroj);
            var list = new List<BookIraModel>(primke);

            IsLoading = true;
            await _bookIraEndpoint.PostPrimke(list);

            _loaded = false;
            LoadIra();
        }
        #endregion

        #region Book item processing
        private Dictionary<string, decimal> MapColumnToPropertyValue()
        {
            var iraItem = SelectedIra;
            var item = new Dictionary<string, decimal>();
            item.Add("Iznos s Pdv", iraItem.IznosSPdv);
            item.Add("Oslobođeno Pdv-a EU", iraItem.OslobodjenoPdvEU);
            item.Add("Oslobođeno Pdv-a ostalo", iraItem.OslobodjenoPdvOstalo);
            item.Add("Prolazna stavka", iraItem.ProlaznaStavka);
            item.Add("Osnovica 0%", iraItem.PoreznaOsnovica0);
            item.Add("Osnovica 5%", iraItem.PoreznaOsnovica5);
            item.Add("Pdv 5%", iraItem.Pdv5);
            item.Add("Osnovica 10%", iraItem.PoreznaOsnovica10);
            item.Add("Pdv 10%", iraItem.Pdv10);
            item.Add("Osnovica 13%", iraItem.PoreznaOsnovica13);
            item.Add("Pdv 13%", iraItem.Pdv13);
            item.Add("Osnovica 25%", iraItem.PoreznaOsnovica25);
            item.Add("Pdv 25%", iraItem.Pdv25);
            item.Add("Pdv ukupno", iraItem.UkupniPdv);
            item.Add("Ukupno uplaćeno", iraItem.UkupnoUplaceno);

            return item;
        }

        private async Task<List<AccountingJournalModel>> CreateJournalEntries()
        {
            var pairs = await _accoutPairsEndpoint.GetByBookName(_bookName);

            var mappings = MapColumnToPropertyValue();
            var entry = SelectedIra;
            var entries = new List<AccountingJournalModel>();
            foreach (var setting in AccountingSettings)
            {
                if (entry.NazivISjedisteKupca.Contains("PROMET"))
                {
                    entry.NazivISjedisteKupca = "PROMET";
                }
                string account = null;
                if (setting.Account == "12")
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
                        Broj = entry.RedniBroj,
                        Dokument = entry.NazivISjedisteKupca + ":Račun br.:" + entry.BrojRacuna,
                        Datum = entry.Datum,
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
                    p => p.Name == SelectedIra.NazivISjedisteKupca
                    && p.Description == setting.Name).DefaultIfEmpty(new AccountPairModel()).FirstOrDefault().Account;
            }

            return result;
        }

        private bool CanProcess()
        {
            return SelectedIra != null && !SelectedIra.Knjizen;
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
                if (SelectedIra.Knjizen)
                {
                    continue;
                }

                SelectedIra = (BookIraModel)item;
                var entries = await CreateJournalEntries();
                bool check = entries.Sum(x => x.Dugovna) == entries.Sum(x => x.Potrazna);
                if (!check)
                {
                    AutomaticProcess = false;
                    await SendToProcessingDialog();
                    break;
                }
                if (!await _processToJournalService.ProcessEntries(entries))
                {
                    AutomaticProcess = false;
                    await SendToProcessingDialog();
                    break;
                }
                else
                {
                    SelectedIra.Knjizen = true;
                    await _bookIraEndpoint.MarkAsProcessed(SelectedIra.RedniBroj);
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
                    SelectedIra.Knjizen = true;
                    _bookIraEndpoint.MarkAsProcessed(SelectedIra.RedniBroj);
                }
            });
        }
        #endregion

        #region Calculations dialog
        private void ShowCalculationDialog()
        {
            var parameters = new DialogParameters();
            var filteredItems = _filteredView.Cast<BookIraModel>().ToList();
            parameters.Add("collection", filteredItems);
            _showDialog.ShowDialog("IraCalculation", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

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
                SelectedIra = (BookIraModel)item;
                SelectedIra.Knjizen = false;
            }
        }
        #endregion

        #region Row editing
        private async void EditSelectedRow()
        {
            await _bookIraEndpoint.PostRow(SelectedIra);
        }

        private bool CanEditRow() => SelectedIra != null;
        
        public void ResetSelectedItem()
        {
            SelectedIra = null;
        }
        #endregion
    }
}
