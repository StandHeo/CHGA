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

namespace Pvirtech.Framework.Controls
{
    /// <summary>
    /// LoadWinControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadWinControl : UserControl
    {
        public LoadWinControl()
        {
            InitializeComponent();
        }
        public void ShowLoading(string msg = "请稍后...")
        {
            this.message.BusyContent = msg;
            this.cont.Visibility = Visibility.Visible;
        }
        public void CloseLoading()
        {
            this.cont.Visibility = Visibility.Collapsed;
        }

        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register("BusyContent", typeof(object), typeof(LoadWinControl), new UIPropertyMetadata("流转中..."));

        public object BusyContent
        {
            get
            {
                return GetValue(BusyContentProperty);
            }
            set
            {
                message.BusyContent = value;
                SetValue(BusyContentProperty, value);
            }
        }

        public static readonly DependencyProperty ShowBusyContentProperty = DependencyProperty.Register("ShowBusyContent", typeof(bool), typeof(LoadWinControl), new UIPropertyMetadata(false));

        public bool ShowBusyContent
        {
            get
            {
                return (bool)GetValue(BusyContentProperty);
            }
            set
            {
                message.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                SetValue(BusyContentProperty, value);
            }
        }
    }
}
