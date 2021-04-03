using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Validation
{
    public class CustomValidate
    {
        public static void Validate(object value, ValidationContext validationContext, ICollection<ValidationResult> validationResults)
        {
            string name = validationContext.DisplayName;
            var data = value;

            switch (name)
            {
                case "Oib":
                    ValidateOib((string)data, validationResults, name);
                    break;
                case "Naziv":
                    ValidateMinLength((string)data, validationResults, name);
                    break;
                case "Ime":
                    ValidateMinLength((string)data, validationResults, name);
                    break;
                case "Prezime":
                    ValidateMinLength((string)data, validationResults, name);
                    break;
                case "Mjesto":
                    ValidateMinLength((string)data, validationResults, name);
                    break;
                case "Zupanija":
                    ValidateMinLength((string)data, validationResults, name);
                    break;
                case "Iban":
                    ValidateIban((string)data, validationResults, name);
                    break;
            }
        }

        private static void ValidateIban(string data, ICollection<ValidationResult> validationResults, string name)
        {
            if (data != null && data.Length < 21)
            {
                validationResults.Add(
                    new ValidationResult("Naziv mora sadržavati 21 znak."));
            }
        }

        private static void ValidateMinLength(string data, ICollection<ValidationResult> validationResults, string name)
        {
            if (data.Length < 3)
            {
                validationResults.Add(
                    new ValidationResult("Naziv mora sadržavati najmanje 3 znaka."));
            }
        }

        private static void ValidateOib(string data, ICollection<ValidationResult> validationResults, string name)
        {
            if (data == null)
            {
                return;
            }

            if(data.Length != 11)
            {
                validationResults.Add(
                    new ValidationResult("Oib nema odgovarajući broj znamenki"));
            }

            foreach (char c in data)
            {
                if (c < '0' || c > '9')
                {
                   validationResults.Add(new ValidationResult("Unos smije sadržavati samo brojeve"));
                }
            }
        }
    }
}
