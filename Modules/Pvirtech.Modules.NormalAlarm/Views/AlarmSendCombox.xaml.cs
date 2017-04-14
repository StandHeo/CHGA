using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Pvirtech.Modules.NormalAlarm.Views
{
    /// <summary>
    /// AlarmSendCombox.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmSendCombox : UserControl
    {
        public AlarmSendCombox()
        {
            InitializeComponent();
        }

        private void RadComboBox_DropDownClosed(object sender, EventArgs e)
        {
            RadComboBox one = sender as RadComboBox;
            one.SelectedIndex = -1;

        }

        private void RadComboBox_DropDownOpened(object sender, EventArgs e)
        {
            RadComboBox one = sender as RadComboBox;
            if (one.ActualWidth < 350)
                one.DropDownWidth = new GridLength(350);
        }

        private void RadComboBoxItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
