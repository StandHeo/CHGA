using Microsoft.Practices.Unity;
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    /// <summary>
    /// 反馈窗口
    /// </summary>
    public class AddTicklingControlViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        public delegate string GetSeletecdWord(string bjlb,string bjlx,string bjxl);
        private UCAlarmTypeViewModel _UCalarmType;

        public UCAlarmTypeViewModel UCalarmType
        {
            get { return _UCalarmType; }
            set
            { 
                _UCalarmType = value;
                OnPropertyChanged(() => UCalarmType);
            }
        }
        public AlarmReplyIList AlarmReplys { get; set; }
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;

        public AddTicklingControlViewModel(IEventAggregator eventAggregator, IUnityContainer container, WorkingAlarmInfoViewModel alarm)            
        {
            _eventAggregator = eventAggregator;
            _container = container;
            _eventAggregator.GetEvent<CommonEventArgs>().Subscribe(OnGetEvent, ThreadOption.UIThread,true, (t) =>
            {
                return t.MessageType == MessageTypes.ENDALARM;
            });
            this.FeedSaveCommand = new DelegateCommand(this.FeedBackAlarm);
            this.CancelCommand = new DelegateCommand(this.Cancel);
            this.JieAnCommand = new DelegateCommand(JieAnWin);
            this.okFknr = new DelegateCommand(GetFknr);
            Words = new ObservableCollection<string>();
            OnLoadData(alarm);
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
                    if (ThisModel.Jqlsh != m.Jqlsh)
                        return;

                    #endregion
                    this.FinishInteraction();
                    break;
                default:
                    break;
            }
        }

        void _UCalarmType_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectWord")
            {
                TrimFknr(UCalarmType.SelectWord);
            }
        }

        public void OnLoadData(WorkingAlarmInfoViewModel alarm)
        { 
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                GetAlarmReply.AlarmReplys = XmlHelper.GetXmlEntities<AlarmReplyIList>("Alarm.PhraseWord.xml");
                Patrol p = new Patrol();
                p.GroupName = "成华分局";
                p.GroupNo = "510108000000";
                ComServer = new List<Patrol>();
                ComServer.Add(p);
                SelectPatrols = ComServer[0];
                ThisModel = alarm;
                UCalarmType = new UCAlarmTypeViewModel();
                UCalarmType.getSeletecdWord = new GetSeletecdWord(UCalarmType.UpWordsByNo);
                _UCalarmType.PropertyChanged += _UCalarmType_PropertyChanged;
                LoadSet();
            }),System.Windows.Threading.DispatcherPriority.Loaded);
        }

        #region 界面ComboBox 数据

        public void LoadSet()
        {
            if (ThisModel != null)
            {
                #region 初始化类型/细类

                Fknr = "";
                BjlbDict = new ObservableCollection<DictItem>(LocalDataCenter.GetBjlb());
                BjlxDict = new ObservableCollection<DictItem>(LocalDataCenter.GetBjlx().Where(M => M.superCode == ThisModel.Bjlb).ToList());
                BjxlDict = new ObservableCollection<DictItem>(LocalDataCenter.GetBjxl().Where(M => M.superCode == ThisModel.Bjlx).ToList());
                SelectBjlb = BjlbDict.FirstOrDefault(M => M.code == ThisModel.Bjlb);
                SelectBjlx = BjlxDict.FirstOrDefault(M => M.code == ThisModel.Bjlx);
                SelectBjxl = BjxlDict.FirstOrDefault(M => M.code == ThisModel.Bjxl);
                ThisModel.Init();
                if (ThisModel.FdBacklit != null && ThisModel.FdBacklit.Count > 0)
                {
                    foreach (var item in ThisModel.FdBacklit)
                    {
                        if (item.PcOrPDA == 1)
                        {
                            string sjtext = "-  -";
                            if (!string.IsNullOrEmpty(item.Fksj))
                                sjtext = DateTime.ParseExact(item.Fksj, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy:MM:dd HH:mm:ss");
                            LogInfo += "【反馈单位】" + item.Cjdwmz + "【反馈时间】" + sjtext + ":" + item.Fknr + "\r\n";
                        }
                    }
                }
                
                #endregion
            }
        }

        public void JieAnWin()
        {
            //EventCenter.PublishAlarm("AlarmHintWin", ThisModel.Jqlsh, "NO");
            
            //EventCenter.PublishAlarm("ErrorHandleWin", ThisModel.Jqlsh, "NO");
            EventCenter.PublishAlarm("CloseAlarm", ThisModel.Jqlsh, "NO");

            //todo：重大警情 调用信息流转中心发送消息的方法 上报市局
            if (ThisModel.Jqly == 3)
            {
                StringBuilder info = new StringBuilder();
                info.Append("成华指挥室已反馈：");
                info.Append(Fknr);
                info.Append(DateTime.Now.ToString("【时间：yyyy-MM-dd HH:mm:ss】"));
                OnSendMsgCmd(ThisModel.Jjdbh, info.ToString());
            }
            //#region 参数验证

            //if (ThisModel == null)
            //{
            //    EventCenter.PublishError("参数不能为空");
            //    return;
            //}

            //#endregion
            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    CloseCaseControlViewModel win = this._container.Resolve<CloseCaseControlViewModel>(new ParameterOverride("alarm", ThisModel));
            //    win.Title = "结案";
            //    win.Content = new CloseCaseControl();
            //    PopupWindows.GetInstance().Raise(
            //     win,
            //         sendMessage =>
            //         {
            //             var notification = sendMessage as CloseCaseControlViewModel;
            //             if (notification.Confirmed)
            //             {
            //                // PopupWindows.ShowWinMessage(notification.NotifyMessage);
            //                 EventCenter.PublishPrompt("结案成功"); 
            //                 this.FinishInteraction();
            //             }
            //         });
            //}));
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

        public ICommand JieAnCommand
        {
            get;
            set;
        }

        public WorkingAlarmInfoViewModel ThisModel { get; set; }

        private Patrol _SelectPatrols;

        /// <summary>
        /// 当前选中的单位
        /// </summary>
        public Patrol SelectPatrols
        {
            get { return _SelectPatrols; }
            set
            {
                _SelectPatrols = value;
                OnPropertyChanged(() => SelectPatrols);
            }
        }

        private List<Patrol> _ComServer;
    
        /// <summary>
        /// 单位数据
        /// </summary>
        public List<Patrol> ComServer
        {
            get { return _ComServer; }
            set
            { 
                _ComServer = value;
                OnPropertyChanged(() => ComServer);
            }
        }

        #region 当前选中的对象

        private string selectWord;

        public string SelectWord
        {
            get { return selectWord; }
            set
            {
                selectWord = value;
                OnPropertyChanged("SelectWord");
                //TrimFknr(SelectWord);
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
                GetBjlxByBjlbId(SelectBjlb);
                GetKjhfWord(SelectBjlb);
                BjxlDictClear();
            }
        }

        private DictItem _SelectBjlx;

        /// <summary>
        /// 当前选中的类型
        /// </summary>
        public DictItem SelectBjlx
        {
            get { return _SelectBjlx; }
            set
            {
                if (value != null)
                {
                    _SelectBjlx = value;
                    BjxlDictClear();
                    if (value.code != "-1")
                    {
                        GetBjxlByBjlbId(SelectBjlx);
                        GetKjhfWord(SelectBjlx);
                    }
                    OnPropertyChanged(() => SelectBjlx);
                }
            }
        }

        private DictItem _SelectBjxl;

        /// <summary>
        /// 当前选中的细类
        /// </summary>
        public DictItem SelectBjxl
        {
            get { return _SelectBjxl; }
            set
            {
                if (value != null)
                {
                    _SelectBjxl = value;
                    if (value.code != null && value.code != "-1")
                        GetKjhfWord(SelectBjlx);
                    else
                    OnPropertyChanged(() => SelectBjxl);
                }
            }
        }

        #endregion

        #region 页面数据

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


        private ObservableCollection<DictItem> _BjlxDict;

        /// <summary>
        /// 报警类型
        /// </summary>
        public ObservableCollection<DictItem> BjlxDict
        {
            get { return _BjlxDict; }
            set
            {
                if (_BjlxDict != value)
                {
                    _BjlxDict = value;
                    OnPropertyChanged(() => BjlxDict);
                }
            }
        }

        private ObservableCollection<DictItem> _BjxlDict;

        /// <summary>
        /// 报警细类
        /// </summary>
        public ObservableCollection<DictItem> BjxlDict
        {
            get { return _BjxlDict; }
            set
            {
                _BjxlDict = value;
                OnPropertyChanged(() => BjxlDict);
            }
        }

        public User LoginUser
        {
            get { return LocalDataCenter.GetLocalUser(); }
        }

        private string _Fknr;

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Fknr
        {
            get { return _Fknr; }
            set
            {
                if (null != value)
                {
                    _Fknr = value;
                    OnPropertyChanged(() => Fknr);
                }
                else
                    _Fknr = "";
            }
        }

        private string _Saje;

        /// <summary>
        /// 涉案金额
        /// </summary>
        public string Saje
        {
            get { return _Saje; }
            set
            {
                _Saje = value;
                OnPropertyChanged(() => Saje);
            }
        }

        private ObservableCollection<string> words;

        /// <summary>
        /// 快捷回复集合
        /// </summary>
        public ObservableCollection<string> Words
        {
            get { return words; }
            set 
            { 
                words = value;
                OnPropertyChanged(() => Words);
            }
        }
        
        //private List<AlarmPhrase> wordlit;

        //public List<AlarmPhrase> Wordlit
        //{
        //    get { return wordlit; }
        //    set
        //    {
        //        wordlit = value;
        //        OnPropertyChanged(()=>Wordlit);
        //    }
        //}

        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 匹配快捷语句
        /// </summary>
        /// <param name="obj"></param>
        public void GetKjhfWord(DictItem obj)
        {
            //报警类别
            string bjlbDesc = SelectBjlb == null ? "" : SelectBjlb.code;

            //报警类型
            string bjlxDesc = SelectBjlx == null ? "" : SelectBjlx.code;
            if (!string.IsNullOrEmpty(bjlxDesc) && bjlxDesc.Contains("-1"))
	        {
		        bjlxDesc = null; 
	        }

            //报警细类
            string bjxlDesc = SelectBjxl == null ? "" : SelectBjxl.code;
            if (!string.IsNullOrEmpty(bjxlDesc) && bjxlDesc.Contains("-1"))
	        {
		        bjxlDesc = null;
	        }

            TrimFknr(UCalarmType.getSeletecdWord(bjlbDesc, bjlxDesc, bjxlDesc));
            OnPropertyChanged("UCalarmType");
        }

        /// <summary>
        /// 确认反馈内容
        /// </summary>
        private void GetFknr()
        {
            Fknr = SelectWord;
        }

        /// <summary>
        /// 获取报警类型
        /// </summary>
        /// <param name="obj"></param>
        private void GetBjlxByBjlbId(DictItem obj)
        {
            #region 参数验证

            if (obj == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            BjlxDict.Clear();
            List<DictItem> GetBjlxs = LocalDataCenter.GetBjlx().Where(M => M.superCode == obj.code).ToList();
            if (GetBjlxs != null && GetBjlxs.Count > 0)
            {
                DictItem addItem;
                foreach (var item in GetBjlxs)
                {
                    addItem = new DictItem();
                    UtilsHelper.CopyEntity(addItem, item);
                    BjlxDict.Add(addItem);
                }
            }
            DictItem d = new DictItem();
            d.code = "-1";
            d.note = "-请选择-";
            BjlxDict.Insert(0, d);
            SelectBjlx = BjlxDict[0];
        }

        /// <summary>
        /// 加载报警细类
        /// </summary>
        /// <param name="bjlxNumber"></param>
        public void GetBjxlByBjlbId(DictItem obj)
        {
            #region 参数验证

            if (obj == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            BjxlDict.Clear();
            List<DictItem> GetBjxls = LocalDataCenter.GetBjxl().Where(M => M.superCode == obj.code).ToList();
            if (GetBjxls != null && GetBjxls.Count > 0)
            {
                DictItem addItem;
                foreach (var item in GetBjxls)
                {
                    addItem = new DictItem();
                    UtilsHelper.CopyEntity(addItem, item);
                    BjxlDict.Add(addItem);
                }
               
            }
            DictItem d = new DictItem();
                d.code = "-1";
                d.note = "-请选择-";
                BjxlDict.Insert(0, d);
                SelectBjxl = BjxlDict[0];
        }

        /// <summary>
        /// 根据选择的回复语音生产语句
        /// </summary>
        /// <param name="v"></param>
        public void TrimFknr(string v)
        {
            Fknr = v;
        }

        /// <summary>
        /// 清空报警细类
        /// </summary>
        public void BjxlDictClear()
        {
            if (BjxlDict != null)
                BjxlDict = new ObservableCollection<DictItem>();
        }

        /// <summary>
        /// 加载快捷回复措施
        /// </summary>
        //public void GetXMLWord()
        //{
        //    XmlDocument doc = XmlHelper.GetXmlDoc("Alarm.PhraseWord.xml");
        //    XmlElement rootElem = doc.DocumentElement;   
        //    XmlNodeList personNodes = rootElem.GetElementsByTagName("Key");  
        //    AlarmPhrase model;
        //    List<string> words = new List<string>();

        //    //编译警告注释 20151211 BINGLE
        //    //string s;

        //    foreach (var node in personNodes)
        //    {
        //        model = new AlarmPhrase();
        //        model.BjlbWord= new List<string>();
        //        model.Type = ((XmlElement)node).GetAttribute("key");   
        //        XmlNodeList subAgeNodes = ((XmlElement)node).GetElementsByTagName("Word");
        //        if (subAgeNodes.Count > 0)
        //        {
        //            foreach (XmlElement item in subAgeNodes)
        //            {
        //                model.BjlbWord.Add(item.InnerText);
        //            }
        //        }
        //        Wordlit.Add(model);
        //    }
        //}

        /// <summary>
        /// 反馈警情
        /// </summary>
        /// <param name="jams"></param>
        private async void FeedBackAlarm()
        {
            #region 参数验证

            if (SelectPatrols == null)
            {
                return;
            }
            Fknr=Fknr.Trim();
            if (string.IsNullOrEmpty(Fknr))
            {
                EventCenter.PublishError("反馈内容不能为空");
                return;
            }
            if (SelectBjlx == null || SelectBjlx.code=="-1")
            {
                EventCenter.PublishError("请选择类型");
                return;
            }

            #endregion

            Result<bool> result = await ThisModel.FeedBackMession(Fknr, SelectPatrols.GroupNo, SelectPatrols.GroupName,
                Saje == null ? "" : Saje, SelectBjxl != null ? SelectBjxl.code : "", SelectBjlb != null ? SelectBjlb.code : ""
                , SelectBjlx != null ? SelectBjlx.code : "", 1);
            if (result.Code == ResultCode.SUCCESS)
            {
                if (result.Model)
                {
                    NotifyMessage = "反馈成功！";
                    this.Confirmed = true;
                    this.FinishInteraction();
                }
                else
                {
                    NotifyMessage = "反馈失败！";
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

        #region 接口命令/页面扩展字段

        private string logInfo;

        public string LogInfo
        {
            get { return logInfo; }
            set
            {
                logInfo = value;
                OnPropertyChanged("LogInfo");
            }
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
                    OnPropertyChanged(() => NotifyMessage);
                }
            }
        }

        public bool Confirmed { get; set; }
        public bool IsModel { get; set; }
        public string Title { get; set; }

        public object Content { get; set; }
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public INotification Notification { get; set; }
        public bool IsModal { get; set; }
        public Action FinishInteraction { get; set; }
        public Action Activate { get; set; }

        /// <summary>
        /// 选中事件
        /// </summary>
        public ICommand SelectModelClick { get; private set; }

        /// <summary>
        /// 反馈保存命令
        /// </summary>
        public ICommand FeedSaveCommand { get; private set; }
        /// <summary>
        /// 取消命令
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        private ICommand okFknr;

        public ICommand OkFknr
        {
            get { return okFknr; }
        }

        #endregion

    }
}
