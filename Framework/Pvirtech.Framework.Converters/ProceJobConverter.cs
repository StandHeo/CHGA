using System;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class ProceJobConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value != null)
            {
                int type = (int)value;
                switch(type)
                {
                    case 1001:
                        return "带班领导";
                	case 1002:
                        return "主值班员";
                    case 1003:
                        return "副值班员";
                    case 1004:
                        return "值班民警";
                    case 2001:
                        return "巡组组长";
                    case 2002:
                        return "巡组组员";
                }
            }
            return "其他";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
