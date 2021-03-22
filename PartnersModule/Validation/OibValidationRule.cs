using System.Globalization;
using System.Windows.Controls;

namespace PartnersModule.Validation
{
    public class OibValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string v = (string)value;

            foreach(char c in v)
            {
                if (c < '0' || c > '9')
                {
                    return new ValidationResult(false, "Unos smije sadržavati samo brojeve");
                }
            }

            if (v.Length != 11)
            {
                return new ValidationResult(false, "Broj znamenki nije ispravan");
            }

            return ValidationResult.ValidResult;
        }
    }
}
