using AccountingUI.Core.Models;
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
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILoggedInUserModel, LoggedInUserModel>();
            containerRegistry.RegisterDialog<LoginView>();
        }
    }
}
