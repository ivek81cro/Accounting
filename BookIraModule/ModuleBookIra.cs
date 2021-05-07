using AccountingUI.Core.Services;
using BookIraModule.Dialogs;
using BookIraModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace BookIraModule
{
    public class ModuleBookIra : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<IraView>();

            containerRegistry.RegisterScoped<IXlsFileReader, XlsFileReader>();
            containerRegistry.RegisterScoped<IBookIraEndpoint, BookIraEndpoint>();

            containerRegistry.RegisterDialog<CalculationDialog>("IraCalculation");
        }
    }
}
