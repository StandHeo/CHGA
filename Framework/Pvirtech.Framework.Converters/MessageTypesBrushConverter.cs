using Pvirtech.Commander.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Pvirtech.Framework.Converters
{
   
    [ValueConversion(typeof(MessageTypes), typeof(Brush))]
    public class MessageTypesBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value == null;
            object result;
            if (flag)
            {
                result = string.Empty;
            }
            else
            {
                //MessageTypes messageTypes = value.ToExactlyEnum<MessageTypes>();
                //BrushKeyAttribute custermAttribute = messageTypes.GetCustermAttribute<BrushKeyAttribute>();
                //object obj = Application.Current.TryFindResource(custermAttribute.BrushKey);
                //result = (obj ?? Brushes.Transparent);
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
