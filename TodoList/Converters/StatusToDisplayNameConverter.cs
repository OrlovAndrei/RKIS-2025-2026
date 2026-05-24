using System.Globalization;
using System.Windows.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.Converters
{
    public class StatusToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TodoStatus status)
                return TodoItem.GetStatusDisplayName(status);
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}