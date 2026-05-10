using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TodoApp.Models;

namespace TodoApp.Desktop.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TodoStatus status)
            {
                return status switch
                {
                    TodoStatus.NotStarted => new SolidColorBrush(Colors.Gray),
                    TodoStatus.InProgress => new SolidColorBrush(Colors.Blue),
                    TodoStatus.Completed => new SolidColorBrush(Colors.Green),
                    TodoStatus.Postponed => new SolidColorBrush(Colors.Orange),
                    TodoStatus.Failed => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Black)
                };
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}