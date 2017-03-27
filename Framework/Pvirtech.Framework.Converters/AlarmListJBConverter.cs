using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
   
    public class AlarmListJBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = null;
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                int i = 2;
                int.TryParse(value.ToString(), out i);
                if (i >= 2)
                    v = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/importtrack_0.png";
                else
                    v = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/importtrack_1.png";
            }
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
