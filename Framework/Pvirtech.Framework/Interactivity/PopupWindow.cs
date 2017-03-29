using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pvirtech.Framework.Interactivity
{
   public static class PopupWindows
    {
        private static readonly InteractionRequest<INotification> popupWindowControl = new InteractionRequest<INotification>();
        public static IInteractionRequest PopupWindowControl
        {
            get
            {
                return popupWindowControl;
            }
        }
        public static InteractionRequest<INotification> GetInstance()
        {
            return popupWindowControl;
        }

       /// <summary>
       /// 消息提示
       /// </summary>
       /// <param name="txt">提示内容</param>
        public static void ShowWinMessage(string txt, bool surfaceShow = true)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                PopupWindows.GetInstance().Raise(
                    new Notification()
                    {
                        Topmost = surfaceShow,
                        Title = "消息提示",
                        Content = txt,
                    }, null);
            }));
            return;
        }
    }
}
