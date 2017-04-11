using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Pvirtech.Framework.Common;
using Pvirtech.Framework.Controls;
using Pvirtech.Framework.Domain;
using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    /// <summary>
    /// 未完成警情列表
    /// </summary>

    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class WorkingAlarmsViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private int i = 0;
        public WorkingAlarmsViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator)
        {

            _regionManager = regionManager;
            _container = container;
            this._eventAggregator = eventAggregator;
            updateSetCommand = new Prism.Commands.DelegateCommand(UpDataSet);
            listRightMenue = new DelegateCommand<object>(RightMenueClick);
            resetSeek = new Prism.Commands.DelegateCommand(ClearSeek);
            countDate = new Prism.Commands.DelegateCommand(alarmCountDate);
            eventAggregator.GetEvent<CommonEventArgs>().Subscribe(OnGetEvent, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<TimeSpanEventArgs>().Subscribe(OnGetTimeSpanEvent, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<BigLinkageStatusChangedArgs>().Subscribe(OnBigLinkageStatusChanged, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<BigLinkageProgressBarVisibilityArgs>().Subscribe(OnProgressBarVisibilityChanged, ThreadOption.UIThread, true);

            dblclick = new Prism.Commands.DelegateCommand(AlarmInfoWin);
            Menu = new ObservableCollection<MenuItem>();
            lists = new ObservableCollection<WorkingAlarmInfoViewModel>();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                UpDataSet();
                XiaQus = new ObservableCollection<DictItem>(LocalDataCenter.GetXiaQus());
                if (XiaQus.Count > 0)
                {
                    SelectedXiaQu = XiaQus[0];
                }

            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }


        private int m_timeoutWaitingCount = 0;

        private void OnProgressBarVisibilityChanged(Tuple<string, Visibility> val)
        {
            WaitingWindow wd = WaitingWindow.Instanace;
            wd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wd.Owner = Application.Current.MainWindow;
            if (val.Item2 == Visibility.Collapsed)
            {
                if (m_timeoutWaitingCount <= 1)
                {
                    wd.Hide();
                }
                m_timeoutWaitingCount--;
            }
            else if (val.Item2 == Visibility.Visible)
            {
                m_timeoutWaitingCount++;
                wd.Show();
            }

        }

        private void OnBigLinkageStatusChanged(KeyValuePair<string, int> vals)
        {
            if (!vals.Equals(default(KeyValuePair<string, string>)))
            {
                var jqlsh = vals.Key;
                var status = vals.Value;
                var item = Lists.FirstOrDefault(a => a.Jqlsh == jqlsh);
                if (item != null)
                {
                    item.DldStatus = status;
                }
            }
        }

        private void OnGetTimeSpanEvent(long obj)
        {
            i++;
            if (i >= 1)
            {
                i = 0;
                ListCountDate();
            }
            for (int index = 0; index < LocalDataCenter.lstSendAlarmTime.Count; index++)
            {
                if (LocalDataCenter.lstSendAlarmTime[index].Count == 20)
                {
                    EventCenter.PublishAlarmPrompt("AlarmSendFailed", LocalDataCenter.lstSendAlarmTime[index].Jqlsh, LocalDataCenter.lstSendAlarmTime[index].GroupNo);
                }
                LocalDataCenter.lstSendAlarmTime[index].Count += 1;
            }
        }

        #region   页面数据

        #region 页面状态

        private Visibility _progressVisibility = Visibility.Collapsed;

        public Visibility ProgressVisibility
        {
            get
            {
                return _progressVisibility;
            }
            set
            {
                _progressVisibility = value;
                OnPropertyChanged(() => ProgressVisibility);
            }
        }

        private string wcjDate;

        public string WcjDate
        {
            get { return wcjDate; }
            set
            {
                wcjDate = value;
                OnPropertyChanged("WcjDate");
            }
        }

        private string cjDate;

        public string CjDate
        {
            get { return cjDate; }
            set
            {
                cjDate = value;
                OnPropertyChanged("CjDate");
            }
        }

        private string cjzDate;

        public string CjzDate
        {
            get { return cjzDate; }
            set
            {
                cjzDate = value;
                OnPropertyChanged("CjzDate");
            }
        }

        private string daoDaDate;

        public string DaoDaDate
        {
            get { return daoDaDate; }
            set
            {
                daoDaDate = value;
                OnPropertyChanged(() => DaoDaDate);
            }
        }

        #endregion

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

        private DictItem _SelectedXiaQu;

        public DictItem SelectedXiaQu
        {
            get
            {
                return _SelectedXiaQu;
            }
            set
            {
                if (_SelectedXiaQu != value)
                {
                    _SelectedXiaQu = value;
                    _Models.Filter = new Predicate<object>(FilterList);
                    OnPropertyChanged("SelectedXiaQu");
                    OnPropertyChanged("Models");
                }
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
                    OnPropertyChanged("FilterString");
                    _Models.Filter = new Predicate<object>(FilterList);
                    OnPropertyChanged("Models");
                }
            }
        }

        public string HeaderInfo
        {
            get
            {
                return "警情处理";
            }
        }

        public ObservableCollection<MenuItem> Menu
        {
            get;
            set;
        }

        private ICollectionView _Models;

        /// <summary>
        /// 列表数据
        /// </summary>
        public ICollectionView Models
        {
            get
            {
                return _Models;
            }
            set
            {
                _Models = value;
                OnPropertyChanged("Models");
            }
        }

        /// <summary>
        /// 当前选中的对象
        /// </summary>
        private WorkingAlarmInfoViewModel _SelectedAlarm;

        public WorkingAlarmInfoViewModel SelectedAlarm
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

        #endregion

        #region 方法

        public void ListCountDate()
        {
            #region 参数验证

            if (Lists == null || Lists.Count <= 0)
            {
                return;
            }

            #endregion

            #region 执行操作

            lock (Lists)
            {
                foreach (var item in Lists)
                {
                    if (!string.IsNullOrEmpty(item.Zhgxsj))
                    {
                        long l = 0;
                        long.TryParse(item.Zhgxsj.ToString(), out l);
                        if (!item.IsUrge && item.Ajzt <= 5 && item.Ajzt > 9 && l <= GlobalConfig.GetInstance().Urge)
                        {
                            /*
                             * 是否催促
                             *催促时间是否已到
                             */
                            MessageModel one = new MessageModel();
                            one.MessageBody = item;
                            one.MessageType = MessageTypes.ALARMHASURGE;
                            _eventAggregator.GetEvent<CommonEventArgs>().Publish(one);
                            item.IsUrge = true;
                        }
                        else
                        {
                            /*
                             * 总计时
                             * 阶段计时
                             */
                            item.ThisStageDate = UtilsHelper.DateDistance(DateTime.Now, DateTime.ParseExact(item.Zhgxsj, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture));
                            item.TotalDate = UtilsHelper.DateDistance(DateTime.Now, DateTime.ParseExact(item.Rcsjc, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture));
                        }
                    }
                }
            }
            #endregion
        }

        public void alarmCountDate()
        {
            #region 参数验证

            if (SelectedAlarm == null)
            {
                return;
            }

            #endregion

            long v = (long)((DateTime.Now - DateTime.ParseExact(SelectedAlarm.Rcsjc, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture)).TotalSeconds);
            string val = UtilsHelper.formatLongToTimeStr(v);
        }

        private void AlarmInfoWin()
        {
            #region 参数验证

            if (SelectedAlarm == null)
            {
                EventCenter.PublishError("请选中警情");
                return;
            }

            #endregion

            EventCenter.PublishAlarm("AlarmDetail", SelectedAlarm.Jqlsh, "NO");
        }

        private ObservableCollection<WorkingAlarmInfoViewModel> lists;

        //<summary>
        //刷新数据
        //</summary>
        public ObservableCollection<WorkingAlarmInfoViewModel> Lists
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

        public void ClearSeek()
        {
            FilterString = null;
            SelectedXiaQu = LocalDataCenter.GetXiaQus().FirstOrDefault(M => M.detail == "成华分局");
            lock (lists)
            {
                Lists.Clear();
                List<AlarmBase> alarms = new List<AlarmBase>(LocalDataCenter.GetAlarmCollections()).OrderByDescending(m => m.Jjsj).ToList();
                for (int i = 0; i < alarms.Count; i++)
                {
                    var DomModel = new WorkingAlarmInfoViewModel();
                    UtilsHelper.CopyEntity(DomModel, alarms[i]);
                    Lists.Add(DomModel);
                }
            }
        }

        private void UpDataSet()
        {
            #region 刷新数据

            Lists = new ObservableCollection<WorkingAlarmInfoViewModel>();
            List<AlarmBase> alarms = new List<AlarmBase>(LocalDataCenter.GetAlarmCollections()).OrderByDescending(m => m.Jjsj).ToList();
            for (int i = 0; i < alarms.Count; i++)
            {
                var DomModel = new WorkingAlarmInfoViewModel();
                UtilsHelper.CopyEntity(DomModel, alarms[i]);
                Lists.Add(DomModel);
            }
            Models = new ListCollectionView(lists);

            #endregion
        }

        private bool FilterList(object source1)
        {
            if (source1 == null)
            {
                return false;
            }
            MAlarmInfo source = source1 as MAlarmInfo;
            if (source == null)
            {
                return false;
            }
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

        private ICommand listRightMenue;

        public ICommand ListRightMenue
        {
            get { return listRightMenue; }
        }

        private ICommand countDate;

        public ICommand CountDate
        {
            get { return countDate; }
        }

        /// <summary>
        /// 列表菜单事件
        /// </summary>
        /// <param name="obj"></param>
        private void RightMenueClick(object obj)
        {
            Menu.Clear();
            var clickedItem = (obj as MouseButtonEventArgs).OriginalSource as FrameworkElement;
            if (clickedItem != null)
            {
                var parentRow = clickedItem.ParentOfType<GridViewRow>();
                if (parentRow != null)
                    parentRow.IsSelected = true;
                if (SelectedAlarm != null)
                {
                    SelectedAlarm.Init();

                    #region   处警右键功能

                    foreach (AlarmPatrolCmdItem one in AlarmCommands.OperItems.CmdItems)
                    {
                        MenuItem mi = new MenuItem();
                        mi.Header = one.Name;
                        mi.Command = one.BindCommand;
                        mi.CommandParameter = one.Key + "," + SelectedAlarm.Jqlsh;
                        Menu.Add(mi);
                    }

                    #endregion

                    foreach (MAlarmPatrol one in SelectedAlarm.UsePatrols.Patrols)
                    {
                        MenuItem xzItem = new MenuItem();

                        xzItem.Header = one.patrol != null ? one.patrol.GroupName : string.Empty;
                        foreach (var nitem in one.Cmds)
                        {
                            MenuItem cmd = new MenuItem();
                            cmd.Command = nitem.BindCommand;
                            cmd.CommandParameter = nitem.Key;
                            cmd.Header = nitem.Name;
                            xzItem.Items.Add(cmd);
                        }
                        if (xzItem.HasItems)
                        {
                            Menu.Add(xzItem);
                        }

                    }
                }
            }
        }

        private void OnGetEvent(MessageModel obj)
        {
            #region 参数验证

            if (obj == null)
            {
                return;
            }

            #endregion 
            lock (this)
            {
                switch (obj.MessageType)
                {
                    case MessageTypes.ALARMRECIVE:
                        ChangeAlarm(obj);
                        RemoveAlarmTime(obj.MessageBody);
                        break;
                    case MessageTypes.ALARMARRIVE:
                    case MessageTypes.ALARMCHANGE:
                    case MessageTypes.ALARMSTART:
                    case MessageTypes.NEWALARM:
                    case MessageTypes.FEEDBACK:
                    case MessageTypes.EXCUTEALARM:
                    case MessageTypes.PDAFULFILL:
                    case MessageTypes.OTHER:
                        ChangeAlarm(obj);
                        break;
                    case MessageTypes.ENDALARM:
                        DeleteAlarm(obj);
                        break;
                    //case MessageTypes.GROUPSTATUS:
                    //case MessageTypes.GROUPOTHER:
                    //    UpDateList();
                    //    break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 清除派警计时
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveAlarmTime(object obj)
        {
            var alarm = obj as AlarmBase;
            if (alarm != null)
            {
                var findItem = LocalDataCenter.lstSendAlarmTime.FirstOrDefault(o => o.Jqlsh == alarm.Jqlsh);
                if (findItem != null)
                {
                    LocalDataCenter.lstSendAlarmTime.Remove(findItem);
                }
            }
        }

        /// <summary>
        /// 单兵/巡组信息变更
        /// </summary>
        public void UpDateList()
        {
            #region 执行操作

            foreach (var item in lists)
            {
                item.Init();
            }

            #endregion
        }

        /// <summary>
        /// 数据变更
        /// </summary>
        /// <param name="obj"></param>
        public void ChangeAlarm(MessageModel obj)
        {
            if (obj.MessageBody is AlarmBase)
            {
                AlarmBase m = obj.MessageBody as AlarmBase;
                if (m == null)
                {
                    return;
                }
                WorkingAlarmInfoViewModel litModel = lists.FirstOrDefault(M => M.Jqlsh == m.Jqlsh);
                if (litModel == null)
                {
                    WorkingAlarmInfoViewModel mainfo = new WorkingAlarmInfoViewModel();
                    UtilsHelper.CopyEntity(mainfo, m);
                    lists.Insert(0, mainfo);
                }
                else
                {
                    UtilsHelper.CopyEntity(litModel, m);
                }
            }
        }


        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteAlarm(MessageModel obj)
        {
            if (obj.MessageBody is AlarmBase)
            {
                AlarmBase m = obj.MessageBody as AlarmBase;
                if (m == null)
                    return;
                WorkingAlarmInfoViewModel litModel = lists.FirstOrDefault(M => M.Jqlsh == m.Jqlsh);
                if (litModel == null)
                    return;
                lists.Remove(litModel);
            }
        }

        private ICommand dblclick;

        public ICommand Dblclick
        {
            get { return dblclick; }
        }

        private ICommand resetSeek;

        public ICommand ResetSeek
        {
            get { return resetSeek; }
        }

        private ICommand updateSetCommand;

        public ICommand UpdateSetCommand
        {
            get { return updateSetCommand; }
        }

        #endregion

    }
}
