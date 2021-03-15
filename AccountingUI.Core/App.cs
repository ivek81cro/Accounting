using AccountingUI.Core.Helpers;
using AccountingUI.Core.Models;
using AccountingUI.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;

namespace AccountingUI.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<IApiHelper, ApiHelper>();
            Mvx.IoCProvider.RegisterSingleton<ILoggedInUserModel>(() => new LoggedInUserModel());

            RegisterAppStart<LoginViewModel>();
        }
    }
}
