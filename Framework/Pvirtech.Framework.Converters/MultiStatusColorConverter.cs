using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Pvirtech.Framework.Converters
{
    public class MultiStatusColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count()>1)
            {
                int appStatus =System.Convert.ToInt32(values[0]);
                int status =System.Convert.ToInt32(values[1]); 
                if (status==1)
                {
                    //if (appStatus==1)
                    //{
                    //     return new SolidColorBrush(Colors.Green);
                    //}
                    //return new SolidColorBrush(Colors.Red);
                    return new SolidColorBrush(Colors.Green);
                } 
                else
                {
                    return new SolidColorBrush(Colors.Red); 
                }
            }          
            return new SolidColorBrush(Colors.Black); 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
