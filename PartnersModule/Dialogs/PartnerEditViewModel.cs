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
        private readonly IPartnersEndpoint _partnersEndpoint;


        public PartnerEditViewModel(IPartnersEndpoint partnersEndpoint)
        {
            _partnersEndpoint = partnersEndpoint;
            SavePartnerCommand = new DelegateCommand(CloseDialog);
        }

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand SavePartnerCommand { get; }
        public string Title => "Izmjena Podataka Partnera";

        private PartnersModel _partner;
        public PartnersModel Partner
        {
            get { return _partner; }
            set 
            { 
                SetProperty(ref _partner, value);
            }
        }

        private void CloseDialog()
        {
            if (Partner != null && !Partner.HasErrors)
            {
                _partnersEndpoint.PostPartner(Partner);
                var result = ButtonResult.OK;
                var p = new DialogParameters();
                p.Add("partner", Partner);

                RequestClose?.Invoke(new DialogResult(result, p));
            }
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
            Partner = parameters.GetValue<PartnersModel>("partner");
            if (Partner != null)
            {
                GetPartnerFromDatabase(Partner.Id);
            }
            else 
            {
                Partner = new PartnersModel();
                Partner.Reset();
            }
        }

        private async void GetPartnerFromDatabase(int partnerId)
        {
            Partner = await _partnersEndpoint.GetById(partnerId);            
        }
    }
}
