using System;
using System.Windows.Data;

namespace MoonBurst.Helper
{
    public sealed class EnumeratorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string enumString = "";
                if (value != null) enumString = Enum.GetName((value.GetType()), value);
                return enumString;
            }
            catch
            {
                return string.Empty;
            }
        }


        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}