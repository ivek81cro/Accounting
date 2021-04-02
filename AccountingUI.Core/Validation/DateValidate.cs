using System;
using System.Globalization;
using System.Windows.Controls;

namespace AccountingUI.Core.Validation
{
    public class DateValidate : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(value is DateTime || value == null, null);
        }
    }
}
