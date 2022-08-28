using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WealthFrogs.Common.Validators
{
    public class PriceValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double v = 0;

            try
            {
                if (((string)value).Length > 0)
                    v = double.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "非法输入");
            }

            if (v <= 0)
            {
                return new ValidationResult(false, "非法输入");
            }
            return ValidationResult.ValidResult;
        }
    }
}
