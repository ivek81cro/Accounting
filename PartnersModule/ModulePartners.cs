using PartnersModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PartnersModule
{
    public class ModulePartners : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(PartnersView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PartnerDetails>();
        }
    }
}
