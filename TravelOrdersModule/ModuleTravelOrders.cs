using Prism.Ioc;
using Prism.Modularity;
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
            containerRegistry.RegisterForNavigation<TravelOrdersView>();
            containerRegistry.RegisterForNavigation<LocoOrdersView>();
        }
    }
}
