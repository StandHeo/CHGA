using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Mvvm;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class LinkXunZuClontrolViewModel :BindableBase, IConfirmation, IInteractionRequestAware
    {
        private WorkingAlarmInfoViewModel _alarm;

        public WorkingAlarmInfoViewModel Alarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }

        private Patrol patrol;

        public Patrol Patrol
        {
            get { return patrol; }
            set { patrol = value; }
        }

        //private readonly IDialogueRepository _dialogueRepository;
        //private readonly IUserRepository _userRepository;
        private readonly IPoliceCaseRepository _policeCaseRepository;
        public LinkXunZuClontrolViewModel(Patrol p,WorkingAlarmInfoViewModel alarm)
        {
            this.DialCommand = new DelegateCommand<object>(this.DialPhone);
            this.SendPhoneWord = new DelegateCommand<object>(this.SendPhoneWordByNumber);
            Patrol = p;
            Alarm = alarm;
            _SendMessager = new DelegateCommand(SendMessagerByNo);
            _SendPhoneMessager = new DelegateCommand(SendPhoneMessagerByNo);
            //_userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            //_dialogueRepository = ServiceLocator.Current.GetInstance<IDialogueRepository>();

            _policeCaseRepository = ServiceLocator.Current.GetInstance<IPoliceCaseRepository>();
            
          
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        private async void SendMessagerByNo()
        {
            if (!string.IsNullOrEmpty(Content01) && Alarm != null && Patrol != null)
            {
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("sessionId", "");
                para.Add("senderId", LocalDataCenter.GetLocalUser().UserNo);
                para.Add("senderName", LocalDataCenter.GetLocalUser().UserName);
                para.Add("isForcedReply", 0);
                para.Add("content", Content01);
                if (Alarm == null)
                    para.Add("jqlsh", "");
                else
                    para.Add("jqlsh", Alarm.Jqlsh);
                para.Add("fjlb", "[]");
                para.Add("member", Patrol.CallNo.ToArray());
                //var result = await _dialogueRepository.SendOrder(para);
                //if (result)
                //{
                //    EventCenter.PublishError("发送指令成功！");
                //    this.Confirmed = true;
                //    this.FinishInteraction();
                //}
                //else
                //    EventCenter.PublishError("发送失败");
            }
            else
            {
                EventCenter.PublishError("缺少主要参数，发送失败!");
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        public async void SendPhoneMessagerByNo()
        { 
           // PhoneMes obj = new PhoneMes();
           // obj.content = obj.content;
           // obj.phone = obj.phone;
           // obj.title = obj.title;
           // obj.sender = obj.sender; 

           //var  backValue = await _policeCaseRepository.SendSMS(GlobalConfig.GetInstance().SMSUrl, obj.content, obj.phone);
          
        
           //// var backValue = await _userRepository.SendPhoneMessager(models);
           //if (backValue)
           //{
           //    EventCenter.PublishError("短信发送成功");
           //    this.Confirmed = true;
           //    this.FinishInteraction();
           //}
           //else
           //    EventCenter.PublishError("短信发送失败");
        }
        
        private void SendPhoneWordByNumber(object obj)
        {
            
        }

        /// <summary>
        /// 拨打电话
        /// </summary>
        /// <param name="obj"></param>
        private void DialPhone(object obj)
        {
            
        }

        #region 方法

        private void Cancel()
        {

        
        }

        #endregion

        #region 接口命令

        public ICommand SendPhoneWord { get; private set; }

        public ICommand DialCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        #endregion

        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
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

        private string _PhoneContent;

        public string PhoneContent
        {
            get { return _PhoneContent; }
            set 
            {
                _PhoneContent = value;
                OnPropertyChanged(() => PhoneContent);
            }
        }

        public object Content
        {
            get;
            set;
        }

        private string _Content01;

        /// <summary>
        /// 指令信息
        /// </summary>
        public string Content01
        {
            get { return _Content01; }
            set 
            {
                _Content01 = value;
                OnPropertyChanged(() => Content01);
            }
        }

        private ICommand _SendMessager;

        public ICommand SendMessager
        {
            get { return _SendMessager; }
        }

        private ICommand _SendPhoneMessager;

        public ICommand SendPhoneMessager
        {
            get { return _SendPhoneMessager; }
        }

        public INotification Notification
        {
            get;
            set;
        }

        public Action FinishInteraction
        {
            get;
            set;
        }
        public Action Activate { get; set; }
        public bool IsModal { get; set; }
    }
}
