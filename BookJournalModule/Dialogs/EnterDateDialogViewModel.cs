using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace BookJournalModule.Dialogs
{
    public class EnterDateDialogViewModel : BindableBase, IDialogAware
    {
        public EnterDateDialogViewModel()
        {
            ProcessCommand = new DelegateCommand(StartProcessing, CanStart);
        }

        public string Title => "Datum knjiženja";

        public DelegateCommand ProcessCommand { get; private set; }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set 
            { 
                SetProperty(ref _selectedDate, value);
                ProcessCommand.RaiseCanExecuteChanged();
            }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return SelectedDate != null;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        private void StartProcessing()
        {
            var result = ButtonResult.OK;
            var p = new DialogParameters();
            p.Add("date", SelectedDate);

            RequestClose?.Invoke(new DialogResult(result, p));
        }

        private bool CanStart()
        {
            return SelectedDate != null;
        }
    }
}
