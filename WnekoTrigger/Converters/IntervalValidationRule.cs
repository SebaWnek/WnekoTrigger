using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WnekoTrigger
{
    class IntervalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = (string)value;
            string[] strings = str.Split(new char[] { ',', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                foreach(string s in strings)
                {
                    int.Parse(s);
                }
            }
            catch
            {
                return new ValidationResult(false, "Sequence must contain only numbers or separators");
            }
            return ValidationResult.ValidResult;
        }
    }
}
