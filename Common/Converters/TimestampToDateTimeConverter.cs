using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WealthFrogs.Converters
{
    public class TimestampToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-- -- --";
            }
            long timestamp = (long)value;
            DateTime nowTime;
            if (timestamp.ToString().Length == 13)
            {
                nowTime = new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(timestamp);
            }
            else
            {
                nowTime = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timestamp);
            }
            return nowTime.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
