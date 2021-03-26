using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Accounting.CoreModule.Validation
{
    public class CustomValidate
    {
        public static void Validate(object? value, ValidationContext validationContext, ICollection<ValidationResult>? validationResults)
        {
            string name = validationContext.DisplayName;
            var data = value;

            switch (name)
            {
                case "Oib":
                    ValidateOib((string)data, validationResults, name);
                    return;
            }
        }

        private static void ValidateOib(string data, ICollection<ValidationResult> validationResults, string name)
        {
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
