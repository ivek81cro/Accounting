using AccountingUI.Core.Services;
using PayrollModule.Dialogs;
using PayrollModule.ServiceLocal;
using PayrollModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace PayrollModule
{
    public class ModulePayroll : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PayrollView>();
            containerRegistry.RegisterForNavigation<PayrollProcessing>();

            containerRegistry.RegisterDialog<PayrollCalculationDialog>();
            containerRegistry.RegisterDialog<SupplementsDialog>();

            containerRegistry.RegisterScoped<IPayrollEndpoint, PayrollEndpoint>();
            containerRegistry.RegisterScoped<IPayrollSupplementEndpoint, PayrollSupplementEndpoint>();
            containerRegistry.RegisterScoped<IPayrollSupplementEmployeeEndpoint, PayrollSupplementEmployeeEndpoint>();
            containerRegistry.RegisterScoped<IPayrollCalculation, PayrollCalculation>();
        }
    }
}
