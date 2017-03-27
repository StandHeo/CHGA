using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    /// <summary>
    /// 根据值显示用户登录的图标
    /// </summary>
    public class LoginIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string backUrl;
            int newValue = Int32.Parse(value.ToString());
            if (newValue == 1)
            {
                backUrl = "pack://application:,,,/Pvirtech.CaseCommander;component/images/PC.png";
                return backUrl;
            }
            if (newValue == 2)
            {
                backUrl = "pack://application:,,,/Pvirtech.CaseCommander;component/images/iphone.png";
                return backUrl;
            }
            backUrl = "pack://application:,,,/Pvirtech.CaseCommander;component/images/iphone.png";
            return backUrl;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
