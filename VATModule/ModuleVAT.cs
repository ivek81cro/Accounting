using AccountingUI.Core.Services;
using Prism.Ioc;
using Prism.Modularity;
using VATModule.Views;

namespace VATModule
{
    public class ModuleVAT : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<VatCalculation>();

            containerRegistry.RegisterScoped<IVatEndpoint, VatEndpoint>();
        }
    }
}
