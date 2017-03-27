using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class VehicleType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string backClor = "#a9a9a9"; 
            if (value != null)
            {
                int status = 0;
                int.TryParse(value.ToString(), out status);
                if (status > 1)
                    backClor = "#228b22";
            }
            return backClor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
