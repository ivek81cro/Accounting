using AccountingUI.Core.Events;
using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Microsoft.Win32;
using Prism.Commands;
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
        private readonly BackgroundWorker _worker;

        private bool _loaded = false;
        private int _maxPrimka;

        public PrimkeViewModel(IXlsFileReader xlsFileReader, IBookUraEndpoint bookUraEndpoint)
        {
            _xlsFileReader = xlsFileReader;
            _bookUraEndpoint = bookUraEndpoint;

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSavePrimke);
        }

        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }

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

        public async void LoadPrimke()
        {
            StatusMessage = "Učitavam podatke iz baze...";
            var primke = await _bookUraEndpoint.GetAll();
            StatusMessage = "";
            UraPrimke = new ObservableCollection<BookUraPrimkaModel>(primke);
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
    }
}
