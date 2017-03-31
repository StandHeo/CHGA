using Pvirtech.Modules.NormalAlarm.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Pvirtech.Modules.NormalAlarm.StyleSlectors
{
    public class AlarmStyleSlector : StyleSelector
    {
        public override System.Windows.Style SelectStyle(object item, DependencyObject container)
        {
            if (item is WorkingAlarmInfoViewModel)
            {
                WorkingAlarmInfoViewModel club = item as WorkingAlarmInfoViewModel;
                if (club.CaseLevel <= 1)
                    return BigStadiumStyle;
                else
                    return SmallStadiumStyle;
            }
            return null;
        }
        public Style BigStadiumStyle { get; set; }
        public Style SmallStadiumStyle { get; set; }
    }
}
