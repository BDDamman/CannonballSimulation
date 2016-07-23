using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Cannon_Simulation
{
    class IndexToSolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color clr = new Color();

            switch ((int)value)
            {
                case 0:
                    clr = (Color)ColorConverter.ConvertFromString("#FFed754e");
                    return new SolidColorBrush(clr);
                case 1:
                    clr = (Color)ColorConverter.ConvertFromString("#FF31af28");
                    return new SolidColorBrush(clr);
                case 2:
                    clr = (Color)ColorConverter.ConvertFromString("#FFed4e75");
                    return new SolidColorBrush(clr);
                case 3:
                    clr = (Color)ColorConverter.ConvertFromString("#FF9b35d6");
                    return new SolidColorBrush(clr);
                default:
                case 4:
                    clr = (Color)ColorConverter.ConvertFromString("#FFbaed4e");
                    return new SolidColorBrush(clr);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}
