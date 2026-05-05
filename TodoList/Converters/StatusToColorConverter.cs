using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;
using System.Windows.Data;
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
					TodoStatus.Completed => "#4CAF50",
					TodoStatus.InProgress => "#2196F3",
					TodoStatus.Postponed => "#FF9800",
					TodoStatus.Failed => "#F44336",
					_ => "#9E9E9E"
				};
			}
			return "#9E9E9E";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
