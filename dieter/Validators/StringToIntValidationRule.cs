using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dieter.Validators
{
    class StringToIntValidationRule : ValidationRule
    {
        String errorMessage;
        public StringToIntValidationRule()
        {
        }

        public string ErrorMessage { get => errorMessage; set => errorMessage = value; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Equals(""))
            {
                return new ValidationResult(false, "Pole jest wymagane.");
            }
            else if (int.TryParse(value.ToString(), out int parseOutput))
            {
                if (parseOutput > 0)
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, ErrorMessage);
                }
            }
            else
            {
                return new ValidationResult(false, ErrorMessage);
            }
        }
    }
}
