using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Wkiro.ImageClassification.Gui.Helpers.Converters
{
    public class ListOfStringsToStringConverter : IValueConverter
    {
        private const char Delimiter = ';'; // delimiter to join
        private readonly char[] _delimiters = { ';', ',', ' ' }; // possible delimiters, which might be in the string when converting back

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as string[];

            if (list == null || !list.Any())
                return null;

            var result = list.Aggregate((x, y) => $"{x}{Delimiter} {y}");
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valuestring = value as string;

            if (string.IsNullOrEmpty(valuestring))
                return null;

            // Do some conversion back to some List
            var list = valuestring
                .Split(_delimiters)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));

            return list.ToArray();
        }
    }
}