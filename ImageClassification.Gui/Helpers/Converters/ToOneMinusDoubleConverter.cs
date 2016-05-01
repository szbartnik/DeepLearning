using System;
using System.Globalization;
using System.Windows.Data;

namespace Wkiro.ImageClassification.Gui.Helpers.Converters
{
    public class ToOneMinusDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double
                ? 1.0 - (double)value
                : 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
