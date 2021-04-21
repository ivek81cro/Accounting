using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace BookAccountsModule.Dialogs
{
    public class AddEditDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IBookAccountsEndpoint _bookAccountsEndpoint;

        public AddEditDialogViewModel(IBookAccountsEndpoint bookAccountsEndpoint)
        {
            _bookAccountsEndpoint = bookAccountsEndpoint;

            SaveAccountCommand = new DelegateCommand(SaveAccount);
        }

        public DelegateCommand SaveAccountCommand { get; private set; }

        public string Title => "Kontni plan";

        public event Action<IDialogResult> RequestClose;

        private BookAccountModel _bookAccount;
        public BookAccountModel BookAccount
        {
            get { return _bookAccount; }
            set { SetProperty(ref _bookAccount, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            BookAccount = parameters.GetValue<BookAccountModel>("account");
            if(BookAccount == null)
            {
                BookAccount = new BookAccountModel() { Konto="", Opis="" };
            }
        }

        private async void SaveAccount()
        {
            if (BookAccount != null && !BookAccount.HasErrors)
            {
                var result = await _bookAccountsEndpoint.Insert(BookAccount);
                if (result)
                {
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                }
            }
        }
    }
}
