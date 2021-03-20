using Prism.Mvvm;
using System;

namespace AccountingUI.Wpf.ViewModels
{
    public class StartViewModel : BindableBase
    {
        private string _todayDate = "Današnji datum: " + DateTime.Today.ToString("dd.MM.yyyy");
        public string TodayDate
        {
            get { return _todayDate; }
            set { SetProperty(ref _todayDate, value); }
        }
    }
}
