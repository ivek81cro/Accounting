using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace Accounting.MainModule.Dialogs.AreYouSure
{
    public class AreYouSureViewModel : BindableBase, IDialogAware
    {
        public AreYouSureViewModel()
        {
            ButtonClickCommand = new DelegateCommand<string>(SetAnswer);
        }

        private void SetAnswer(string result)
        {
            Answer = result == "true";
            var answer = Answer ? ButtonResult.OK : ButtonResult.Cancel;

            RequestClose?.Invoke(new DialogResult(answer));
        }

        public DelegateCommand<string> ButtonClickCommand { get; private set; }

        public string Title => "Upozorenje";

        private string _messageText;
        public string MessageText
        {
            get { return _messageText; }
            set { SetProperty(ref _messageText, value); }
        }

        public event Action<IDialogResult> RequestClose;

        private bool _answer = false;
        public bool Answer
        {
            get { return _answer; }
            set { SetProperty(ref _answer, value); }
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
            MessageText = parameters.GetValue<string>("message");
            if (MessageText == null)
            {
                MessageText = "Da li ste sigurni da želite brisati podatke?";
            }
        }
    }
}
