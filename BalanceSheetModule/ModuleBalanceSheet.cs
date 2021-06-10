using AccountingUI.Core.Services;
using BalanceSheetModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace BalanceSheetModule
{
    public class ModuleBalanceSheet : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BalanceView>();

            containerRegistry.RegisterScoped<IBalanceSheetEndpoint, BalanceSheetEndpoint>();
        }
    }
}
