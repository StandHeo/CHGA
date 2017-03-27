using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class SexToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string defaultValue = "男";
            if (value != null)
            {
                int result = 0;
                int.TryParse(value.ToString(), out result);
                return result == 1 ? "女" : defaultValue;
            }
            return defaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
