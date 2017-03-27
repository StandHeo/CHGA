using Pvirtech.Framework.Helper;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    /// <summary>
    /// 警情来源
    /// </summary>
    public class PoliceSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                int v = 0;
                int.TryParse(value.ToString(), out v);
                AllEnum.PolSource p_Sort = (AllEnum.PolSource)v;
                return p_Sort.ToString();
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int p_Value = (int)Enum.Parse(typeof(AllEnum.PolSource), value.ToString());
            return p_Value;
        }
    }
}
