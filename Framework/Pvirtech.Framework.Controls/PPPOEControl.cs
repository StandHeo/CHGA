using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls; 
using System.Windows.Input;
namespace Pvirtech.Framework.Controls
{
    
    /// <summary>
    /// 拨号盘控件
    /// </summary>
    public class PPPOEControl : Control
    {
        private const string SwitchPart = "NumberBorder";
        public static readonly DependencyProperty PhoneNumberProperty = DependencyProperty.Register("PhoneNumber", typeof(string), typeof(PPPOEControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty LetterTextProperty = DependencyProperty.Register("LetterText", typeof(string), typeof(PPPOEControl), new PropertyMetadata(default(string)));
        //CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PPPOEControl), new PropertyMetadata(default(CornerRadius)));

        public event EventHandler<RoutedEventArgs> Click;
        static PPPOEControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PPPOEControl), new FrameworkPropertyMetadata(typeof(PPPOEControl)));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var _toggleButton = GetTemplateChild(SwitchPart) as Border;
            if (_toggleButton != null)
            {
                _toggleButton.MouseLeftButtonDown += ClickHandler;
                _toggleButton.MouseLeftButtonUp += _toggleButton_MouseLeftButtonUp;
                _toggleButton.MouseLeave += _toggleButton_MouseLeave;
            }
        }

        void _toggleButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 1.0;
        }

        void _toggleButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 1.0;
        }

        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }
        public string LetterText
        {
            get { return (string)GetValue(LetterTextProperty); }
            set { SetValue(LetterTextProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {

            //<LinearGradientBrush EndPoint="0.5,1" StartPoint="0.5,0">
            //    <GradientStop Color="#FFEFF4F7" Offset="0"/>
            //    <GradientStop Color="#FFD2DFE9" Offset="1"/>
            //    <GradientStop Color="#FED2DFE9" Offset="0.5"/>
            //    <GradientStop Color="#FEE9F0F3" Offset="0.362"/>
            //</LinearGradientBrush>

            this.Opacity = 0.9;
            Raise(Click, this, e);
        }

        public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
        {
            if (eventToRaise != null)
            {
                eventToRaise(sender, args);
            }
        }
    }
}
