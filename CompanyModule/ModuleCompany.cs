using CompanyModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace CompanyModule
{
    public class ModuleCompany : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CompanyView>();
        }
    }
}
