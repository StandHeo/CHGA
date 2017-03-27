using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
   public  class VideoOnlineConverter : IValueConverter
    {
       public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           int flag = 0;
           int.TryParse(value.ToString(), out flag);
           if (flag == 1)
           {
               return "pack://application:,,,/Pvirtech.Framework.Resources;component/Images/MDVRNormal.png";
           }
           else
           {
               return "pack://application:,,,/Pvirtech.Framework.Resources;component/Images/MDVROffline.png";
           }
       }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
