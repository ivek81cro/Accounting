using AccountingUI.Core.ViewModels;
using MvvmCross.ViewModels;

namespace AccountingUI.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<LoginViewModel>();
        }
    }
}
