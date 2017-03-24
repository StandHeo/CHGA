using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.Framework.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.Framework.Controls;assembly=Pvirtech.Framework.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomButton/>
    ///
    /// </summary>
    public class CustomButton : Button
    {
        static CustomButton()
        {
            PhoneProcessKeyProperty = DependencyProperty.Register("PhoneProcessKey", typeof(PressKey), typeof(CustomButton), new PropertyMetadata(null, new PropertyChangedCallback(CustomButton.OnCurrentPressKeyPropertyChangedCallBack)));
            CurrentStateProperty = DependencyProperty.Register("CurrentState", typeof(CustomButtonStates), typeof(CustomButton), new PropertyMetadata(CustomButtonStates.Nomal));
            PressKeyProperty = DependencyProperty.Register("PressKey", typeof(PressKey), typeof(CustomButton), new PropertyMetadata(null));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomButton), new PropertyMetadata(new CornerRadius(2.0)));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButton), new FrameworkPropertyMetadata(typeof(CustomButton)));
        }
        public static readonly DependencyProperty PhoneProcessKeyProperty;

        public static readonly DependencyProperty CurrentStateProperty;

        public static readonly DependencyProperty PressKeyProperty;

        public static readonly DependencyProperty CornerRadiusProperty;

        public PressKey PhoneProcessKey
        {
            get
            {
                return (PressKey)base.GetValue(PhoneProcessKeyProperty);
            }
            set
            {
                base.SetValue(PhoneProcessKeyProperty, value);
            }
        }

        public CustomButtonStates CurrentState
        {
            get
            {
                return (CustomButtonStates)base.GetValue(CurrentStateProperty);
            }
            set
            {
                base.SetValue(CurrentStateProperty, value);
            }
        }

        public PressKey PressKey
        {
            get
            {
                return (PressKey)base.GetValue(PressKeyProperty);
            }
            set
            {
                base.SetValue(PressKeyProperty, value);
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)base.GetValue(CornerRadiusProperty);
            }
            set
            {
                base.SetValue(CornerRadiusProperty, value);
            }
        }



        private static void OnCurrentPressKeyPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomButton)d).OnCurrentPressKeyPropertyChangedCallBack((PressKey)e.NewValue);
        }

        private void OnCurrentPressKeyPropertyChangedCallBack(PressKey newValue)
        {
            bool flag = this.PressKey.Display != newValue.Display;
            if (flag)
            {
                this.CurrentState = CustomButtonStates.Nomal;
            }
            else
            {
                this.CurrentState = newValue.IsPress;
                base.ReleaseAllTouchCaptures();
                base.ReleaseMouseCapture();
                base.ReleaseStylusCapture();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.IsPressed = true;
            e.Handled = true;
            base.ReleaseAllTouchCaptures();
            base.ReleaseMouseCapture();
            base.ReleaseStylusCapture();
            CriticalExecuteCommandSource(this, false);
            base.OnMouseDown(e);
        }
        [SecurityCritical]
        private void CriticalExecuteCommandSource(ICommandSource commandSource, bool userInitiated)
        {
            ICommand command = commandSource.Command;
            bool flag = command != null;
            if (flag)
            {
                object commandParameter = commandSource.CommandParameter;
                bool flag2 = command.CanExecute(commandParameter);
                if (flag2)
                {
                    command.Execute(commandParameter);
                }
            }
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.IsPressed = false;
            e.Handled = true;
            base.OnMouseUp(e);
        }
    }
}
