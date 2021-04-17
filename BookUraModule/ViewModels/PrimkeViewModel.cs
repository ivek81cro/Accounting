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

namespace BookUraModule.ViewModels
{
    public class PrimkeViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookUraEndpoint _bookUraEndpoint;
        private readonly IDialogService _showDialog;
        private readonly IBookAccountSettingsEndpoint _settingsEndpoint;

        private bool _loaded = false;
        private int _maxPrimka;

        public PrimkeViewModel(IXlsFileReader xlsFileReader, IBookUraEndpoint bookUraEndpoint,
            IDialogService showDialog, IBookAccountSettingsEndpoint settingsEndpoint)
        {
            _xlsFileReader = xlsFileReader;
            _bookUraEndpoint = bookUraEndpoint;
            _showDialog = showDialog;
            _settingsEndpoint = settingsEndpoint;

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSavePrimke);
            AccountsSettingsCommand = new DelegateCommand(OpenAccountsSettings);
        }

        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand AccountsSettingsCommand { get; private set; }

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
            UraPrimke = new ObservableCollection<BookUraPrimkaModel>(primke);

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
                var data = _xlsFileReader.Convert(FilePath, "Primke");
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

        private Dictionary<string, decimal> MapColumnToPropertyValue(BookUraPrimkaModel primka)
        {
            var item = new Dictionary<string, decimal>();
            item.Add("Id", primka.Id);
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

        private void OpenAccountsSettings()
        {
            var list = new List<string>() {"Maloprodajna vrijednost", "Fakturna vrijednost", "Maloprodajna marža", "Iznos PDV-a",
            "Vrijednost bez poreza", "Nabavna vrijednost", "Maloprodajni rabat", "NettoNabavna vrijednost", "Pretporez", "Veleprodajni rabat",
            "Cassa sconto", "Netto ruc", "Povratna naknada"};
            var bookName = "Primke";
            var parameters = new DialogParameters();
            parameters.Add("columnsList", list);
            parameters.Add("bookName", bookName);
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
            AccountingSettings = await _settingsEndpoint.GetByBookName("Primke");
        }
    }
}
