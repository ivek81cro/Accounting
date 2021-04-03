using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using AccountingUI.Core.TabControlRegion;
using CitiesModule.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace CitiesModule.ViewModels
{
    public class CitiesViewModel : ViewModelBase
    {
        private ICityEndpoint _cityEndpoint;
        private IRegionManager _regionManager;
        private IDialogService _showDialog;

        public CitiesViewModel(ICityEndpoint cityEndpoint, IRegionManager regionManager, IDialogService showDialog)
        {
            _cityEndpoint = cityEndpoint;
            _regionManager = regionManager;
            _showDialog = showDialog;

            NewCityCommand = new DelegateCommand(AddNewCity);
            EditCityCommand = new DelegateCommand(EditCity);
            DeleteCityCommand = new DelegateCommand(DeleteCity, CanDelete);
        }

        public DelegateCommand NewCityCommand { get; private set; }
        public DelegateCommand EditCityCommand { get; private set; }
        public DelegateCommand DeleteCityCommand { get; private set; }

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
                DeleteCityCommand.RaiseCanExecuteChanged();
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

        public async void LoadCities()
        {
            var citiesList = await _cityEndpoint.GetAll();
            Cities = new ObservableCollection<CityModel>(citiesList);
            _citiesView = CollectionViewSource.GetDefaultView(Cities);
            _citiesView.Filter = o => string.IsNullOrEmpty(FilterCities) ? 
                true : ((CityModel)o).Mjesto.ToLower().Contains(FilterCities.ToLower());
        }

        private void DeleteCity()
        {
            _showDialog.ShowDialog("AreYouSureView", null, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    _cityEndpoint.DeleteCity(SelectedCity.Id);
                    _cities.Remove(SelectedCity);
                }
            });
        }

        private bool CanDelete()
        {
            return SelectedCity != null;
        }

        private void EditCity()
        {
            SaveCityToDatabase();
        }

        private void AddNewCity()
        {
            if (SelectedCity != null)
            {
                SelectedCity = null;
            }
            SaveCityToDatabase();
        }

        private void SaveCityToDatabase()
        {
            var parameters = new DialogParameters();
            parameters.Add("city", SelectedCity);
            _showDialog.ShowDialog(nameof(CityEdit), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    CityModel city = result.Parameters.GetValue<CityModel>("city");
                    _cityEndpoint.PostCity(city);
                    LoadCities();
                }
            });
        }
    }
}
