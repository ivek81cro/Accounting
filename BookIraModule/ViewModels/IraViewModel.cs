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

namespace BookIraModule.ViewModels
{
    public class IraViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookIraEndpoint _bookIraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;

        private readonly string _bookName;

        private bool _loaded = false;
        private int _maxRedniBroj;

        public IraViewModel(IXlsFileReader xlsFileReader, 
            IBookIraEndpoint bookIraEndpoint, IDialogService showDialog, 
            IBookAccountSettingsEndpoint settingsEndpoint)
        {
            _xlsFileReader = xlsFileReader;
            _bookIraEndpoint = bookIraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;

            _bookName = "Knjiga izlaznih računa";

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSaveItems);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
        }

        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }

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

        public async void LoadIra()
        {
            StatusMessage = "Učitavam podatke iz baze...";
            var primke = await _bookIraEndpoint.GetAll();
            StatusMessage = "";
            IraItems = new ObservableCollection<BookIraModel>(primke);

            LoadAccountingSettings();
        }

        private void LoadDataFromFile()
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

        private bool CanSaveItems()
        {
            return IraItems != null && _loaded;
        }

        private async void SaveToDatabase()
        {
            IEnumerable<BookIraModel> primke = IraItems.Where(x => x.RedniBroj > _maxRedniBroj);
            var list = new List<BookIraModel>(primke);

            StatusMessage = "Zapisujem u bazu podataka...";
            await _bookIraEndpoint.PostPrimke(list);
            StatusMessage = ""; ;

            _loaded = false;
            LoadIra();
        }

        private Dictionary<string, decimal> MapColumnToPropertyValue(BookIraModel iraItem)
        {
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
    }
}
