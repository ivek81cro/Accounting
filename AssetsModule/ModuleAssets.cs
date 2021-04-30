using AssetsModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace AssetsModule
{
    public class ModuleAssets : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AssetsFixedView>();
            containerRegistry.RegisterForNavigation<AssetsCurrentView>();
        }
    }
}
