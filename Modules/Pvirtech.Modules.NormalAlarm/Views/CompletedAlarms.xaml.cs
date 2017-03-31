using Pvirtech.Modules.NormalAlarm.ViewModels;
using System.Windows.Controls;

namespace Pvirtech.Modules.NormalAlarm.Views
{
    /// <summary>
    /// VAlarmList02.xaml 的交互逻辑
    /// </summary>
    public partial class CompletedAlarms : UserControl
    {
        public CompletedAlarms(CompletedAlarmsViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
