using System;

namespace AccountingUI.Core.Models
{
    public interface ILoggedInUserModel
    {
        public string Token { get; set; }
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
    }
}