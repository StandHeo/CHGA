using System;
using System.Globalization;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
	[ValueConversion(typeof(DateTime), typeof(string))]
	public class StoryBoardTimeConverter : IValueConverter
	{ 
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DateTime.Parse(value.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None).ToString("HH:mm");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
