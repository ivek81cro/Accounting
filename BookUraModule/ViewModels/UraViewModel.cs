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
    public class UraViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookUraRestEndpoint _bookUraRestEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;
        private readonly IAccountPairsEndpoint _accoutPairsEndpoint;
        private readonly IProcessToJournalService _processToJournalService;

        private readonly string _bookName;

        private bool _loaded = false;
        private int _maxUraRedniBroj;

        public UraViewModel(IXlsFileReader xlsFileReader,
                             IBookUraRestEndpoint bookUraRestEndpoint,
                             IDialogService showDialog,
                             IBookAccountSettingsEndpoint settingsEndpoint,
                             IAccountPairsEndpoint accoutPairsEndpoint,
                             IProcessToJournalService processToJournalService)
        {
            _xlsFileReader = xlsFileReader;
            _bookUraRestEndpoint = bookUraRestEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;
            _accoutPairsEndpoint = accoutPairsEndpoint;
            _processToJournalService = processToJournalService;

            _bookName = "Knjiga ulaznih računa";

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSavePrimke);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
            FilterDataCommand = new DelegateCommand(FilterPrimke);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess);
            CalculationsReportCommand = new DelegateCommand(ShowCalculationDialog);
            LoadExpendituresCommand = new DelegateCommand(LoadOnlyRestExpenditures);
            LoadRetailCommand = new DelegateCommand(LoadRetailInvoices);
            CreateUraXmlCommand = new DelegateCommand(CreateUraXml, CanCreateXml);
            UnmarkProcessedCommand = new DelegateCommand(UnmarkProcessed, CanUnmark);
            OpenEditCommand = new DelegateCommand(EditSelectedRow, CanEditRow);

            LoadPrimke();
        }

        #region Delegate commands
        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }
        public DelegateCommand FilterDataCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        public DelegateCommand CalculationsReportCommand { get; private set; }
        public DelegateCommand LoadExpendituresCommand { get; private set; }
        public DelegateCommand LoadRetailCommand { get; private set; }
        public DelegateCommand CreateUraXmlCommand { get; private set; }
        public DelegateCommand UnmarkProcessedCommand { get; private set; }
        public DelegateCommand OpenEditCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<BookUraRestModel> _uraRestInvoices;
        public ObservableCollection<BookUraRestModel> UraRestInvoices
        {
            get { return _uraRestInvoices; }
            set
            {
                SetProperty(ref _uraRestInvoices, value);
                SaveDataCommand.RaiseCanExecuteChanged();
            }
        }

        private BookUraRestModel _selectedUraPrimke;
        public BookUraRestModel SelectedUraPrimke
        {
            get { return _selectedUraPrimke; }
            set
            {
                SetProperty(ref _selectedUraPrimke, value);
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
                CreateUraXmlCommand.RaiseCanExecuteChanged();
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
                CreateUraXmlCommand.RaiseCanExecuteChanged();
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
        public async void LoadPrimke()
        {
            IsLoading = true;
            await LoadInitialData();
            FilterPrimke();
            LoadAccountingSettings();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private async Task LoadInitialData()
        {
            var ulazniRacuni = await _bookUraRestEndpoint.GetAll();
            UraRestInvoices = new ObservableCollection<BookUraRestModel>(ulazniRacuni);
        }

        private void DatagridLoaded() => IsLoading = false;

        public async void LoadOnlyRestExpenditures()
        {
            IsLoading = true;
            await LoadInitialData();
            var expenditures = UraRestInvoices.Where(x => (x.BrojPrimke == 0 && x.IznosSPorezom > 0 && !x.Storno) 
                                                            || (x.BrojPrimke == 0 && x.IznosSPorezom < 0 && x.Storno)).ToList();
            UraRestInvoices = new ObservableCollection<BookUraRestModel>(expenditures);
            FilterPrimke();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }
        
        public async void LoadRetailInvoices()
        {
            IsLoading = true;
            await LoadInitialData();
            var expenditures = UraRestInvoices.Where(x => x.BrojPrimke != 0 ).ToList();
            UraRestInvoices = new ObservableCollection<BookUraRestModel>(expenditures);
            FilterPrimke();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }
        #endregion

        #region Load accounting settings
        private void OpenAccountsSettings()
        {
            var list = new List<string>() { "Planirana uplata", "Za uplatu", "Netto nabavna vrijednost", "Iznos s porezom", 
                "Osnovica 0%", "Osnovica 5%", "Pretporez T5", "Osnovica 10%", "Pretporez T10", "Osnovica 13%", "Pretporez T13", 
                "Osnovica 23%", "Pretporez T23", "Osnovica 25%", "Pretporez T25", "Ukupni pretporez", "Može se odbiti", 
                "Ne može se odbiti", "Iznos bez poreza", "Prolazna stavka", "Cassa sconto", "Odobreni PDV", "Ukupno uplaćeno", 
                "Preostalo za uplatit" };
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
            _filteredView = CollectionViewSource.GetDefaultView(UraRestInvoices);
            _filteredView.Filter = o => FilterData((BookUraRestModel)o);
        }

        private bool FilterData(BookUraRestModel o)
        {
            if (FilterName != null && (DateFrom == null || DateTo == null))
            {
                return o.NazivDobavljaca.ToLower().Contains(FilterName.ToLower());
            }
            else if (FilterName != null && (DateFrom != null || DateTo != null))
            {
                return o.NazivDobavljaca.ToLower().Contains(FilterName.ToLower()) &&
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
            _maxUraRedniBroj = UraRestInvoices.Count > 0 ? UraRestInvoices.Max(y => y.RedniBroj) : 0;

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
            UraRestInvoices = new ObservableCollection<BookUraRestModel>();
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
            UraRestInvoices.Add(new BookUraRestModel
            {
                RedniBroj = int.Parse(val[1].ToString()),
                Datum = DateTime.Parse(val[2].ToString()),
                BrojRacuna = val[3].ToString(),
                Storno = val[4].ToString() == "*",
                StornoBroja = int.Parse(val[5].ToString()),
                DatumRacuna = DateTime.Parse(val[6].ToString()),
                StarostRacuna = int.Parse(val[7].ToString()),
                Dospijece = DateTime.Parse(val[8].ToString()),
                PlaniranaUplata = val[9].ToString() == "" ? 0 : decimal.Parse(val[9].ToString()),
                DatumUplate = val[10].ToString() == "" ? null : DateTime.Parse(val[10].ToString()),
                ZaUplatu = decimal.Parse(val[11].ToString()),
                NazivDobavljaca = val[12].ToString(),
                BrojPrimke = int.Parse(val[13].ToString()),
                NapomenaORacunu = val[14].ToString(),
                NettoNabavnaVrijednost = decimal.Parse(val[15].ToString()),
                SjedisteDobavljaca = val[16].ToString(),
                OIB = val[17].ToString(),
                IznosSPorezom = decimal.Parse(val[18].ToString()),
                PoreznaOsnovica0 = decimal.Parse(val[19].ToString()),
                PoreznaOsnovica5 = decimal.Parse(val[20].ToString()),
                PretporezT5 = decimal.Parse(val[21].ToString()),
                PoreznaOsnovica10 = decimal.Parse(val[22].ToString()),
                PretporezT10 = decimal.Parse(val[23].ToString()),
                PoreznaOsnovica13 = decimal.Parse(val[24].ToString()),
                PretporezT13 = decimal.Parse(val[25].ToString()),
                PoreznaOsnovica23 = decimal.Parse(val[26].ToString()),
                PretporezT23 = decimal.Parse(val[27].ToString()),
                PoreznaOsnovica25 = decimal.Parse(val[28].ToString()),
                PretporezT25 = decimal.Parse(val[29].ToString()),
                UkupniPretporez = decimal.Parse(val[30].ToString()),
                MozeSeOdbiti = decimal.Parse(val[31].ToString()),
                NeMozeSeOdbiti = decimal.Parse(val[32].ToString()),
                IznosBezPoreza = decimal.Parse(val[33].ToString()),
                ProlaznaStavka = decimal.Parse(val[34].ToString()),
                Neoporezivo = decimal.Parse(val[35].ToString()),
                CassaScontoPercent = decimal.Parse(val[36].ToString()),
                CassaSconto = decimal.Parse(val[37].ToString()),
                BrojOdobrenja = val[38].ToString(),
                OdobrenjaBezPDV = val[39].ToString(),
                OdobreniPDV = decimal.Parse(val[40].ToString()),
                DatumPodnosenja = val[41].ToString() == "" ? null : DateTime.Parse(val[41].ToString()),
                DatumIzvrsenja = val[42].ToString() == "" ? null : DateTime.Parse(val[42].ToString()),
                UkupnoUplaceno = decimal.Parse(val[43].ToString()),
                PreostaloZaUplatit = decimal.Parse(val[44].ToString()),
                DospijeceDana = int.Parse(val[45].ToString())
            });
        }
        #endregion

        #region Save to database
        private bool CanSavePrimke()
        {
            return UraRestInvoices != null && _loaded;
        }

        private async void SaveToDatabase()
        {
            IEnumerable<BookUraRestModel> primke = UraRestInvoices.Where(x => x.RedniBroj > _maxUraRedniBroj);
            var list = new List<BookUraRestModel>(primke);
            
            IsLoading = true;
            await _bookUraRestEndpoint.PostPrimke(list);

            _loaded = false;
            LoadPrimke();
        }
        #endregion

        #region Book items processing
        private Dictionary<string, decimal> MapColumnToPropertyValue()
        {
            var primka = SelectedUraPrimke;
            var item = new Dictionary<string, decimal>();
            item.Add("Planirana uplata", primka.PlaniranaUplata);
            item.Add("Za uplatu", primka.ZaUplatu);
            item.Add("Netto nabavna vrijednost", primka.NettoNabavnaVrijednost);
            item.Add("Iznos s PDV", primka.IznosSPorezom);
            item.Add("Osnovica 0%", primka.PoreznaOsnovica0);
            item.Add("Osnovica 5%", primka.PoreznaOsnovica5);
            item.Add("Pretporez T5", primka.PretporezT5);
            item.Add("Osnovica 10%", primka.PoreznaOsnovica10);
            item.Add("Pretporez T10", primka.PretporezT10);
            item.Add("Osnovica 13%", primka.PoreznaOsnovica13);
            item.Add("Pretporez T13", primka.PretporezT13);
            item.Add("Osnovica 23%", primka.PoreznaOsnovica23);
            item.Add("Pretporez T23", primka.PretporezT23);
            item.Add("Osnovica 25%", primka.PoreznaOsnovica25);
            item.Add("Pretporez T25", primka.PretporezT25);
            item.Add("Ukupni pretporez", primka.UkupniPretporez);
            item.Add("Može se odbiti", primka.MozeSeOdbiti);
            item.Add("Ne može se odbiti", primka.NeMozeSeOdbiti);
            item.Add("Iznos bez poreza", primka.IznosBezPoreza);
            item.Add("Prolazna stavka", primka.ProlaznaStavka);
            item.Add("Neoporezivo", primka.Neoporezivo);
            item.Add("Cassa sconto", primka.CassaSconto);
            item.Add("Odobreni PDV", primka.OdobreniPDV);
            item.Add("Ukupno uplaćeno", primka.UkupnoUplaceno);
            item.Add("Preostalo za uplatit", primka.PreostaloZaUplatit);

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
                        Broj = entry.RedniBroj,
                        Dokument = entry.NazivDobavljaca + ":Račun br.:" + entry.BrojRacuna,
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
                    p => p.Name == SelectedUraPrimke.NazivDobavljaca
                    && p.Description == setting.Name).DefaultIfEmpty(new AccountPairModel()).FirstOrDefault().Account;
            }

            return result;
        }

        private bool CanProcess()
        {
            return SelectedUraPrimke != null && SelectedUraPrimke.BrojPrimke == 0 && !SelectedUraPrimke.Knjizen;
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
                SelectedUraPrimke = (BookUraRestModel)item;
                if (SelectedUraPrimke.Knjizen)
                {
                    continue;
                }

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
                    SelectedUraPrimke.Knjizen = true;
                    await _bookUraRestEndpoint.MarkAsProcessed(SelectedUraPrimke.RedniBroj);
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
                    SelectedUraPrimke.Knjizen = true;
                    _bookUraRestEndpoint.MarkAsProcessed(SelectedUraPrimke.RedniBroj);
                }
            });
        }
        #endregion

        #region Calculations dialog
        private void ShowCalculationDialog()
        {
            var parameters = new DialogParameters();
            var filteredItems = _filteredView.Cast<BookUraRestModel>().ToList();
            parameters.Add("collection", filteredItems);
            _showDialog.ShowDialog("UraCalculation", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            });
        }
        #endregion

        #region U-RA creation
        private bool CanCreateXml()
        {
            return DateFrom != null && DateTo != null;
        }

        private void CreateUraXml()
        {
            var parameters = new DialogParameters();
            var filteredItems = _filteredView.Cast<BookUraRestModel>().ToList();
            DateTime[] period = { (DateTime)DateFrom, (DateTime)DateTo };            
            parameters.Add("collection", filteredItems);
            parameters.Add("period", period);
            _showDialog.ShowDialog("UraToXmlDialog", parameters, result =>
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
                SelectedUraPrimke = (BookUraRestModel)item;
                SelectedUraPrimke.Knjizen = false;
            }
        }
        #endregion

        #region Row editing
        private async void EditSelectedRow()
        {
            await _bookUraRestEndpoint.PostRow(SelectedUraPrimke);
        }

        private bool CanEditRow() => SelectedUraPrimke != null;

        public void ResetSelectedItem()
        {
            SelectedUraPrimke = null;
        }
        #endregion
    }
}
