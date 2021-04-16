using Accounting.MainModule.Dialogs.AreYouSure;
using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using AccountingUI.Wpf.Views;
using AutoMapper;
using BookAccountsModule;
using BookUraModule;
using CitiesModule;
using CompanyModule;
using EmployeeModule;
using LoginModule;
using Microsoft.Extensions.Configuration;
using PartnersModule;
using PayrollModule;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.IO;
using System.Windows;

namespace AccountingUI.WPF
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
            containerRegistry.RegisterScoped<IApiService, ApiService>();

            containerRegistry.RegisterInstance<IConfiguration>(AddConfiguration());
            containerRegistry.RegisterInstance<IMapper>(ConfigureAutomapper());

            containerRegistry.RegisterForNavigation<StartView>();

            containerRegistry.RegisterDialog<AreYouSureView, AreYouSureViewModel>();
        }

        private IMapper ConfigureAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PayrollModel, PayrollArchivePayrollModel>();
                cfg.CreateMap<PayrollSupplementEmployeeModel, PayrollArchiveSupplementModel>();
            });

            var mapper = config.CreateMapper();

            return mapper;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleLogin>();
            moduleCatalog.AddModule<ModulePartners>();
            moduleCatalog.AddModule<ModuleCompany>();
            moduleCatalog.AddModule<ModuleEmployee>();
            moduleCatalog.AddModule<ModuleCities>();
            moduleCatalog.AddModule<ModulePayroll>();
            moduleCatalog.AddModule<ModuleBookUra>();
            moduleCatalog.AddModule<ModuleBookAccounts>();
        }

        private IConfiguration AddConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
#if DEBUG
            builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
            builder.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
#endif        
            return builder.Build();
        }
    }
}
