using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

namespace BookUraModule.ViewModels
{
    public class PrimkeViewModel : ViewModelBase
    {
        private readonly IXlsFileReader _xlsFileReader;
        public PrimkeViewModel(IXlsFileReader xlsFileReader)
        {
            _xlsFileReader = xlsFileReader;

            LoadDataCommand = new DelegateCommand(LoadDataFromFile);
        }

        public DelegateCommand LoadDataCommand { get; private set; }

        private ObservableCollection<BookUraPrimkaModel> _uraPrimke;
        public ObservableCollection<BookUraPrimkaModel> UraPrimke
        {
            get { return _uraPrimke; }
            set { SetProperty(ref _uraPrimke, value); }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        public void LoadPrimke()
        {

        }

        private void LoadDataFromFile()
        {
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
                }
            }
        }

        private void FromStringToList(DataSet data)
        {
            UraPrimke = new ObservableCollection<BookUraPrimkaModel>();
            foreach (DataRow row in data.Tables[0].Rows)
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
                RedniBroj = int.Parse(val[0].ToString()),
                DatumKnjizenja = DateTime.ParseExact(val[1].ToString().Split(' ')[0], "dd.MM.yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                BrojPrimke = int.Parse(val[2].ToString()),
                Storno = val[3].ToString() == "*",
                MaloprodajnaVrijednost = decimal.Parse(val[4].ToString()),
                NazivDobavljaca = val[5].ToString(),
                BrojRacuna = val[6].ToString(),
                DatumRacuna = DateTime.ParseExact(val[7].ToString().Split(' ')[0], "dd.MM.yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                Otpremnica = val[8].ToString() == "DA",
                DospijecePlacanja = DateTime.ParseExact(val[9].ToString().Split(' ')[0], "dd.MM.yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
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
    }
}
