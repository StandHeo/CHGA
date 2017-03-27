 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    /// <summary>
    /// 根据值显示案件状态
    /// </summary>
    public class PoliceCaseTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strStatus = "新警情";
            if (value == null)
                return strStatus;
            int m_Value = 0;
            int.TryParse(value.ToString(), out m_Value);
            switch (m_Value)
            {
                case 1:
                    strStatus = "新警情";
                    break;
                case 3:
                    strStatus = "已处警";
                    break;
                case 5:
                    strStatus = "单兵接收";
                    break;
                case 7:
                    strStatus = "出警中";
                    break;
                case 9:
                    strStatus = "到达现场";
                    break;
                case 11:
                    strStatus = "已反馈";
                    break;
                case 13:
                    strStatus = "处理完毕";
                    break;
                default:
                    strStatus = "新警情";
                    break;
            }
            return strStatus;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strStatus = "新警情";
            int m_Value = 0;
            int.TryParse(value.ToString(), out m_Value);
            switch (m_Value)
            {
                case -1:
                case 1:
                case 2:
                    strStatus = "新警情";
                    break;
                case 3:
                    strStatus = "已接警";
                    break;
                case 4:
                    strStatus = "已处警";
                    break;
                case 5:
                    strStatus = "单兵接受";
                    break;
                case 6:
                    strStatus = "到达现场";
                    break;
                case 7:
                    strStatus = "撤离现场";
                    break;
                case 8:
                    strStatus = "处理完毕";
                    break;
                default:
                    strStatus = "新警情";
                    break;
            }
            return strStatus;
        }
    }
}
