using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Pvirtech.Framework.Common;
using Pvirtech.Framework.Domain;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static Pvirtech.Modules.NormalAlarm.ViewModels.PopUp.AddTicklingControlViewModel;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class CloseCaseControlViewModel : BindableBase,IConfirmation, IInteractionRequestAware
    {
        
        private readonly IEventAggregator _eventAggregator;
        //private readonly ICommonRepository _commonRepository;
        
        public CloseCaseControlViewModel(WorkingAlarmInfoViewModel alarm, IEventAggregator eventAggregator) 
          {
              _eventAggregator = eventAggregator;
            //ViewModel = alarm;
            this.OKCommand = new DelegateCommand<object>(this.CloseMession);
            this.CancelCommand = new DelegateCommand(this.Cancel);
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                //GetAlarmReply.AlarmReplys = XmlHelper.GetXmlEntities<AlarmReplyIList>("Alarm.PhraseWord.xml");
                ViewModel = alarm;
                UCAlarmTypevViewModel = new UCAlarmTypeViewModel();
                UCAlarmTypevViewModel.getSeletecdWord = new GetSeletecdWord(UCAlarmTypevViewModel.UpWordsByNo);
                UCAlarmTypevViewModel.PropertyChanged += UCAlarmTypevViewModel_PropertyChanged;
                Word = new ObservableCollection<string>();
                viewModel.Init();
                LoadSet();
                // ViewModel.Jams = UCAlarmTypevViewModel.getSeletecdWord(ViewModel.Bjlb, ViewModel.Bjlx, ViewModel.Bjxl);
                
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        void UCAlarmTypevViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="SelectWord")
            {
                this.ViewModel.Jams = UCAlarmTypevViewModel.SelectWord;
            }
        }
  
        public void LoadSet()
        {
            BjlbDict = new ObservableCollection<DictItem>(LocalDataCenter.GetBjlb());
            SelectBjlb = BjlbDict.FirstOrDefault(B=>B.code == ViewModel.Bjlb);
            if (ViewModel != null && ViewModel.FdBacklit.Count > 0)
            {
                foreach (var item in ViewModel.FdBacklit)
                {
                    string sjtext = "- -";
                    if (!string.IsNullOrEmpty(item.Fksj))
                        sjtext = DateTime.ParseExact(item.Fksj, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy:MM:dd HH:mm:ss");
                    FaKuiWord += "【反馈单位】" + item.Cjdwmz + "【时间】" + sjtext + ":" + item.Fknr + "\r\n";
                }
                ViewModel.Jams = ViewModel.FdBacklit.OrderBy(B => B.Fksj).Last().Fknr;
            }
        }

        /// <summary>
        /// 报警类别
        /// </summary>
        private ObservableCollection<DictItem> bjlbDict;

        public ObservableCollection<DictItem> BjlbDict
        {
            get { return bjlbDict; }
            set
            {
                bjlbDict = value;
                OnPropertyChanged(() => BjlbDict);
            }
        }

        private DictItem _SelectBjlb;
        /// <summary>
        /// 当前选中的类别
        /// </summary>
        public DictItem SelectBjlb
        {
            get { return _SelectBjlb; }
            set
            {
                _SelectBjlb = value;
                OnPropertyChanged(() => SelectBjlb);
                GetKjhfWord(SelectBjlb);
            }
        }

        private ObservableCollection<string> word;

        public ObservableCollection<string> Word
        {
            get { return word; }
            set
            {
                word = value;
                OnPropertyChanged(() => Word);
            }
        }

        private string selectWord;

        public string SelectWord
        {
            get { return selectWord; }
            set
            {
                selectWord = value;
                OnPropertyChanged("SelectWord");
                TrimFknr(SelectWord);
            }
        }

        /// <summary>
        /// 根据选择的回复语音生产语句
        /// </summary>
        /// <param name="v"></param>
        public void TrimFknr(string v)
        {
            ViewModel.Jams = v;
        }

        public void GetKjhfWord(DictItem obj)
        {
            //#region 参数验证

            //if (obj == null)
            //{
            //    EventCenter.PublishError("参数不能为空");
            //    return;
            //}
            //if (Wordlit == null)
            //    return;

            //#endregion

            //Word.Clear();
            //AlarmPhrase m = Wordlit.FirstOrDefault(W => W.Type == obj.code);
            //if (m != null)
            //{
            //    foreach (var item in m.BjlbWord)
            //    {
            //        Word.Add(item);
            //    }
            //    SelectWord = Word[0];
            //}
        }
        
        /// <summary>
        /// 提示信息
        /// </summary>
        private string notifyMessage;
       
        public string NotifyMessage {

            get
            {
                return notifyMessage;
            }
            set
            {
                if (value!=notifyMessage)
                {
                    notifyMessage = value;
                    OnPropertyChanged(() => NotifyMessage);
                }
            }
        }

        private string faKuiWord;

        public string FaKuiWord
        {
            get { return faKuiWord; }
            set
            { 
                faKuiWord = value;
                OnPropertyChanged(() => FaKuiWord);
            }
        }

        private WorkingAlarmInfoViewModel viewModel;

        public WorkingAlarmInfoViewModel ViewModel
        {
            get { return viewModel; }
            set 
            {
                if (viewModel != value)
                {
                    viewModel = value;
                    OnPropertyChanged(() => ViewModel);
                }             
            }
        }

        private List<object> cmdparams;
        public List<object> CmdParams
        {
           get
            {
                return cmdparams;
            }
            set
            {
                if (value != cmdparams)
                {
                    cmdparams = value;
                    OnPropertyChanged(() => CmdParams);
                }
            }
        }

        private UCAlarmTypeViewModel _UCAlarmTypevViewModel;

        public UCAlarmTypeViewModel UCAlarmTypevViewModel
        {
            get { return _UCAlarmTypevViewModel; }
            set
            {
                _UCAlarmTypevViewModel = value;
                OnPropertyChanged(() => UCAlarmTypevViewModel);
            }
        }

        public bool Confirmed { get; set; }
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
        public string Title { get; set; }
        public object Content { get; set; }

        public INotification Notification { get; set; }
        public Action Activate { get; set; }
        public Action FinishInteraction { get; set; }

        public ICommand OKCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        private async void CloseMession(object jams)
        {
            #region 参数验证

            if (ViewModel == null||string.IsNullOrEmpty(ViewModel.Jams))
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }
            viewModel.Jams = viewModel.Jams.Trim();
            if (string.IsNullOrEmpty(ViewModel.Jams))
            {
                EventCenter.PublishError("结案描述不能为空");
                return;
            }
            #endregion
            
            UtilsHelper.LogTexts("1111");
            Result<bool> result = await ViewModel.CloseMession(ViewModel.Jams);
            if (result.Code == ResultCode.SUCCESS)
            {
                if (result.Model)
                {
                    UtilsHelper.LogTexts("3333");
                    EventCenter.PublishPrompt("操作成功");
                    this.Confirmed = true;
                    this.FinishInteraction();

                    //todo：重大警情 调用信息流转中心发送消息的方法 上报市局
                    if (ViewModel.Jqly == 3)
                    {
                        StringBuilder info = new StringBuilder();
                        info.Append("成华指挥室已结案：");
                        info.Append(viewModel.Jams);
                        info.Append(DateTime.Now.ToString("【时间：yyyy-MM-dd HH:mm:ss】"));
                        OnSendMsgCmd(ViewModel.Jjdbh, info.ToString());
                    }
                }
                else
                    EventCenter.PublishError("操作失败");
            }
            else 
            {
                EventCenter.PublishError("操作失败");
                LogHelper.ErrorLog(null,"结案操作失败！代码："+result.Code+" "+result.Message);
            }
        }

        //两个参数 事件编号  信息内容
        private void OnSendMsgCmd(string bh, string info)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("jjdbh", bh);
            dic.Add("info", info);
            var eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.
                GetInstance<IEventAggregator>();
            if (eventAggregator != null)
                eventAggregator.GetEvent<ZDJQSendAlarmEvent>().Publish(dic);
        }

        private void Cancel()
        {
            this.Confirmed = false;
            this.FinishInteraction();
        }
    }
}
