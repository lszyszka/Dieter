using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dieter.Validators
{
    class PatternValidationRule : ValidationRule
    {
        String pattern;
        String error;

        public PatternValidationRule() { }

        public string Pattern { get => pattern; set => pattern = value; }
        public string Error { get => error; set => error = value; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Equals(""))
            {
                return new ValidationResult(false, "Pole nie może być puste.");
            }
            else if (!Regex.IsMatch(value.ToString(), Pattern))
            {
                return new ValidationResult(false, Error);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
