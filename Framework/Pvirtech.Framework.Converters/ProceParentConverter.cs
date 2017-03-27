using System;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class ProceParentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string type = (string)value;
                switch (type)
                {
                    case "510108991200":
                        return "桃溪";
                    case "510108990600":
                        return "府青";
                    case "510108990400":
                        return "双桥";
                    case "510108990200":
                        return "猛追";
                    case "510108990700":
                        return "二仙";
                    case "510108990500":
                        return "建设";
                    case "510108991100":
                        return "万年";
                    case "510108991000":
                        return "双水";
                    case "510108990800":
                        return "跳蹬";
                    case "510108991300":
                        return "圣灯";
                    case "510108991400":
                        return "保和";
                    case "510108991600":
                        return "青龙";
                    default:
                        return "其他";
                }
            }
            return "其他";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "其他";
        }
    }
}
