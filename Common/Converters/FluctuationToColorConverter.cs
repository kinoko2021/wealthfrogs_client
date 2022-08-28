using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WealthFrogs.Converters
{
    /// <summary>
    /// 股票涨幅接口数据转换为显示颜色
    /// </summary>
    public class FluctuationToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((double)value > 0)
                    return "red";
                return (double)value < 0 ? "green" : "white";
            }
            return "white";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
