using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace Pvirtech.Framework.Converters
{
    public class TileStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var contentState = (FluidContentControlState)value;
            switch (contentState)
            {
                case FluidContentControlState.Small:
                    return TileViewItemState.Minimized;
                case FluidContentControlState.Normal:
                    return TileViewItemState.Restored;
                case FluidContentControlState.Large:
                    return TileViewItemState.Maximized;
                default:
                    return TileViewItemState.Restored;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var tileState = (TileViewItemState)value;
            switch (tileState)
            {
                case TileViewItemState.Minimized:
                    return FluidContentControlState.Small;
                case TileViewItemState.Restored:
                    return FluidContentControlState.Normal;
                case TileViewItemState.Maximized:
                    return FluidContentControlState.Large;
                default:
                    return FluidContentControlState.Normal;
            }
        }
    }
}
