using LoginModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace LoginModule
{
    public class ModuleLogin : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleLogin(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegion region = _regionManager.Regions["ContentRegion"];
            var view = containerProvider.Resolve<LoginView>();
            region.Add(view);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
