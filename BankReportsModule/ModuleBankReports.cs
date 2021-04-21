using BankkStatementsModule.Dialogs;
using BankkStatementsModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace BankkStatementsModule
{
    public class ModuleBankReports : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BankStatementView>();

            containerRegistry.RegisterDialog<IndividualReportDialog>();
        }
    }
}
