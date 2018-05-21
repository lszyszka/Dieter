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
    class RegExpValidationRule : ValidationRule
    {
        String regExp;
        String errorMassage;

        public RegExpValidationRule() { }

        public string RegExp { get => regExp; set => regExp = value; }
        public string ErrorMassage { get => errorMassage; set => errorMassage = value; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Equals(""))
            {
                return new ValidationResult(false, "Pole jest wymagane.");
            }
            else if (!Regex.IsMatch(value.ToString(), RegExp))
            {
                return new ValidationResult(false, ErrorMassage);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
