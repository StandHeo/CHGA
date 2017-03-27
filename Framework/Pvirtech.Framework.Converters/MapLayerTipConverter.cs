using ESRI.ArcGIS.Client;
using Pvirtech.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Pvirtech.Framework.Converters
{
    public class MapLayerTipConverter : IMultiValueConverter
    { 
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null)
            {
                if (values[0]==null)
                {
                    return null;
                }
                LayerCollection layers= values[0] as LayerCollection;
                var maptip = values[1] as MapTip;
                switch (maptip.Name)
                {
                    case "xz":
                        var findItem = layers.FirstOrDefault(o=>o.ID=="1");
                        if (findItem!=null)
                        {
                            return findItem;
                        }
                        break;
                    case "tw":
                        var twItem = layers.FirstOrDefault(o => o.ID == "4");
                        if (twItem != null)
                        {
                            return twItem;
                        }
                        break;
                    default:
                        break;
                }
                //int ss = 9;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
