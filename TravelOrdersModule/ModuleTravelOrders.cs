using Prism.Ioc;
using Prism.Modularity;
using TravelOrdersModule.Dialogs;
using TravelOrdersModule.Views;

namespace TravelOrdersModule
{
    public class ModuleTravelOrders : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<GeneratorDialog>();
            containerRegistry.RegisterDialog<SaveLocoOrderDialog>();

            containerRegistry.RegisterForNavigation<TravelOrdersView>();
            containerRegistry.RegisterForNavigation<LocoOrdersView>();
        }
    }
}
