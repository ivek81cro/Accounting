using AccountingUI.Core.Services;
using BookAccountsModule.Dialogs;
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

            containerRegistry.RegisterDialog<AddEditDialog>();

            containerRegistry.RegisterScoped<IBookAccountsEndpoint, BookAccountsEndpoint>();
            containerRegistry.RegisterScoped<IBookAccountSettingsEndpoint, BookAccountSettingsEndpoint>();
        }
    }
}
