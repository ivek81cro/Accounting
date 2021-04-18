using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace BookUraModule.ViewModels
{
    public class RestViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookUraRestEndpoint _bookUraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;

        private readonly string _bookName;

        private bool _loaded = false;
        private int _maxPrimka;

        public RestViewModel(IXlsFileReader xlsFileReader, 
            IBookUraRestEndpoint bookUraEndpoint, IDialogService showDialog, 
            IBookAccountSettingsEndpoint settingsEndpoint)
        {
            _xlsFileReader = xlsFileReader;
            _bookUraEndpoint = bookUraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;

            _bookName = "Knjiga ulaznih računa";

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSavePrimke);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
        }

        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }

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

        public async void LoadPrimke()
        {
            StatusMessage = "Učitavam podatke iz baze...";
            var primke = await _bookUraEndpoint.GetAll();
            StatusMessage = "";
            UraRestInvoices = new ObservableCollection<BookUraRestModel>(primke);

            LoadAccountingSettings();
        }

        private void LoadDataFromFile()
        {
            _maxPrimka = UraRestInvoices.Count > 0 ? UraRestInvoices.Max(y => y.RedniBroj) : 0;

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

        private bool CanSavePrimke()
        {
            return UraRestInvoices != null && _loaded;
        }

        private async void SaveToDatabase()
        {
            IEnumerable<BookUraRestModel> primke = UraRestInvoices.Where(x => x.RedniBroj > _maxPrimka);
            var list = new List<BookUraRestModel>(primke);

            StatusMessage = "Zapisujem u bazu podataka...";
            await _bookUraEndpoint.PostPrimke(list);
            StatusMessage = ""; ;

            _loaded = false;
            LoadPrimke();
        }

        private Dictionary<string, decimal> MapColumnToPropertyValue(BookUraRestModel primka)
        {
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
    }
}
