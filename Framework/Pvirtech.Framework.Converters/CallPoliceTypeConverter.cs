using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class CallPoliceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string back = "- -";
            if (value == null)
            {
                return back;
            }
            int objValue = 0;
            int.TryParse(value.ToString(),out objValue);
            switch (objValue)
            {
                case 0:
                    back = "电话报警";
                    break;
                case 1:
                    back = "现场报警";
                    break;
                case 2:
                    back = "巡警报警";
                    break;
            }
            return back; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
