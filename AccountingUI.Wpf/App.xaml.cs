using AccountingUI.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AccountingUI.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window wnd = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            wnd.Show();
        
            base.OnStartup(e);
        }
    }
}
