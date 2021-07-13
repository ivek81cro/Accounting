using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace BookIraModule.Dialogs
{
    public class HzzoPaymentsDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IBookIraHzzoEndpoint _bookIraHzzoEndpoint;
        private readonly IXlsFileReader _xlsFileReader;
        private readonly IBookIraEndpoint _bookIraEndpoint;

        public HzzoPaymentsDialogViewModel(IXlsFileReader xlsFileReader,
                                           IBookIraHzzoEndpoint bookIraHzzoEndpoint,
                                           IBookIraEndpoint bookIraEndpoint)
        {
            _xlsFileReader = xlsFileReader;
            _bookIraHzzoEndpoint = bookIraHzzoEndpoint;
            _bookIraEndpoint = bookIraEndpoint;

            FilterDataCommand = new DelegateCommand(FilterData);
            LoadDataCommand = new DelegateCommand(LoadDataFromFileAsync);
            SaveDataCommand = new DelegateCommand(SaveToDatabase, CanSaveItems);
            ConnectPaymentCommand = new DelegateCommand(ConnectPaymentsToInvoice);
        }

        public DelegateCommand FilterDataCommand { get; private set; }
        public DelegateCommand LoadDataCommand { get; private set; }
        public DelegateCommand SaveDataCommand { get; private set; }
        public DelegateCommand ConnectPaymentCommand { get; private set; }

        public string Title => "Evidencija i povezivanje uplata HZZO-a";

        public event Action<IDialogResult> RequestClose;

        #region Properties
        private ObservableCollection<BookIraHzzoModel> _payments;
        public ObservableCollection<BookIraHzzoModel> Payments
        {
            get { return _payments; }
            set { SetProperty(ref _payments, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        private ICollectionView _filterView;
        private string _filterKonto;
        public string FilterKonto
        {
            get { return _filterKonto; }
            set
            {
                SetProperty(ref _filterKonto, value);
                _filterView.Refresh();
            }
        }

        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set
            {
                SetProperty(ref _dateFrom, value);
            }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set
            {
                SetProperty(ref _dateTo, value);
            }
        }
        #endregion

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            LoadDataFromDatabase();
        }

        private void DatagridLoaded() => IsLoading = false;

        #region Load data from database
        private async void LoadDataFromDatabase()
        {
            IsLoading = true;

            var primke = await _bookIraHzzoEndpoint.GetAll();
            Payments = new ObservableCollection<BookIraHzzoModel>(primke);
            FilterData();

            await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
        }
        #endregion

        #region Load data from file
        private async void LoadDataFromFileAsync()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Xlsx Files *.xlsx|*.xlsx|Xls Files *.xls|*.xls|Csv files *.csv|*.csv",
                FilterIndex = 1,
                Multiselect = false
            };

            Nullable<bool> result = ofd.ShowDialog();
            string filepath;
            if (result != null && result == true)
            {
                IsLoading = true;
                filepath = ofd.FileName;
                var data = _xlsFileReader.Convert(filepath, "Hrvatski zavod");
                if (data != null)
                {
                    FromStringToList(data);
                }
                await Application.Current.Dispatcher.BeginInvoke(new Action(DatagridLoaded), DispatcherPriority.ContextIdle, null);
            }
        }

        private void FromStringToList(DataSet data)
        {
            Payments = new ObservableCollection<BookIraHzzoModel>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                if (!DateTime.TryParse(row[0].ToString(), out _))
                {
                    continue;
                }
                AddDataToList(row);
            }
        }

        private void AddDataToList(DataRow val)
        {
            Payments.Add(new BookIraHzzoModel
            {
                DatumPlacanja = DateTime.Parse(val[0].ToString()),
                Dokument = val[1].ToString(),
                OriginalniBroj = val[2].ToString(),
                DatumDokumenta = DateTime.Parse(val[3].ToString()),
                Program = val[4].ToString(),
                Opis = val[5].ToString(),
                IznosRacuna = decimal.Parse(val[6].ToString()),
                PlaceniIznos = decimal.Parse(val[7].ToString())
            });
        }
        #endregion

        #region Filtering datagrid
        private void FilterData()
        {
            _filterView = CollectionViewSource.GetDefaultView(Payments);
            _filterView.Filter = o => FilterData((BookIraHzzoModel)o);
        }

        private bool FilterData(BookIraHzzoModel o)
        {
            if (DateFrom != null && DateTo != null)
            {
                return o.DatumPlacanja >= DateFrom && o.DatumPlacanja <= DateTo;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Save data to database
        private void SaveToDatabase()
        {
            _bookIraHzzoEndpoint.PostPayments(Payments.ToList());
        }

        private bool CanSaveItems()
        {
            return true;
        }
        #endregion

        #region Connect payments to invoice
        private async void ConnectPaymentsToInvoice()
        {
            List<BookIraHzzoModel> unprocessed = Payments.Where(x => x.Povezan == false && !x.Program.StartsWith("10014")).ToList();
            foreach(var item in unprocessed)
            {
                await _bookIraEndpoint.UpdateInvoice(item);
            }
        }
        #endregion
    }
}
