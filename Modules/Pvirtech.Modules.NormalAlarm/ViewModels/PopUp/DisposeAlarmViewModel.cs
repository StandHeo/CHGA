using Prism.Events;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class DisposeAlarmViewModel: IConfirmation, IInteractionRequestAware
    {
        //编译警告注释 20151211 BINGLE
        private readonly Prism.Events.IEventAggregator _eventAggregator;

        public DisposeAlarmViewModel(WorkingAlarmInfoViewModel alarm,IEventAggregator eventAggregator)
        {
            viewModel = alarm;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<Framework.Domain.CommonEventArgs>().Subscribe(OnGetEvent, ThreadOption.UIThread, true, (t) =>
            {
                return t.MessageType == Model.MessageTypes.ENDALARM;
            });
        }

        private void OnGetEvent(MessageModel obj)
        {
            switch (obj.MessageType)
            {
                case MessageTypes.ENDALARM:

                    #region 参数验证

                    AlarmBase m = obj.MessageBody as AlarmBase;
                    if (m == null)
                        return;
                    if (viewModel.Jqlsh != m.Jqlsh)
                        return;

                    #endregion
                    this.FinishInteraction();
                    break;
                default:
                    break;
            }
        }

        public WorkingAlarmInfoViewModel viewModel 
        {
            get; set;
        }

        #region 方法实现

        /// <summary>
        /// 关闭
        /// </summary>
        private void Cancel()
        {
            this.Confirmed = false;
            this.FinishInteraction();
        }


        #endregion

        /// <summary>
        /// 提示信息
        /// </summary>
        private string notifyMessage;

        public string NotifyMessage
        {
            get
            {
                return notifyMessage;
            }
            set
            {
                if (value != notifyMessage)
                {
                    notifyMessage = value;
                }
            }
        }
        public bool Topmost { get; set; }

        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
        public bool Confirmed
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public object Content
        {
            get;
            set;
        }

        /// <summary>
        /// 派警
        /// </summary>
        public ICommand ExcuteCommand { get; private set; }

        public INotification Notification
        {
            get;
            set;
        }
        public Action Activate { get; set; }
        public Action FinishInteraction
        {
            get;
            set;
        }

        public ICommand CancelCommand { get; private set; }
    }
}
