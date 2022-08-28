using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WealthFrogs.Converters
{
    public class StatusToWordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int status = (int)value;
                if (status == 0)
                    return "已报";
                if (status == 1)
                    return "部成";
                if (status == 2)
                    return "已撤";
                if (status == 3)
                    return "部成已撤";
                if (status == 4)
                    return "已成";
            }
            return "--";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
