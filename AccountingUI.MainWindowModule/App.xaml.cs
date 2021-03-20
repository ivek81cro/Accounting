using AccountingUI.Core.Helpers;
using AccountingUI.MainWindowModule.Helpers;
using AccountingUI.MainWindowModule.ViewModels;
using AccountingUI.MainWindowModule.Views;
using DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace AccountingUI.MainWindowModule
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApiHelper, ApiHelper>();
            containerRegistry.RegisterDialog<LoginWindow, LoginWindowViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {

        }
    }
}
