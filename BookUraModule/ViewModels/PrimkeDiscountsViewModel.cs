using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
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
    public class PrimkeDiscountsViewModel : ViewModelBase
    {
        private readonly IBookUraRestEndpoint _bookUraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;
        private readonly IAccountPairsEndpoint _accoutPairsEndpoint;
        private readonly IProcessToJournalService _processToJournalService;

        private readonly string _bookName;

        public PrimkeDiscountsViewModel(IXlsFileReader xlsFileReader,
                                        IBookUraRestEndpoint bookUraEndpoint,
                                        IDialogService showDialog,
                                        IBookAccountSettingsEndpoint settingsEndpoint,
                                        IAccountPairsEndpoint accoutPairsEndpoint,
                                        IProcessToJournalService processToJournalService)
        {
            _bookUraEndpoint = bookUraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;
            _accoutPairsEndpoint = accoutPairsEndpoint;
            _processToJournalService = processToJournalService;

            _bookName = "Odobrenja";

            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
            FilterDataCommand = new DelegateCommand(FilterPrimke);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess);
            CalculationsReportCommand = new DelegateCommand(ShowCalculationDialog);
            UnmarkProcessedCommand = new DelegateCommand(UnmarkProcessed, CanUnmark);
        }

        #region Delegate commands
        public DelegateCommand AccountsSettingsCommand { get; private set; }
        public DelegateCommand FilterDataCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        public DelegateCommand CalculationsReportCommand { get; private set; }
        public DelegateCommand UnmarkProcessedCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<BookUraRestModel> _uraRestInvoices;
        public ObservableCollection<BookUraRestModel> UraRestInvoices
        {
            get { return _uraRestInvoices; }
            set
            {
                SetProperty(ref _uraRestInvoices, value);
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
            set { SetProperty(ref _filterName, value); }
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

        private decimal _sumTotal;
        public decimal SumTotal
        {
            get { return _sumTotal; }
            set { SetProperty(ref _sumTotal, value); }
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
            var primke = await _bookUraEndpoint.GetDiscounts();
            UraRestInvoices = new ObservableCollection<BookUraRestModel>(primke);
            FilterPrimke();
            LoadAccountingSettings();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private void DatagridLoaded() => IsLoading = false;
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
                    await _bookUraEndpoint.MarkAsProcessed(SelectedUraPrimke.RedniBroj);
                }
            }
        }

        private async Task SendToProcessingDialog()
        {
            var entries = await CreateJournalEntries();
            var parameters = new DialogParameters();
            parameters.Add("entries", entries);
            parameters.Add("automatic", AutomaticProcess);
            _showDialog.ShowDialog("ProcessToJournal", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedUraPrimke.Knjizen = true;
                    _bookUraEndpoint.MarkAsProcessed(SelectedUraPrimke.RedniBroj);
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
    }
}
