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
    public class RetailViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;
        private readonly IBookRetailEndpoint _bookRetailEndpoint;
        private readonly IProcessToJournalService _processToJournalService;
        
        private readonly string _bookName; 
        private int _maxRedniBroj;
        private bool _loaded;

        public RetailViewModel(IXlsFileReader xlsFileReader,
                               IDialogService showDialog,
                               IBookAccountSettingsEndpoint settingsEndpoint,
                               IBookRetailEndpoint bookRetailEndpoint,
                               IProcessToJournalService processToJournalService)
        {
            _xlsFileReader = xlsFileReader;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;
            _processToJournalService = processToJournalService;
            _bookRetailEndpoint = bookRetailEndpoint;

            _bookName = "Maloprodaja";

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSaveItems);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
            FilterDataCommand = new DelegateCommand(FilterPrimke);
            ProcessItemCommand = new DelegateCommand(ProcessItem, CanProcess);
            UnmarkProcessedCommand = new DelegateCommand(UnmarkProcessed, CanUnmark);

            LoadRetail();
        }

        #region Delegate commands
        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }
        public DelegateCommand FilterDataCommand { get; private set; }
        public DelegateCommand ProcessItemCommand { get; private set; }
        public DelegateCommand UnmarkProcessedCommand { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<RetailIraModel> _retiailItems;
        public ObservableCollection<RetailIraModel> RetailItems
        {
            get { return _retiailItems; }
            set
            {
                SetProperty(ref _retiailItems, value);
                SaveDataCommand.RaiseCanExecuteChanged();
            }
        }

        private RetailIraModel _selectedItem;
        public RetailIraModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
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
        public async void LoadRetail()
        {
            IsLoading = true;
            var primke = await _bookRetailEndpoint.GetAll();
            RetailItems = new ObservableCollection<RetailIraModel>(primke);
            FilterPrimke();
            LoadAccountingSettings();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }

        private void DatagridLoaded() => IsLoading = false;
        #endregion

        #region Load data from file
        private async void LoadDataFromFile()
        {
            _maxRedniBroj = RetailItems.Count > 0 ? RetailItems.Max(y => y.RedniBroj) : 0;

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
                var data = _xlsFileReader.Convert(FilePath, "REKAPITULACIJA POREZA");
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
            RetailItems = new ObservableCollection<RetailIraModel>();
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
            RetailItems.Add(new RetailIraModel
            {
                RedniBroj = int.Parse(val[0].ToString()),
                Datum = DateTime.Parse(val[1].ToString()),
                Stopa = decimal.Parse(val[3].ToString()),
                NaplacenaVrijednost = decimal.Parse(val[4].ToString()),
                PoreznaOsnovica = decimal.Parse(val[5].ToString()),
                NettoRuc = decimal.Parse(val[6].ToString()),
                Pdv = decimal.Parse(val[8].ToString()),
                NabavnaVrijednost = decimal.Parse(val[9].ToString()),
                StornoMarze = decimal.Parse(val[10].ToString()),
                StornoPdv = decimal.Parse(val[11].ToString()),
                MaloprodajnaVrijednost = decimal.Parse(val[12].ToString())
            });
        }
        #endregion

        #region Save to database
        private bool CanSaveItems()
        {
            return RetailItems != null && _loaded;
        }

        private async void SaveToDatabase()
        {
            IEnumerable<RetailIraModel> primke = RetailItems.Where(x => x.RedniBroj > _maxRedniBroj);
            var list = new List<RetailIraModel>(primke);

            IsLoading = true;
            await _bookRetailEndpoint.Post(list);

            _loaded = false;
            LoadRetail();
        }
        #endregion

        #region Filtering datagrid
        private void FilterPrimke()
        {
            _filteredView = CollectionViewSource.GetDefaultView(RetailItems);
            _filteredView.Filter = o => FilterData((RetailIraModel)o);
        }

        private bool FilterData(RetailIraModel o)
        {
            if (FilterName == null && (DateFrom != null || DateTo != null))
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
            var list = new List<string>() {"Naplaćena vrijednost", "Porezna osnovica", "Netto RUC", "PDV", 
                "Nabavna vrijednost", "Storno marže", "Storno PDV-a", "Maloprodajna vrijednost"};
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

        #region Book item processing
        private Dictionary<string, decimal> MapColumnToPropertyValue()
        {
            var iraItem = SelectedItem;
            var item = new Dictionary<string, decimal>();
            item.Add("Naplaćena vrijednost", iraItem.NaplacenaVrijednost);
            item.Add("Porezna osnovica", iraItem.PoreznaOsnovica);
            item.Add("Netto RUC", iraItem.NettoRuc);
            item.Add("PDV", iraItem.Pdv);
            item.Add("Nabavna vrijednost", iraItem.NabavnaVrijednost);
            item.Add("Storno marže", iraItem.StornoMarze);
            item.Add("Storno PDV-a", iraItem.StornoPdv);
            item.Add("Maloprodajna vrijednost", iraItem.MaloprodajnaVrijednost);

            return item;
        }

        private List<AccountingJournalModel> CreateJournalEntries()
        {

            var mappings = MapColumnToPropertyValue();
            var entry = SelectedItem;
            var entries = new List<AccountingJournalModel>();
            foreach (var setting in AccountingSettings)
            {
                var value = mappings.GetValueOrDefault(setting.Name);
                value *= setting.Prefix ? (-1) : 1;
                if (value != 0)
                {
                    entries.Add(new AccountingJournalModel
                    {
                        Broj = entry.RedniBroj,
                        Dokument = "Rasknjižavanje maloprodaja: " + entry.Datum?.ToString("dd.MM.yyyy"),
                        Datum = entry.Datum,
                        Opis = setting.Name,
                        Konto = setting.Account,
                        Dugovna = setting.Side == "Dugovna" ? value : 0,
                        Potrazna = setting.Side == "Potražna" ? value : 0,
                        Valuta = "HRK",
                        VrstaTemeljnice = _bookName
                    });
                }
            }
            return entries;
        }

        private bool CanProcess()
        {
            return SelectedItem != null && !SelectedItem.Knjizen;
        }

        private async void ProcessItem()
        {
            if (AutomaticProcess)
            {
                await ProcessToJournalAutomatic();
            }
            else
            {
                SendToProcessingDialog();
            }
        }

        private async Task ProcessToJournalAutomatic()
        {
            foreach (var item in _filteredView)
            {
                SelectedItem = (RetailIraModel)item;
                var entries = CreateJournalEntries();
                if (!await _processToJournalService.ProcessEntries(entries))
                {
                    AutomaticProcess = false;
                    SendToProcessingDialog();
                    break;
                }
                else
                {
                    SelectedItem.Knjizen = true;
                    await _bookRetailEndpoint.MarkAsProcessed(SelectedItem.RedniBroj);
                }
            }
        }

        private void SendToProcessingDialog()
        {

            var entries = CreateJournalEntries();
            var parameters = new DialogParameters();
            parameters.Add("entries", entries);
            _showDialog.ShowDialog("ProcessToJournal", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedItem.Knjizen = true;
                    _bookRetailEndpoint.MarkAsProcessed(SelectedItem.RedniBroj);
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
                SelectedItem = (RetailIraModel)item;
                SelectedItem.Knjizen = false;
            }
        }
        #endregion
    }
}
