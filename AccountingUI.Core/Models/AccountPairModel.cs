using AccountingUI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class AccountPairModel : ValidationBindableBase
    {
        public int Id { get; set; }
        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _description;
        [Required]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _account;
        [Required]
        public string Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        private string _bookName;
        [Required]
        public string BookName
        {
            get { return _bookName; }
            set { SetProperty(ref _bookName, value); }
        }
    }
}
