using AccountingUI.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccountingUI.Wpf.States.Navigators
{
    public enum ViewType
    {
        Company,
        Partners,
        Employees,
        TaxRates,
        Cities,
        StockInvoice,
        MaterialsInvoice,
        UndefinedInvoice,
        SalesInvoice
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}
