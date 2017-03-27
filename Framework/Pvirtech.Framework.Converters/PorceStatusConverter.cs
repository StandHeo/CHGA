using System;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    /// <summary>
    /// 警力状态转换
    /// </summary>
    public partial class PorceStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                Int32 type = (int)value;
                switch (type)
                {
                    case 0:
                        return "工作中";
                    default:
                        return "工作中";
                }
            }
            return "工作中";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
