using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CitiesModule.Dialogs
{
    class CountySelectDialogViewModel : BindableBase, IDialogAware
    {
        private ICountyEndpoint _countyEndpoint;

        public CountySelectDialogViewModel(ICountyEndpoint countyEndpoint)
        {
            _countyEndpoint = countyEndpoint;
            SelectAndCloseCommand = new DelegateCommand(CloseDialog);
        }

        public DelegateCommand SelectAndCloseCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;
        public string Title => "Odabir županije";

        private ObservableCollection<CountyModel> _counties;
        public ObservableCollection<CountyModel> Counties
        {
            get { return _counties; }
            set { SetProperty(ref _counties, value); }
        }

        private CountyModel _selectedCounty;
        public CountyModel SelectedCounty
        {
            get { return _selectedCounty; }
            set
            {
                SetProperty(ref _selectedCounty, value);
            }
        }

        private ICollectionView _countiesView;
        private string _filterCounties;
        public string FilterCounties
        {
            get { return _filterCounties; }
            set
            {
                SetProperty(ref _filterCounties, value);
                _countiesView.Refresh();
            }
        }

        public async Task LoadCounties()
        {
            var countiesList = await _countyEndpoint.GetAll();
            Counties = new ObservableCollection<CountyModel>(countiesList);
            _countiesView = CollectionViewSource.GetDefaultView(Counties);
            _countiesView.Filter = o => string.IsNullOrEmpty(FilterCounties) ? true : ((CountyModel)o).Naziv.Contains(FilterCounties);
        }

        private void CloseDialog()
        {
            if (SelectedCounty != null)
            {
                var result = ButtonResult.OK;
                var p = new DialogParameters();
                p.Add("county", SelectedCounty);

                RequestClose?.Invoke(new DialogResult(result, p));
            }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            await LoadCounties();
        }
    }
}
