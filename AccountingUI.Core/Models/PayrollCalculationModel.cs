namespace AccountingUI.Core.Models
{
    public class PayrollCalculationModel : PayrollModel
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }
    }
}
