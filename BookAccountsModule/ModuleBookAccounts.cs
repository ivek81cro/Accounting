using AccountingUI.Core.Services;
using BookAccountsModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace BookAccountsModule
{
    public class ModuleBookAccounts : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AccountsView>();

            containerRegistry.RegisterScoped<IBookAccountsEndpoint, BookAccountsEndpoint>();
            containerRegistry.RegisterScoped<IBookAccountSettingsEndpoint, BookAccountSettingsEndpoint>();
        }
    }
}
