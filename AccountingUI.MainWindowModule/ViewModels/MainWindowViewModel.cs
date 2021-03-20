using AccountingUI.MainWindowModule.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace AccountingUI.MainWindowModule.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IDialogService _dialogService;

        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            InitalizeLoginDialog();
        }

        private void InitalizeLoginDialog()
        {
            _dialogService.ShowDialog("LoginWindow");
        }

        public DelegateCommand<string> ClickCommand { get; private set; }
        public MainWindowViewModel()
        {
            ClickCommand = new DelegateCommand<string>(Click);
        }

        private void Click(string menuItem)
        {
            switch (Enum.Parse<MenuItems>(menuItem))
            {
                case MenuItems.Komitent:
                    Title = "Komitent";
                    break;
                case MenuItems.Partneri:
                    Title = "Partneri";
                    break;
                case MenuItems.Porezne_stope:
                    Title = "Porezne stope";
                    break;
                case MenuItems.Zaposlenici:
                    Title = "Zaposlenici";
                    break;
                case MenuItems.Gradovi:
                    Title = "Gradovi";
                    break;
                default:
                    break;
            }
        }

        private string _title = "Knjigovodstvo";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
