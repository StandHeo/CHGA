using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class DateHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string m_Value = string.Empty;
            if (value != null)
            {
                m_Value = value.ToString();
                if (!string.IsNullOrEmpty(m_Value))
                {
                    m_Value = value.ToString();

                    DateTime dateValue = DateTime.Now; 
                    bool successed = DateTime.TryParseExact(m_Value, "yyyyMMddHHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue);
                    if (successed)
                    {
                        string backValue = dateValue.ToString("HH:mm:ss");
                        return backValue;
                    }
                    else
                    {
                        return "--";
                    }
                }
            }
            return m_Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
