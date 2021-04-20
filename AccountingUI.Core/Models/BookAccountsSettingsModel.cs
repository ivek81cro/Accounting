using AccountingUI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class BookAccountsSettingsModel : ValidationBindableBase
    {
        public int Id { get; set; }

        private string _bookName;
        [Required]
        public string BookName
        {
            get { return _bookName; }
            set { SetProperty(ref _bookName, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _account;
        [Required]
        public string Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        private string _side;
        [Required]
        public string Side
        {
            get { return _side; }
            set { SetProperty(ref _side, value); }
        }

        private bool _prefix;
        [Required]
        public bool Prefix
        {
            get { return _prefix; }
            set 
            {
                SetProperty(ref _prefix, value);
            }
        }
    }    
}
