using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pvirtech.Framework.Controls
{
    /// <summary>
    /// WaitingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingWindow : Window
    {
        private WaitingWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.Closed += MainWindow_Closed;

            Init();
            m_timer = new System.Timers.Timer();
            m_timer.Interval = 220;
            m_timer.Elapsed += M_timer_Elapsed;
            m_timer.Start();
            this.Closed += WaitingWindow_Closed;

        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            m_timer.Close();
            this.Close();
        }

        int idx = 0;
        private void M_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var count = m_rectangles.Count;
                //暗色
                if (idx % count == 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            var color = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF21333F");
                            color.A = 100;
                            m_rectangles[i].Fill = new SolidColorBrush(color);
                            color = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF051931");
                            m_rectangles[i].Effect = new DropShadowEffect() { Color = Colors.Blue, Opacity = 1, BlurRadius = 10, ShadowDepth = 0.1, Direction = 0.8 };
                        }));
                    }
                }

                //亮色
                for (int i = 0; i < count; i++)
                {
                    if (idx % count == i)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            var color = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF64EBFE");
                            color.A = 255;
                            m_rectangles[i].Fill = new SolidColorBrush(color);
                            color = (Color)ColorConverter.ConvertFromString("#FF64EBFE");
                            color.A = 50;
                            m_rectangles[i].Effect = new DropShadowEffect() { Color = color, Opacity = 1, BlurRadius = 15, ShadowDepth = 0.5 };
                        }));

                    }
                }

                idx++;
                if (idx >= int.MaxValue)
                {
                    idx = 0;
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private List<Rectangle> m_rectangles = new List<Rectangle>();
        private Timer m_timer;

        private void Init()
        {
            int angle = 0;
            var centerX = this.Width / 2;
            var centerY = this.Height / 2;
            int num = 30;
            var step = 360 / num;
            for (int i = 0; i < num; i++)
            {
                Rectangle rec = new Rectangle();
                rec.Width = 5;
                rec.Height = 15;

                rec.Fill = new SolidColorBrush(Colors.Blue);
                Canvas.SetLeft(rec, centerX);
                Canvas.SetTop(rec, 20);
                TransformGroup gro = new TransformGroup();
                gro.Children.Add(new RotateTransform() { Angle = angle });
                gro.Children.Add(new SkewTransform());
                gro.Children.Add(new TranslateTransform());
                rec.RenderTransformOrigin = new Point(0, 4);
                rec.RenderTransform = gro;
                this.canvas.Children.Add(rec);
                m_rectangles.Add(rec);
                angle += step;
            }
        }

        private void WaitingWindow_Closed(object sender, EventArgs e)
        {
            _instance = null;
            m_timer.Stop();
            m_timer.Dispose();
            //_instance = new WaitingWindow();
        }

        private static WaitingWindow _instance;

        public static WaitingWindow Instanace
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WaitingWindow();
                }

                return _instance;
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
