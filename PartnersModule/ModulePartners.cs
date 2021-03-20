using PartnersModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace PartnersModule
{
    public class ModulePartners : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PartnersView>();
        }
    }
}
