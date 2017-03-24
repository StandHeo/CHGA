using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Pvirtech.Framework.Controls
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class StoryBoardIndexVisibility : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            var isVisibility = false;
			var flag = value == null;
			if (flag)
			{
				value = false;
			}
			var flag2 = value is bool;
			if (flag2)
			{
                isVisibility = (bool)value;
			}
            return isVisibility ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
