using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class OnlineColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string colorStr = "Gray";
            if (value != null)
            {
                int result = 0;
                int.TryParse(value.ToString(), out result);
                if (result>0)
                {
                       colorStr = "DarkBlue";
                }
                //switch (result)
                //{
                //    case 1:
                //        colorStr = "DarkBlue";
                //        break;
                //    case 3:
                //        colorStr = "Green";
                //        break;
                //    case 4:
                //        colorStr = "Red";
                //        break;
                //}
            }
            return colorStr;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
