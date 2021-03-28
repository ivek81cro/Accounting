using AccountingUI.Core.Services;
using EmployeeModule.Dialogs;
using EmployeeModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace EmployeeModule
{
    public class ModuleEmployee : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<EmployeesView>();

            containerRegistry.RegisterDialog<EmployeeEdit, EmployeeEditViewModel>();
            containerRegistry.RegisterDialog<CitySelectDialog, CitySelectDialogViewModel>();

            containerRegistry.RegisterScoped<IEmployeeEndpoint, EmployeeEndpoint>();
        }
    }
}
