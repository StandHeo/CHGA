using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Pvirtech.Framework.Common;
using Pvirtech.Framework.Domain;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using Pvirtech.Modules.NormalAlarm.Views;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    /// <summary>
    /// 未完成警情列表
    /// </summary>
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class CompletedAlarmsViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IPoliceCaseRepository _policeCaseRepository;
        List<AlarmPrintViewModel> _popupwindowList = null;
        public CompletedAlarmsViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IPoliceCaseRepository policeCaseRepository)
        {
            _regionManager = regionManager;
            _container = container;
            _policeCaseRepository = policeCaseRepository;
            this._eventAggregator = eventAggregator;
            XiaQus = new ObservableCollection<DictItem>(LocalDataCenter.GetXiaQus());
            queryAlarm = new DelegateCommand<object>(QueryAlarm);
            advanceQueryAlarm = new DelegateCommand(ShowAdvanceAlarm);
            CmdPrintAlarm = new DelegateCommand<object>(PrintAlarm);
            _popupwindowList = new List<AlarmPrintViewModel>();
            lists = new ObservableCollection<CompletedAlarmInfoViewModel>();
            _alarmCollection = new ObservableCollection<CompletedAlarmInfoViewModel>();
            Application.Current.Dispatcher.BeginInvoke(new Action(() => OnLoad()), System.Windows.Threading.DispatcherPriority.Loaded);
            eventAggregator.GetEvent<CommonEventArgs>().Subscribe(OnGetEvent, ThreadOption.UIThread, true, (t) =>
            {
                return t.MessageType == MessageTypes.ENDALARM;
            });
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="obj"></param>
        private void OnGetEvent(MessageModel obj)
        {
            #region 参数验证

            AlarmBase ab = obj.MessageBody as AlarmBase;
            if (ab == null)
                return;

            #endregion

            CompletedAlarmInfoViewModel m = new CompletedAlarmInfoViewModel();
            UtilsHelper.CopyEntity(m, ab);
            Lists.Add(m);
            CompletedAlarmInfoViewModel alarm = null;
            if ((alarm = AlarmCollection.FirstOrDefault(a => a.Jqlsh == m.Jqlsh)) != null)
            {
                UtilsHelper.CopyEntity(alarm, ab);
            }
            else
            {
                AlarmCollection.Add(m);
            }
        }

        public string HeaderInfo
        {
            get { return "结案警情"; }
        }

        #region 页面数据

        private DictItem _SelectedXiaQu;

        /// <summary>
        /// 当前选中的辖区
        /// </summary>
        public DictItem SelectedXiaQu
        {
            get { return _SelectedXiaQu; }
            set
            {
                _SelectedXiaQu = value;
                OnPropertyChanged(() => SelectedXiaQu);
                //AlarmCollection=AlarmCollection.Filter = new Predicate<object>(FilterList);

                OnPropertyChanged("SelectedXiaQu");
                OnPropertyChanged("Models");
                GetCompleteAlarm();
            }
        }

        /// <summary>
        /// 当前选中的警情
        /// </summary>
        private CompletedAlarmInfoViewModel _SelectedAlarm;

        public CompletedAlarmInfoViewModel SelectedAlarm
        {
            get
            {
                return _SelectedAlarm;
            }
            set
            {
                if (_SelectedAlarm != value)
                {
                    _SelectedAlarm = value;
                    OnPropertyChanged(() => SelectedAlarm);
                }
            }
        }

        private ObservableCollection<DictItem> xiaQus;

        public ObservableCollection<DictItem> XiaQus
        {
            get { return xiaQus; }
            set
            {
                xiaQus = value;
                OnPropertyChanged(() => XiaQus);
            }
        }

        private ObservableCollection<CompletedAlarmInfoViewModel> _alarmCollection;

        public ObservableCollection<CompletedAlarmInfoViewModel> AlarmCollection
        {
            get
            {
                return _alarmCollection;
            }
            set
            {
                _alarmCollection = value;
                OnPropertyChanged(() => AlarmCollection);
            }
        }

        private string _FilterString;
        public string FilterString
        {
            get
            {
                return _FilterString;
            }
            set
            {
                if (_FilterString != value)
                {
                    _FilterString = value;
                }
            }
        }

        private ObservableCollection<CompletedAlarmInfoViewModel> lists;

        //<summary>
        //刷新数据
        //</summary>
        public ObservableCollection<CompletedAlarmInfoViewModel> Lists
        {
            get { return lists; }
            set
            {
                if (lists != value)
                {
                    lists = value;
                    OnPropertyChanged(() => Lists);
                }
            }
        }

        #endregion

        #region 方法

        private bool FilterList(object source1)
        {
            CompletedAlarmInfoViewModel source = source1 as CompletedAlarmInfoViewModel;
            if (_SelectedXiaQu != null && _SelectedXiaQu.detail != "成华分局" && source.Gxdwmc != _SelectedXiaQu.detail)
                return false;
            if (!string.IsNullOrEmpty(_FilterString)
                && !(source.Bjnr.IndexOf(_FilterString) >= 0
                  || source.Bjdh.IndexOf(_FilterString) >= 0
                  || source.Sfdz.IndexOf(_FilterString) >= 0)
                )
                return false;
            return true;
        }

        private async void GetCompleteAlarm(Dictionary<string, object> queryParam = null)
        {
            #region 拼接查询值

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("pageSize", GlobalConfig.GetInstance().AlarmFinishNumber.ToString());
            if (!string.IsNullOrEmpty(FilterString))
            {
                param.Add("bjnr", FilterString);
            }
            if (SelectedXiaQu != null && !SelectedXiaQu.code.Contains("510108000000"))
                param.Add("gxdwbh", SelectedXiaQu.code);
            string s = "";
            if (BeginselDate == null)
                s = System.DateTime.Now.ToString("yyyyMMdd") + "000000";
            else
                s = BeginselDate.Value.ToString("yyyyMMddHHmmss");

            param.Add("beginTime", s);
            if (endDate == null)
                s = System.DateTime.Now.ToString("yyyyMMdd") + "235959";
            else
                s = endDate.Value.ToString("yyyyMMddHHmmss");

            param.Add("endTime", s);

            #endregion

            #region 执行操作

            if (queryParam != null && queryParam.Count > 0)
            {
                foreach (var item in queryParam)
                {
                    param.Add(item.Key, item.Value);
                }
            }
            lists.Clear();
            IList<CompletedAlarmInfoViewModel> tmpList = new List<CompletedAlarmInfoViewModel>();
            Result<PageResult<AlarmBase>> result = await _policeCaseRepository.GetFinshAlarm(param);
            if (result.Code == ResultCode.SUCCESS)
            {
                PageResult<AlarmBase> page = result.Model;
                if (page != null && page.bean != null)
                {
                    for (int i = 0; i < page.bean.Count; i++)
                    {
                        var DomModel = new CompletedAlarmInfoViewModel();
                        UtilsHelper.CopyEntity(DomModel, page.bean[i]);
                        tmpList.Add(DomModel);
                    }
                }
               AlarmCollection =new ObservableCollection<CompletedAlarmInfoViewModel>(tmpList);
            }
            else
            {
                EventCenter.PublishError("加载警情异常！错误代码：" + result.Code + "\n" + result.Message);
            }

            #endregion
        }

        private DateTime? _BeginselDate;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginselDate
        {
            get { return _BeginselDate; }
            set
            {
                _BeginselDate = value;
                OnPropertyChanged(() => BeginselDate);
            }
        }

        private DateTime? _endDate;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(() => endDate);
            }
        }

        public void ShowAdvanceAlarm()
        {
            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    SelectConditionViewModel viewModel = new SelectConditionViewModel();
            //    viewModel.Title = "更多选择";
            //    viewModel.Content = new SelectCondition();
            //    PopupWindows.GetInstance().Raise(
            //    viewModel,
            //        sendMessage =>
            //        {
            //            var notification = sendMessage as SelectConditionViewModel;
            //            if (notification.Result != null)
            //            {
            //                GetCompleteAlarm(notification.Result);
            //            }
            //        });
            //}));
        }

        /// <summary>
        /// 显示打印单
        /// </summary>
        /// <param name="obj"></param>
        public async void PrintAlarm(object obj)
        {
            if (SelectedAlarm == null)
            { 
                return;
            }

            #region 打印单数据加载

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", SelectedAlarm.Jqlsh);
            Result<List<AlarmOperationRecord>> operRcd = await _policeCaseRepository.GetAlarmOperRcd(param);
            if (operRcd.Code == ResultCode.SUCCESS)
            {
                if (operRcd.Model != null)
                    SelectedAlarm.OperRecord = operRcd.Model;
            }
            Result<List<AlarmFeedBack>> feedBacks = await _policeCaseRepository.GetAlarmFeedBack(param);
            if (feedBacks.Code == ResultCode.SUCCESS)
            {
                if (feedBacks.Model != null)
                    SelectedAlarm.FeedBacks = feedBacks.Model;
            }

            #endregion

          await  Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            { 
                var item = _popupwindowList.Find(o => o.ViewModel != null && o.ViewModel.Jqlsh == SelectedAlarm.Jqlsh);
                if (item == null)
                {
                    AlarmPrintViewModel viewModel = new AlarmPrintViewModel(SelectedAlarm);
                    viewModel.Title = "打印单";
                    viewModel.Content = new AlarmPrint();
                    //ExcuteCaseViewModel viewModel = this._container.Resolve<ExcuteCaseViewModel>(new ParameterOverride("alarm", SelectedAlarm));
                    //viewModel.Title = "详细信息";
                    //viewModel.Content = new ExcuteCaseControl();
                    viewModel.ResizeMode = ResizeMode.CanMinimize;
                    _popupwindowList.Add(viewModel);
                    PopupWindows.GetInstance().Raise(viewModel, onCallBack =>
                    {
                        _popupwindowList.Remove(viewModel);
                    });
                }
                else
                {
                    //激活已打开的窗口
                    item.Activate();
                }
              
            }));
        }

        public void QueryAlarm(object send)
        {
            DictItem key = send as DictItem;
            GetCompleteAlarm();
        }
        #endregion

        #region CmdAlarm
        /// <summary>
        /// 查询警情
        /// </summary>
        private ICommand queryAlarm;
        public ICommand cmdQueryAlarm { get { return queryAlarm; } }

        private ICommand advanceQueryAlarm;
        public ICommand AdvanceQueryAlarm
        {
            get { return advanceQueryAlarm; }
        }
        /// <summary>
        /// 显示打印单
        /// </summary>
        public ICommand CmdPrintAlarm { get; set; }

      
        #endregion

        public void OnLoad()
        {
            GetCompleteAlarm(new Dictionary<string, object>());
            if (XiaQus.Count>0)
            {
                SelectedXiaQu = XiaQus[0];
            }
        
        }
    }
}
