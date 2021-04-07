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
            containerRegistry.RegisterForNavigation<ArchiveView>();

            containerRegistry.RegisterDialog<PayrollCalculationDialog>();
            containerRegistry.RegisterDialog<SupplementsDialog>();
            containerRegistry.RegisterDialog<JoppdDialog>();

            containerRegistry.RegisterScoped<IPayrollEndpoint, PayrollEndpoint>();
            containerRegistry.RegisterScoped<IPayrollSupplementEndpoint, PayrollSupplementEndpoint>();
            containerRegistry.RegisterScoped<IPayrollSupplementEmployeeEndpoint, PayrollSupplementEmployeeEndpoint>();
            containerRegistry.RegisterScoped<IJoppdEmployeeEndpoint, JoppdEmployeeEndpoint>();
            containerRegistry.RegisterScoped<IPayrollCalculation, PayrollCalculation>();
            containerRegistry.RegisterScoped<IPayrollArchiveEndpoint, PayrollArchiveEndpoint>();
            containerRegistry.RegisterScoped<IPayrollArchivePrepare, PayrollArchivePrepare>();
        }
    }
}
