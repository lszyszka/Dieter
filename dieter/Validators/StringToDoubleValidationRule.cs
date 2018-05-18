using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dieter.Validators
{
    class StringToDoubleValidationRule : ValidationRule
    {
        String error;
        public StringToDoubleValidationRule()
        {
        }

        public string Error { get => error; set => error = value; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Equals(""))
            {
                return new ValidationResult(false, "Pole nie może być puste.");
            }
            else if (double.TryParse(value.ToString(), NumberStyles.Any, new CultureInfo("en-US"), out double output))
            {
                if (output > 0)
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, Error);
                }
            }
            else
            {
                return new ValidationResult(false, Error);
            }
        }
    }
}
