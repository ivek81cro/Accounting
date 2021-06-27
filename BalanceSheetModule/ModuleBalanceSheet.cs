using AccountingUI.Core.Services;
using BalanceSheetModule.Dialogs;
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

            containerRegistry.RegisterDialog<ReportDialog>();

            containerRegistry.RegisterScoped<IBalanceSheetEndpoint, BalanceSheetEndpoint>();
        }
    }
}
