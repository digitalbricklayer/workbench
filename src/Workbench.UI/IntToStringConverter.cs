using System;
using System.Windows.Data;

namespace Workbench
{
    public sealed class IntToStringConverter : IValueConverter
    {
        public int EmptyStringValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is string)
                return value;
            else if (value is int && (int) value == EmptyStringValue)
                return string.Empty;
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            if (value is string s)
            {
                if (Int32.TryParse(s, out int result))
                    return result;
                else
                    return EmptyStringValue;
            }
            return value;
        }
    }
}
