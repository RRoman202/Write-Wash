using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Write_Wash.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as int?;
            if(str == null)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76e383"));
            }
            else if(str >= 9)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7fff00"));
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76e383"));
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
