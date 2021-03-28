using AccountingUI.Core.Services;
using CitiesModule.Dialogs;
using CitiesModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace CitiesModule
{
    public class ModuleCities : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CitiesView>();

            containerRegistry.RegisterDialog<CityEdit, CityEditViewModel>();

            containerRegistry.RegisterScoped<ICityEndpoint, CityEndpoint>();
        }
    }
}
