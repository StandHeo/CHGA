using Prism.Commands;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class ReturnAlarmControlViewModel : WorkingAlarmInfoViewModel, IConfirmation, IInteractionRequestAware
    {
        public ReturnAlarmControlViewModel(WorkingAlarmInfoViewModel alarm):base(alarm)
        {
            this.OKCommand = new DelegateCommand<object>(this.MCloseMession);
            this.CancelCommand = new DelegateCommand(this.Cancel);          
        }

        #region 接口命令
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
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

        public bool Confirmed
        {
            get;
            set;
        }

        public ICommand OKCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

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
        /// 提示信息
        /// </summary>
        private string notifyMessage;

        public string NotifyMessage
        {

            get { return notifyMessage; }
            set
            {
                if (value != notifyMessage)
                {
                    notifyMessage = value;
                    OnPropertyChanged(() => NotifyMessage);
                }
            }
        }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
        #endregion

        #region 方法

        /// <summary>
        /// 确认回退
        /// </summary>
        /// <param name="jams"></param>
        private async  void MCloseMession(object jams)
        {
            Result<bool> result = await base.ReturnAlarmByText(jams.ToString());
            if (result.Code == ResultCode.SUCCESS)
            {
                if (result.Model)
                {
                    NotifyMessage = "操作成功！";
                    this.Confirmed = true;
                    this.FinishInteraction();
                }
                else
                {
                    NotifyMessage = "操作失败！";
                }
            }
            else
            {
                NotifyMessage = "操作失败！代码：" + result.Code + " " + result.Message;
                this.FinishInteraction();
            }
        }

        private void Cancel()
        {
            this.Confirmed = false;
            this.FinishInteraction();
        }

        #endregion


    }
}
