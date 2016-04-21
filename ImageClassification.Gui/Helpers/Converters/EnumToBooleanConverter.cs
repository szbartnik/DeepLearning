using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Wkiro.ImageClassification.Gui.Helpers.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null || parameter == null || !(value is Enum))
                return false;

            var currentState = value.ToString();
            var stateStrings = parameter.ToString();
            var toReturn = false;

            foreach (var state in stateStrings.Split(',', ' ', ';').Select(x => x.Trim()))
            {
                if (state == "!")
                    toReturn = !toReturn;

                if (currentState == state)
                    return !toReturn;
            }

            return toReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
