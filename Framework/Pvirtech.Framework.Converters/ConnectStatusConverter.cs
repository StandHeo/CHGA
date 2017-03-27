using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Pvirtech.Framework.Converters
{
    public class ConnectStatusConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value != null)
            {
                int status = 0;
                int.TryParse(value.ToString(), out status);
                if(status >= 3)
                {
                    return new BitmapImage( new Uri("pack://application:,,,/Pvirtech.Framework.Resources;component/Images/bad.png"));
                }
                else if (status >= 2)
                {
                    return new BitmapImage(new Uri("pack://application:,,,/Pvirtech.Framework.Resources;component/Images/notbad.png"));
                }
                else
                {
                    return  new BitmapImage(new Uri("pack://application:,,,/Pvirtech.Framework.Resources;component/Images/good.png"));
                }
            }
            return new BitmapImage(new Uri("pack://application:,,,/Pvirtech.Framework.Resources;component/Images/bad.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
