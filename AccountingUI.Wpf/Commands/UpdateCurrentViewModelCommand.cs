using AccountingUI.Wpf.States.Navigators;
using AccountingUI.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccountingUI.Wpf.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Company:
                        _navigator.CurrentViewModel = new CompanyViewModel();
                        break;
                    case ViewType.Partners:
                        _navigator.CurrentViewModel = new PartnersViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}