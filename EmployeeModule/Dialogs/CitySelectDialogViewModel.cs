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

namespace EmployeeModule.Dialogs
{
    public class CitySelectDialogViewModel : BindableBase, IDialogAware
    {
        private ICityEndpoint _cityEndpoint;

        public CitySelectDialogViewModel(ICityEndpoint cityEndpoint)
        {
            _cityEndpoint = cityEndpoint;
            SelectAndCloseCommand = new DelegateCommand(CloseDialog);
        }

        public DelegateCommand SelectAndCloseCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;
        public string Title => "Odabir mjesta stanovanja";

        private ObservableCollection<CityModel> _cities;
        public ObservableCollection<CityModel> Cities
        {
            get { return _cities; }
            set { SetProperty(ref _cities, value); }
        }

        private CityModel _selectedCity;
        public CityModel SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                SetProperty(ref _selectedCity, value);
            }
        }

        private ICollectionView _citiesView;
        private string _filterCities;
        public string FilterCities
        {
            get { return _filterCities; }
            set
            {
                SetProperty(ref _filterCities, value);
                _citiesView.Refresh();
            }
        }

        public async Task LoadCities()
        {
            var citiesList = await _cityEndpoint.GetAll();
            Cities = new ObservableCollection<CityModel>(citiesList);
            _citiesView = CollectionViewSource.GetDefaultView(Cities);
            _citiesView.Filter = o => string.IsNullOrEmpty(FilterCities) ? 
                true : ((CityModel)o).Mjesto.ToLower().Contains(FilterCities.ToLower());
        }

        private void CloseDialog()
        {
            if(SelectedCity != null)
            {
                var result = ButtonResult.OK;
                var p = new DialogParameters();
                p.Add("city", SelectedCity);

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
            await LoadCities();
        }
    }
}
