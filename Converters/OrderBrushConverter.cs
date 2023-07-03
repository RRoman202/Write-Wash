using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Write_Wash.Converters
{
    public class OrderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<int> quan = value as List<int>;
            int k = 0;
            foreach(int i in quan)
            {
                if(i > 3)
                {
                    k++;
                }
            }
            if(k == quan.Count())
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#20b2aa"));
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff8c00"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
