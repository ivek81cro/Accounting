using AccountingUI.Core.Helpers;
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

            RegisterAppStart<LoginViewModel>();
        }
    }
}
