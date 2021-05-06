using AccountingUI.Core.Services;
using BookUraModule.Dialogs;
using BookUraModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace BookUraModule
{
    public class ModuleBookUra : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PrimkeView>();
            containerRegistry.RegisterForNavigation<PrimkeRepro>();
            containerRegistry.RegisterForNavigation<RestView>();
            containerRegistry.RegisterForNavigation<PrimkeDiscounts>();

            containerRegistry.RegisterDialog<CalculationDialog>();
            containerRegistry.RegisterDialog<UraToXmlDialog>();

            containerRegistry.RegisterScoped<IXlsFileReader, XlsFileReader>();
            containerRegistry.RegisterScoped<IBookUraEndpoint, BookUraEndpoint>();
            containerRegistry.RegisterScoped<IBookUraReproEndpoint, BookUraReproEndpoint>();
            containerRegistry.RegisterScoped<IBookUraRestEndpoint, BookUraRestEndpoint>();
            containerRegistry.RegisterScoped<IBookAccountSettingsEndpoint, BookAccountSettingsEndpoint>();
            containerRegistry.RegisterScoped<IAccountPairsEndpoint, AccountPairsEndpoint>();
        }
    }
}
