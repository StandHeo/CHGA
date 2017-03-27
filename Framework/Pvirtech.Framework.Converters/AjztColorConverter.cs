using System;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class AjztColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string colorStr = "OrangeRed";
                int result = 0;
                int.TryParse(value.ToString(), out result);
                switch (result)
                {
                    case 1:
                        colorStr = "OrangeRed";
                        break;
                    case 3:
                        colorStr = "Orange";
                        break;
                    case 5:
                        colorStr = "Green";
                        break;
                    case 7:
                        colorStr= "#0099FF";
                        break;
                    case 9:
                    case 11:
                        colorStr="Green";
                        break;
                    case 13:
                        colorStr="Black"; 
                        break;
                    default:
                        colorStr= "OrangeRed";
                        break;
                }
                return colorStr;
            }
            else
            {
                return "OrangeRed";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
 
}
