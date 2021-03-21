using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PartnersModule.Dialogs
{
    public class PartnerEditViewModel : BindableBase, IDialogAware
    {

        public event Action<IDialogResult> RequestClose;

        private PartnersModel _partner;
        private readonly IPartnersEndpoint _partnersEndpoint;
        private bool _changed = false;

        public PartnersModel Partner
        {
            get { return _partner; }
            set 
            { 
                SetProperty(ref _partner, value);
                RaisePropertyChanged(nameof(Partner));
                _changed = true;
            }
        }

        public DelegateCommand CloseDialogCommand { get; }

        public string Title => "Izmjena Podataka Partnera";

        public PartnerEditViewModel(IPartnersEndpoint partnersEndpoint)
        {
            CloseDialogCommand = new DelegateCommand(CloseDialog);
            _partnersEndpoint = partnersEndpoint;
        }

        private void CloseDialog()
        {
            var result = _changed ? ButtonResult.OK : ButtonResult.Cancel;
            var p = new DialogParameters();
            p.Add("partner", Partner);

            RequestClose?.Invoke(new DialogResult(result, p));
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
            var partnerId = parameters.GetValue<PartnersModel>("partner").Id;
            GetPartnerFromDatabase(partnerId);
        }

        private async void GetPartnerFromDatabase(int partnerId)
        {
            Partner = await _partnersEndpoint.GetById(partnerId);
        }
    }
}
