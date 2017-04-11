using Prism.Mvvm;
using Pvirtech.Framework.Common;
using Pvirtech.Framework.Domain;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class AlarmHintControlViewModel : BindableBase, IInteractionRequestAware, IConfirmation
    {
        //编译警告注释 20151211 BINGLE
        //private readonly IUserRepository _eventAggregator;
        private int Type = 0;
        
        /// <summary>
        /// 离线提示
        /// </summary>
        /// <param name="m">警情对象</param>
        /// <param name="selectPolice">巡组集合</param>
        /// <param name="type">操作类型{0：处警；1:催促}</param>
        public AlarmHintControlViewModel(MAlarmInfo m, List<Patrol> selectPolice,int type = 0)
        {
            Type = type;
            WorkingAlarmInfoViewModel mm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(mm, m);
            ViewModel = mm;
            Patrols = selectPolice;
            Init();
            okCommand = new DelegateCommand(DisposeAlarm);
            sendPhoneMessage = new DelegateCommand(SendMessager);
            PhoneContent = GlobalConfig.GetInstance().SendZLInform;
        }

        private void SendMessager(object obj)
        {
            //foreach (var item in Patrols)
            //{
            //    _eventAggregator.SendPhoneMessager("《成华公安》", LocalDataCenter.GetLocalUser().UserName + "(" + LocalDataCenter.GetLocalUser().UserNo + ")", item.AppNo, PhoneContent);
            //}
        }

        #region 方法

        /// <summary>
        /// 派警
        /// </summary>
        /// <param name="obj"></param>
        private async void DisposeAlarm(object obj)
        {
            if (Patrols != null &&　Patrols.Count > 0)
	        {
                switch (Type)
                {
                        //处警
                    case 0:
                        foreach (var item in Patrols)
	                    {
		                    await ViewModel.ExcuteMession(GlobalConfig.GetInstance().SendAlarmWord,item.GroupName,item.GroupNo,1,0);
	                    }
                        break;
                        //催促
                    case 1:
                        foreach (var item in Patrols)
                        {
                            Result<bool> result = await ViewModel.UrgeAlramByIdOrText(item.GroupNo,"0");
                        }
                        break;
                    default:
                        break;
                }
                this.FinishInteraction();
	        }
        }

        public void Init()
        {
            if (Patrols != null &&Patrols.Count > 0)
            {
                string text = "";
                foreach (var item in Patrols)
	            {
                    text += item.GroupName+",";
	            }
                LiaisonPolice = text + "无法联络。你还可以通过如下方式通知对方!";
            }
        }

        #endregion

        #region  字段/定义命令

        private string phoneContent;

        /// <summary>
        /// 短信文本
        /// </summary>
        public string PhoneContent
        {
            get { return phoneContent; }
            set 
            {
                phoneContent = value;
                OnPropertyChanged(() => PhoneContent);
            }
        }

        private WorkingAlarmInfoViewModel viewModel;

        /// <summary>
        /// 页面数据
        /// </summary>
        public WorkingAlarmInfoViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                OnPropertyChanged(() => ViewModel);
            }
        }

        private List<Patrol> patrols;

        /// <summary>
        /// 离线的警力对象
        /// </summary>
        public List<Patrol> Patrols
        {
            get { return patrols; }
            set { patrols = value; }
        }

        private string liaisonPolice;

        /// <summary>
        /// 联络的巡组名称
        /// </summary>
        public string LiaisonPolice
        {
            get { return liaisonPolice; }
            set
            {
                liaisonPolice = value;
                OnPropertyChanged(() => LiaisonPolice);
            }
        }

        private ICommand okCommand;

        private ICommand sendPhoneMessage;

        /// <summary>
        /// 发送短信
        /// </summary>
        public ICommand SendPhoneMessage
        {
            get { return sendPhoneMessage; }
        }

        public ICommand OkCommand
        {
            get { return okCommand; }
        }

        #endregion


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

        public bool IsModal
        {
            get;
            set;
        }

        public bool Topmost
        {
            get;
            set;
        }

        public System.Windows.ResizeMode ResizeMode
        {
            get;
            set;
        }

        public System.Windows.WindowState WindowState
        {
            get;
            set;
        }
    }
}
