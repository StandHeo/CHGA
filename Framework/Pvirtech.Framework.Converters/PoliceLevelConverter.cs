using System;
using System.Globalization;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class PoliceLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string backV = string.Empty;
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                    int v = 2;
                    int.TryParse(value.ToString(),out v);
                    switch (v)
	                {
                        case 1:
                            backV = "一级";
                            break;
                        case 2:
                            backV = "二级";
                            break;
                        case 3:
                            backV = "三级";
                            break;
		                default:
                            backV ="三级";
                            break;
	                }
            }
            return backV;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
