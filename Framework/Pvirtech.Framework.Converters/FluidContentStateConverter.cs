using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace Pvirtech.Framework.Converters
{
    public class FluidContentStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var contentState = (FluidContentControlState)value;
            switch (contentState)
            {
                case FluidContentControlState.Small:
                    return FluidContentControlState.Small;
                case FluidContentControlState.Normal:
                    return FluidContentControlState.Normal;
                case FluidContentControlState.Large:
                    return FluidContentControlState.Large;
                default:
                    return FluidContentControlState.Normal;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var fluidState = (FluidContentControlState)value;
            switch (fluidState)
            {
                case FluidContentControlState.Small:
                    return FluidContentControlState.Small;
                case FluidContentControlState.Normal:
                    return FluidContentControlState.Normal;
                case FluidContentControlState.Large:
                    return FluidContentControlState.Large;
                default:
                    return FluidContentControlState.Normal;
            }
        }
    }
}
