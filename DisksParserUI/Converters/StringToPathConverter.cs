using System.Globalization;
using System.Windows.Data;

namespace DisksParserUI.Converters
{
    public class StringToPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            string? fileWithBannedWordsPath = values[0] as string;
            string? copyFolderPath = values[1] as string;

            string result = "";

            if (!String.IsNullOrWhiteSpace(fileWithBannedWordsPath))
            {
                result += $"Path to banned words file: {fileWithBannedWordsPath}\n";
            }
            if (!String.IsNullOrWhiteSpace(copyFolderPath))
            {
                result += $"Path to copy folder: {copyFolderPath}\n";
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}