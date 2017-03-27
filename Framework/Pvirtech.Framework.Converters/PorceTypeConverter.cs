using System;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    /// <summary>
    /// 警力类型转换
    /// </summary>
    public partial  class PorceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string type =value.ToString();
                switch (type)
                {
                    case "1001":
                        return "分局指挥中心";
                    case "1002":
                        return "科室";
                    case "1003":
                        return "业务大队";
                    case "1004":
                        return "派出所";
                    case "2001":
                        return "110巡组";
                    case "2002":
                        return "快反组";
                    case "3001":
                        return "备勤组";
                    case "3002":
                        return "临时指挥组";
                    default:
                        return "其他单位";
                }
            }
            return "其他单位";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
