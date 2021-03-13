using AccountingUI.Core.Models;
using MvvmCross.ViewModels;

namespace AccountingUI.Core.ViewModels
{
    class PartnersViewModel : MvxViewModel
    {
        private MvxObservableCollection<PartnersModel> _partners;

        public MvxObservableCollection<PartnersModel> Partners
        {
            get { return _partners; }
            set { SetProperty(ref _partners, value); }
        }

    }
}
