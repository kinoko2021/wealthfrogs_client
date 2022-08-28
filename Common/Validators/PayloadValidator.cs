using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WealthFrogs.Common.Validators
{
    public class PayloadValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int v = 0;

            try
            {
                if (((string)value).Length > 0)
                    v = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false,"非法输入");
            }

            if (v <= 0 || v % 100 != 0)
            {
                return new ValidationResult(false,"非法输入");
            }
            return ValidationResult.ValidResult;

        }
    }
}
