using Pvirtech.Modules.NormalAlarm.ViewModels;
using System.Windows.Controls;

namespace Pvirtech.Modules.NormalAlarm.Views
{
    /// <summary>
    /// VAlarmList01.xaml 的交互逻辑
    /// </summary>
    public partial class WorkingAlarms : UserControl
    {

        public WorkingAlarms(WorkingAlarmsViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
