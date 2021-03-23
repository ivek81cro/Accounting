using Prism.Mvvm;
using Prism.Regions;

namespace AccountingUI.Core.TabControlRegion
{
    public class ViewModelBase : BindableBase, INavigationAware
    {

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            string param =  (string)navigationContext.Parameters["title"];
            return Title == param;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = (string)navigationContext.Parameters["title"];
        }
    }
}
