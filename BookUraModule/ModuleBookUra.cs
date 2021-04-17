using AccountingUI.Core.Services;
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

            containerRegistry.RegisterScoped<IXlsFileReader, XlsFileReader>();
            containerRegistry.RegisterScoped<IBookUraEndpoint, BookUraEndpoint>();
            containerRegistry.RegisterScoped<IBookUraReproEndpoint, BookUraReproEndpoint>();
        }
    }
}
