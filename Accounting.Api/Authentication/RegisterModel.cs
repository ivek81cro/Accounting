using System.ComponentModel.DataAnnotations;

namespace Accounting.Api.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
