using Accounting.MainModule.Dialogs.AreYouSure;
using AccountingUI.Core.Models;
using AccountingUI.Core.Service;
using AccountingUI.Core.Services;
using AccountingUI.Wpf.Dialogs.AccountBalanceCard;
using AccountingUI.Wpf.Dialogs.AccountingProcessing;
using AccountingUI.Wpf.Dialogs.AccountingSettings;
using AccountingUI.Wpf.Dialogs.AccountPairing;
using AccountingUI.Wpf.Dialogs.AccountsSelection;
using AccountingUI.Wpf.Views;
using AssetsModule;
using AutoMapper;
using BankkStatementsModule;
using BookAccountsModule;
using BookIraModule;
using BookJournalModule;
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
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using VATModule;

namespace AccountingUI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("hr-HR");

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            base.OnStartup(e);
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<IApiService, ApiService>();
            containerRegistry.RegisterScoped<IAccountingJournalEndpoint, AccountingJournalEndpoint>();
            containerRegistry.RegisterScoped<IProcessToJournalService, ProcessToJournalService>();

            containerRegistry.RegisterInstance<IConfiguration>(AddConfiguration());
            containerRegistry.RegisterInstance<IMapper>(ConfigureAutomapper());

            containerRegistry.RegisterDialog<AreYouSureView>();
            containerRegistry.RegisterDialog<AccountsLinkDialog>();
            containerRegistry.RegisterDialog<AccountsSelectionDialog>();
            containerRegistry.RegisterDialog<ProcessToJournal>();
            containerRegistry.RegisterDialog<AccountPairDialog>();
            containerRegistry.RegisterDialog<BalanceCardDialog>();
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
            moduleCatalog.AddModule<ModuleBookIra>();
            moduleCatalog.AddModule<ModuleBankReports>();
            moduleCatalog.AddModule<ModuleVAT>();
            moduleCatalog.AddModule<ModuleAssets>();
            moduleCatalog.AddModule<ModuleBookJournal>();
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
