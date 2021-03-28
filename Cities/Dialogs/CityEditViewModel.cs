using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace CitiesModule.Dialogs
{
    class CityEditViewModel : BindableBase, IDialogAware
    {
        private ICityEndpoint _cityEndpoint;

        public CityEditViewModel(ICityEndpoint cityEndpoint)
        {
            _cityEndpoint = cityEndpoint;

            SaveCityCommand = new DelegateCommand(CloseDialog);
        }

        public DelegateCommand SaveCityCommand { get; }

        public string Title => "Izmjena podataka";

        public event Action<IDialogResult> RequestClose;

        private CityModel _city;
        public CityModel City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        private void CloseDialog()
        {
            if (City != null && !City.HasErrors)
            {
                _cityEndpoint.PostCity(City);
                var result = ButtonResult.OK;
                var p = new DialogParameters();
                p.Add("city", City);

                RequestClose?.Invoke(new DialogResult(result, p));
            }
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            City = parameters.GetValue<CityModel>("city");
            if (City != null)
            {
                GetCitiesFromDatabase(City.Id);
            }
            else
            {
                City = new CityModel();
                City.Reset();
            }
        }

        private async void GetCitiesFromDatabase(int id)
        {
            City = await _cityEndpoint.GetById(id);
        }
    }
}
