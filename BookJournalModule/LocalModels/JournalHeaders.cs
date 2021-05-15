using AccountingUI.Core.Models;

namespace BookJournalModule.LocalModels
{
    public class JournalHeaders : AccountingJournalModel
    {
        private decimal _stanje;
        public decimal Stanje
        {
            get { return _stanje; }
            set { SetProperty(ref _stanje, value); }
        }
    }
}
