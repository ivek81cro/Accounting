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
    public class PrimkeReproViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookUraReproEndpoint _bookUraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;

        private readonly string _bookName;

        private bool _loaded = false;
        private int _maxPrimka;

        public PrimkeReproViewModel(IXlsFileReader xlsFileReader, 
            IBookUraReproEndpoint bookUraEndpoint, IDialogService showDialog, 
            IBookAccountSettingsEndpoint settingsEndpoint)
        {
            _xlsFileReader = xlsFileReader;
            _bookUraEndpoint = bookUraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;
            _bookName = "Primke repromaterijala";

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSavePrimke);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
        }

        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }

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

        public async void LoadPrimke()
        {
            StatusMessage = "Učitavam podatke iz baze...";
            var primke = await _bookUraEndpoint.GetAll();
            StatusMessage = "";
            UraPrimke = new ObservableCollection<BookUraPrimkaReproModel>(primke);

            LoadAccountingSettings();
        }

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

        private Dictionary<string, decimal> MapColumnToPropertyValue(BookUraPrimkaReproModel primka)
        {
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
    }
}
