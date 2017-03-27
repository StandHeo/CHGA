using System;
using System.Globalization;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    /// <summary>
    /// 根据值显示对应的图片
    /// </summary>
    public class AddImageShowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string backUrl;
            int newValue = Int32.Parse(value.ToString());
            if (newValue == 0)
            {
                backUrl = "pack://application:,,,/Pvirtech.CaseCommander;component/images/add.png";
                return backUrl;
            }
            if (newValue == 1)
            {
                backUrl = "pack://application:,,,/Pvirtech.CaseCommander;component/images/sub.png";
                return backUrl;
            }
            backUrl = "pack://application:,,,/Pvirtech.CaseCommander;component/images/add.png";
            return backUrl;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
