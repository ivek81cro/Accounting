using AccountingUI.Core.Models;
using LoginModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace LoginModule
{
    public class ModuleLogin : IModule
    {
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
