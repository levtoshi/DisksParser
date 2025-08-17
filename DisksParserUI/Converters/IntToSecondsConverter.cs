using System.Globalization;
using System.Windows.Data;

namespace DisksParserUI.Converters
{
    public class IntToSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && intValue >= 0)
            {
                return $"{intValue / 60:D2}:{intValue % 60:D2}";
            }
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}