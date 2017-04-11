using Microsoft.Practices.Unity;
using System;
using System.Windows;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Events;
using Prism.Commands;
using Pvirtech.Model;
using Pvirtech.Framework.Common;
using Pvirtech.Services;
using Pvirtech.Framework.Interactivity;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    /// <summary>
    /// 处警框ViewModel
    /// </summary>
    public class ExcuteCaseViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        private readonly IEventAggregator _eventAggregator; 
        private readonly IUnityContainer _container;

        public ExcuteCaseViewModel(IUnityContainer container, IEventAggregator eventAggregator, WorkingAlarmInfoViewModel alarm)
        {
            _eventAggregator = eventAggregator;
            _container = container;
            this.CancelCommand = new DelegateCommand(this.Cancel);
            this.FeedBackAlarm = new DelegateCommand(this.FeedBackAlarmClick);
            this.AlarmCloseCase = new DelegateCommand(JieAnWin);
            JjAlarmCmd = new DelegateCommand<AlarmStatusItem>(OnJjAlarm);
            NotifyMessage = null;
            _eventAggregator.GetEvent<Framework.Domain.CommonEventArgs>().Subscribe(OnGetEvent, ThreadOption.UIThread, true);
            this.CancelCommand = new DelegateCommand(this.Cancel);
            NotifyMessage = null; 
            viewModel = alarm;
            viewModel.Init();
        }

        
      

        private void OnJjAlarm(AlarmStatusItem obj)
        {
            ShowZhiYin(obj);
        }

        private void ShowZhiYin(AlarmStatusItem obj)
        {
            switch (obj.StatusName)
            {
                case "接警":
                    viewModel.ShowOne = true;
                    viewModel.ShowSecond = false;
                    viewModel.ShowThree = false;
                    viewModel.ShowFour = false;
                    break;
                case "派警":
                    viewModel.ShowSecond = true;
                    viewModel.ShowOne = false;
                    viewModel.ShowThree = false;
                    viewModel.ShowFour = false;
                    break;
                case "到达":
                    viewModel.ShowThree = true;
                    viewModel.ShowSecond = false;
                    viewModel.ShowOne = false;
                    viewModel.ShowFour = false;
                    break;
                case "结案":
                    viewModel.ShowFour = true;
                    viewModel.ShowSecond = false;
                    viewModel.ShowThree = false;
                    viewModel.ShowOne = false;
                    break;
                default:
                    break;
            }
        }

        private void FeedBackAlarmClick()
        {
            Services.EventCenter.PublishAlarm("FeedBackAlarm", viewModel.Jqlsh, "NO");
        }

        /// <summary>
        /// 监听消息
        /// </summary>
        /// <param name="obj"></param>
        private void OnGetEvent(Model.MessageModel model)
        {
            if (model == null)
            {
              //  EventCenter.PublishError("参数不能为空");
                return;
            }

            switch (model.MessageType)
            {
                case Model.MessageTypes.ENDALARM:
                    #region 参数验证

                    AlarmBase m = model.MessageBody as AlarmBase;
                    if (m == null)
                        return;
                    if (viewModel.Jqlsh != m.Jqlsh)
                        return;

                    #endregion
                    this.FinishInteraction();
                    break;
                case MessageTypes.ALARMRECIVE:
                case MessageTypes.ALARMARRIVE:
                case MessageTypes.ALARMCHANGE:
                case MessageTypes.ALARMSTART:
                case MessageTypes.NEWALARM:
                case MessageTypes.FEEDBACK:
                case MessageTypes.EXCUTEALARM:
                case MessageTypes.PDAFULFILL:
                case MessageTypes.OTHER:
                    #region 参数验证

                    AlarmBase alram = model.MessageBody as AlarmBase;
                    if (alram == null)
                        return;
                    if (viewModel.Jqlsh != alram.Jqlsh)
                        return;
                    #endregion
                    UtilsHelper.CopyEntity(viewModel, alram);
                    viewModel.Init();
                    break;
                case MessageTypes.GROUPSTATUS:
                case MessageTypes.GROUPOTHER:
                    viewModel.PatrolInit();
                    break;
                default:
                    break;
            }
        }
        public void JieAnWin()
        {
            EventCenter.PublishAlarm("CloseAlarm", viewModel.Jqlsh, "NO");
        }
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

        private string _RecordWord;

        public string RecordWord
        {
            get { return _RecordWord; }
            set
            {
                _RecordWord = value;
                OnPropertyChanged("RecordWord");
            }
        }

        private WorkingAlarmInfoViewModel _viewModel;

        public WorkingAlarmInfoViewModel viewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    OnPropertyChanged(() => viewModel);
                } 
            }
        }
        private string _zhiYinContent; 
        public string ZhiYinContent
        {
            get { return _zhiYinContent; }
            set
            {
                if (_zhiYinContent != value)
                {
                    _zhiYinContent = value;
                    OnPropertyChanged(() => ZhiYinContent);
                }
            }
        }
        private bool  _show;
        public bool Show
        {
            get { return _show; }
            set
            {
                if (_show != value)
                {
                    _show = value;
                    OnPropertyChanged(() => Show);
                }
            }
        }
        

        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
        public bool Confirmed { get; set; }

        public string Title { get; set; }

        public object Content { get; set; }

        public INotification Notification { get; set; }
        public Action Activate { get; set; }
        public Action FinishInteraction { get; set; }

        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// 警情结案
        /// </summary>
        public ICommand AlarmCloseCase { get; private set; }

        /// <summary>
        /// 反馈
        /// </summary>
        public ICommand FeedBackAlarm { get; set; }

        public ICommand JjAlarmCmd { get; set; }

        private void Cancel()
        {
            this.Confirmed = false;
            this.FinishInteraction();
        }
    }
}
