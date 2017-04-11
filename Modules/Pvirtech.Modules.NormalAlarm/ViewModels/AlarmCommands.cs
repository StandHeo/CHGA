using ESRI.ArcGIS.Client;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Prism.Events;
using Pvirtech.Modules.NormalAlarm.ViewModels;
using Pvirtech.Modules.NormalAlarm.ViewModels.PopUp;
using Pvirtech.Framework.Common;
using Prism.Commands;
using Pvirtech.Services;
using Pvirtech.Model;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Modules.NormalAlarm.Views.PopUp;
using Pvirtech.Modules.NormalAlarm.Views;
using Pvirtech.Framework.Domain;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    public class AlarmCommands
    {
        #region 初始化

        private static readonly IUnityContainer _container;
        private static readonly IEventAggregator _eventAggregator;
        //private static readonly ICommonRepository _commonRepository;
        private static List<ExcuteCaseViewModel> _popupexcutewindow = new List<ExcuteCaseViewModel>();
        private static List<DisposeAlarmViewModel> _popupdisposewindow = new List<DisposeAlarmViewModel>();
        private static List<CloseCaseControlViewModel> _popupclosewindow = new List<CloseCaseControlViewModel>();
        private static List<AddTicklingControlViewModel> _popuptickWindow = new List<AddTicklingControlViewModel>();
        public static BaseOperItems OperItems
        { get; set; }

        static AlarmCommands()
        {
            _container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            if (OperItems != null)
                return;

            OperItems = XmlHelper.GetXmlEntities<BaseOperItems>("Alarm.AlarmOperStatus.xml");

            ICommand cmd = new DelegateCommand<string>(RunAlarmCmd);
            foreach (AlarmPatrolCmdItem item in OperItems.CmdItems)
            {
                item.BindCommand = cmd;
            }
            foreach (AlarmPatrolCmdItem item in OperItems.RunItems)
            {
                item.BindCommand = cmd;
            }

            EventCenter.Subscribe(OnGetCommand);

            EventCenter.Subscribe(OnRunPatrolCommand);
        }

        private static void OnRunPatrolCommand(CommandMessage one)
        {
            try
            {
                if (one.Group != "PatrolCommand")
                    return;

                if (one.Paras.Count == 0)
                {
                    EventCenter.PublishError("参数缺失，请重新处理");
                    return;
                }
                Patrol patl = null;
                if (one.Paras != null && one.Paras.ContainsKey("PID"))
                {
                    patl = LocalDataCenter.GetPatrol(one.Paras["PID"].ToString());
                    if (patl == null)
                    {
                        EventCenter.PublishError("警力信息匹配错误！");
                        LogHelper.WriteLog("警力信息匹配错误" + System.DateTime.Now.ToString() + "$$$" + one.Paras["PID"].ToString());
                        return;
                    }
                }
                else
                {
                    EventCenter.PublishError("参数错误");
                    return;
                };

                MethodInfo p = typeof(AlarmCommands).GetMethod(one.Key);

                if (p != null)
                {
                    object[] param = new object[1] { patl };
                    p.Invoke(null, param);
                }
                else
                {
                    EventCenter.PublishError(string.Format("错误的方法,{0},{1}", one.Key, one.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "OnPatrolCommand");
            }
        }

        #endregion

        #region 消息检查，入口调用
        public static void OnGetCommand(CommandMessage one)
        {
            try
            {
                if (one.Group != "Alarm")
                    return;

                if (one.Paras.Count == 0)
                {
                    EventCenter.PublishError("参数缺失，请重新处理");
                    return;
                }

                AlarmBase alarm = CheckAlarm(one.Paras["AID"] as string);
                if (alarm == null)
                    return;

                List<string> pids = null;
                if (one.Paras.ContainsKey("PID"))
                {
                    //pids = CheckPID(one.Paras["PID"] as string);
                    //if (pids.Count == 0)
                    //    return;
                    if (one.Paras["PID"] != null)
                    {
                        pids = one.Paras["PID"].ToString().Split(new char[] { ',', '，' }).ToList();
                    }
                }

                //MessageBox.Show(one.Paras["PID"].ToString());
                MethodInfo p = typeof(AlarmCommands).GetMethod(one.Key);


                if (p != null)
                {
                    object[] param = new object[2] { alarm, pids };
                    p.Invoke(null, param);
                }
                else
                {
                    EventCenter.PublishError(string.Format("错误的方法,{0},{1}", one.Key, one.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "OnGetCommand");
            }

        }

        public static void RunAlarmCmd(string Key)
        {
            string[] ps = Key.Split(',');
            EventCenter.PublishAlarm(ps[0], ps[1], "NO");
        }
        #endregion

        #region 私有检查
        private static List<string> CheckPID(string pid)
        {
            List<string> ret = new List<string>();

            if (string.IsNullOrEmpty(pid))
            {
                EventCenter.PublishError("请输入或选择警力信息");
                return ret;

            }
            string[] objs = UtilsHelper.ByStringSplit(pid);
            if (objs == null || objs.Length == 0)
            {
                EventCenter.PublishPrompt("请输入或选择警力信息");
                return ret; ;
            }

            foreach (string s in objs)
            {
                ret.Add(s);
            }
            return ret;
        }

        private static AlarmBase CheckAlarm(string jqlsh)
        {
            AlarmBase alarmbase = LocalDataCenter.UnFinishedAlarms.FirstOrDefault(aa => aa.Jqlsh == jqlsh);
            if (alarmbase == null)
                EventCenter.PublishError(string.Format("警情【{0}】状态可能已经发生变化，不适合当前操作", jqlsh));
            return alarmbase;
        }

        #endregion

        #region 计划改造的代码

        /// <summary>
        /// 显示警情详细信息
        /// </summary>
        /// <param name="obj"></param>
        public static void AlarmDetailCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("对象不能为空");
                return;
            }

            #endregion

            #region 显示窗体
            // _popupexcutewindow=new List<ExcuteCaseViewModel>
            WorkingAlarmInfoViewModel info = para["this"] as WorkingAlarmInfoViewModel;
            if (info != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    //ExcuteCaseViewModel viewModel =_container.Resolve<ExcuteCaseViewModel>(new ParameterOverride("alarm", info));
                    //viewModel.Title = "警情详细";
                    //viewModel.Content = new ExcuteCaseControl();
                    //PopupWindows.GetInstance().Raise(
                    //viewModel,
                    //    sendMessage =>
                    //    {
                    //        var notification = sendMessage as ExcuteCaseViewModel;
                    //    });
                    var item = _popupexcutewindow.Find(o => o.viewModel != null && o.viewModel.Jqlsh == info.Jqlsh);
                    if (item == null)
                    {
                        ExcuteCaseViewModel viewModel = _container.Resolve<ExcuteCaseViewModel>(new ParameterOverride("alarm", info));
                        viewModel.Title = "警情详细";
                        viewModel.Content = new CaseExecuteProcess();
                        viewModel.ResizeMode = ResizeMode.CanMinimize;
                        _popupexcutewindow.Add(viewModel);
                        PopupWindows.GetInstance().Raise(viewModel, onCallBack =>
                        {
                            _popupexcutewindow.Remove(viewModel);
                        });
                    }
                    else
                    {
                        //激活已打开的窗口
                        item.Activate();
                    }
                }));
            }

            #endregion
        }

        //<summary>
        //到达
        //</summary>
        //<param name="obj"></param>
        public static async void ArrivedCmd(Dictionary<string, object> info)
        {
            #region 参数验证

            if (info == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel alrmModel = info["this"] as WorkingAlarmInfoViewModel;
            MAlarmPatrol pt = info["that"] as MAlarmPatrol;
            Result<bool> backValue = await alrmModel.ArriveMession(pt.patrol.GroupNo, pt.patrol.GroupNo);
            if (backValue.Code == ResultCode.SUCCESS)
                EventCenter.PublishError("到达成功");
            else
                EventCenter.PublishError("出现错误/异常");

            #endregion

        }

        //<summary>
        //到达
        //</summary>
        //<param name="obj"></param>
        public static void ArrivedWinCmd(Dictionary<string, object> info)
        {
            #region 参数验证

            if (info == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel m = info["this"] as WorkingAlarmInfoViewModel;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ArriveControlViewModel viewModel = _container.Resolve<ArriveControlViewModel>(new ParameterOverride("alarm", m));
                viewModel.Title = "到达";
                viewModel.Content = new ArriveControl();
                PopupWindows.GetInstance().Raise(
                 viewModel,
                     sendMessage =>
                     {
                         var notification = sendMessage as ArriveControlViewModel;
                     });
            }));

            #endregion
        }

        /// <summary>
        /// 出发
        /// </summary>
        /// <param name="obj"></param>
        public static async void StartOffCmd(Dictionary<string, object> info)
        {
            #region 参数验证

            if (info == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel alrmModel = info["this"] as WorkingAlarmInfoViewModel;
            MAlarmPatrol pt = info["that"] as MAlarmPatrol;
            Result<bool> backValue = await alrmModel.StartMession(pt.patrol.GroupNo);
            if (backValue.Model)
                EventCenter.PublishError("巡组出发");
            else
                EventCenter.PublishError("出发失败");

            #endregion
        }

        /// <summary>
        /// 联系
        /// </summary>
        /// <param name="para"></param>
        public static void LinkPatrolCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            MAlarmPatrol pat = para["that"] as MAlarmPatrol;
            WorkingAlarmInfoViewModel model = para["this"] as WorkingAlarmInfoViewModel;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                LinkXunZuClontrolViewModel viewModel = new LinkXunZuClontrolViewModel(pat.patrol, model);
                viewModel.Title = "联系";
                viewModel.Content = new LinkXunZuClontrol();
                viewModel.Topmost = true;
                PopupWindows.GetInstance().Raise(
                 viewModel,
                     sendMessage =>
                     {
                         var notification = sendMessage as LinkXunZuClontrolViewModel;
                     });
            }));

            #endregion
        }

        ///<summary>
        ///接收
        ///</summary>
        ///<param name="obj"></param>
        public static async void AcceptCmd(Dictionary<string, object> info)
        {
            #region 参数验证

            if (info == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel alrmModel = info["this"] as WorkingAlarmInfoViewModel;
            MAlarmPatrol pt = info["that"] as MAlarmPatrol;
            Result<bool> backValue = await alrmModel.AcceptMession(pt.patrol.GroupNo);
            if (backValue.Code == ResultCode.SUCCESS)
                EventCenter.PublishError("接收成功");
            else
                EventCenter.PublishError("接收失败");

            #endregion

        }

        ///<summary>
        ///接收弹窗
        ///</summary>
        ///<param name="obj"></param>
        public static void AcceptWinCmd(Dictionary<string, object> info)
        {
            #region 参数验证

            if (info == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel alrmModel = info["this"] as WorkingAlarmInfoViewModel;
            if (alrmModel == null)
            {
                PopupWindows.ShowWinMessage("警情信息不能为空");
                return;
            }
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                PADReceiveControlViewModel viewModel = new PADReceiveControlViewModel(alrmModel);
                viewModel.Title = "接收";
                viewModel.Content = new PADReceiveControl();
                PopupWindows.GetInstance().Raise(
                 viewModel,
                     sendMessage =>
                     {
                         var notification = sendMessage as PADReceiveControlViewModel;

                     });
            }));

            #endregion
        }

        /// <summary>
        /// 回退警情
        /// </summary>
        /// <param name="obj"></param>
        public static void ReturnAlarmCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel info = para["this"] as WorkingAlarmInfoViewModel;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ReturnAlarmControlViewModel viewModel = new ReturnAlarmControlViewModel(info);
                viewModel.Title = "回退";
                viewModel.Content = new ReturnAlarmControl();
                PopupWindows.GetInstance().Raise(
                viewModel,
                    sendMessage =>
                    {
                        var notification = sendMessage as ReturnAlarmControlViewModel;
                        if (notification.Confirmed)
                            EventCenter.PublishError("回退成功");
                        else
                            EventCenter.PublishError("回退失败");
                    });
            }));

            #endregion
        }

        /// <summary>
        /// 催促信息
        /// </summary>
        /// <param name="obj"></param>
        public static async void UrgeMissonCmd(Dictionary<string, object> info)
        {
            #region 参数验证

            if (info == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            MAlarmPatrol pt = info["that"] as MAlarmPatrol;
            WorkingAlarmInfoViewModel alrmModel = info["this"] as WorkingAlarmInfoViewModel;
            if (alrmModel == null)
            {
                WinHelper.MessageWindow("提示", "转换警力对象失败");
                return;
            }
            if (pt.patrol.AppStatus == 0)
            {
                OffXunzuWin(alrmModel, pt.patrol.GroupName, 1);
            }
            #endregion

            #region 参数验证

            Result<bool> result = await alrmModel.UrgeAlramByIdOrText(pt.patrol.GroupNo, "1");
            if (result.Model)
            {
                EventCenter.PublishError("催促成功");
            }
            else
            {
                EventCenter.PublishError("催促失败");
                OffXunzuWin(alrmModel, pt.patrol.GroupName, 1);
            }

            #endregion
        }

        /// <summary>
        /// 催促弹窗
        /// </summary>
        /// <param name="obj"></param>
        public static void UrgeMissonWinCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel info = para["this"] as WorkingAlarmInfoViewModel;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ChuiCuControlViewModel viewModel = new ChuiCuControlViewModel(info);
                viewModel.Title = "催促";
                viewModel.Content = new ChuiCuControl();
                PopupWindows.GetInstance().Raise(
                 viewModel,
                     sendMessage =>
                     {

                     });
            }));

            #endregion
        }

        /// <summary>
        /// 警情流转到指令中心
        /// </summary>
        /// <param name="obj"></param>
        public async static void SendLinkAgeCmd(AlarmBase info, List<string> strs)
        {
            #region 参数验证

            if (info == null)
            {
                EventCenter.PublishError("警情数据不能为空");
                return;
            }
            // WorkingAlarmInfoViewModel info = para["this"] as WorkingAlarmInfoViewModel;

            //  EventCenter.PublishError("参数转换出错");


            var mainModel = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //mainModel.GetEvent<ZhiLinEventArgs>().Publish(info);
            #endregion

            #region 执行操作

            //Result<bool> result = await info.SendAlarmInfoDldByNo();
            //if (result.Code == ResultCode.SUCCESS)
            //    EventCenter.PublishPrompt("警情流转成功");
            //else
            //    EventCenter.PublishError("出现错误/异常");

            #endregion
        }

        /// <summary>
        /// 结案
        /// 弹出结案ViewModel
        /// </summary>
        /// <param name="obj"></param>
        public static void FinishMissonCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel info = para["this"] as WorkingAlarmInfoViewModel;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var item = _popupclosewindow.Find(o => o.ViewModel != null && o.ViewModel.Jqlsh == info.Jqlsh);
                if (item == null)
                {
                    CloseCaseControlViewModel viewModel = _container.Resolve<CloseCaseControlViewModel>(new ParameterOverride("alarm", info));
                    viewModel.Title = "结案";
                    viewModel.Content = new CloseCaseControl();
                    PopupWindows.GetInstance().Raise(
                     viewModel,
                         sendMessage =>
                         {
                             _popupclosewindow.Remove(viewModel);
                             var notification = sendMessage as CloseCaseControlViewModel;
                             if (notification.Confirmed)
                                 EventCenter.PublishError("结案成功");
                         });
                    _popupclosewindow.Add(viewModel);
                }
                else
                {
                    item.Activate();
                }
            }));

            #endregion
        }

        /// <summary>
        /// 处警框
        /// 弹出处警ViewModel
        /// </summary>
        /// <param name="obj"></param>
        public static void SendMissonWinCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel info = para["this"] as WorkingAlarmInfoViewModel;
            if (info != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var item = _popupexcutewindow.Find(o => o.viewModel != null && o.viewModel.Jqlsh == info.Jqlsh);
                    if (item == null)
                    {
                        ExcuteCaseViewModel viewModel = _container.Resolve<ExcuteCaseViewModel>(new ParameterOverride("alarm", info));
                        viewModel.Title = "警情详细";
                        viewModel.Content = new CaseExecuteProcess();
                        PopupWindows.GetInstance().Raise(
                        viewModel,
                            sendMessage =>
                            {
                                _popupexcutewindow.Remove(viewModel);
                            });
                        _popupexcutewindow.Add(viewModel);
                    }
                    else
                    {
                        item.Activate();
                    }
                }));
            }

            #endregion

        }

        /// <summary>
        /// 地图处警
        /// </summary>
        /// <param name="para"></param>
        public async static void MapSendMisson(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }
            List<Graphic> lit = para["that"] as List<Graphic>;
            WorkingAlarmInfoViewModel alarm = para["this"] as WorkingAlarmInfoViewModel;
            if (alarm == null)
            {
                EventCenter.PublishError("缺少警情对象");
                return;
            }
            if (lit == null && lit.Count <= 0)
            {
                EventCenter.PublishError("缺少巡组参数");
                return;
            }

            #endregion

            #region 执行操作

            string offXunzu = null;  //离线的巡组
            string TrimXunzu = null;  //输入错误的巡组
            string ErrorString = null; //失败的巡组
            foreach (Graphic m in lit)
            {
                #region 参数验证

                Patrol patrol = LocalDataCenter.GetPatrol(m.Attributes["NO"].ToString());
                if (patrol == null)
                {
                    TrimXunzu += m.Attributes["NAME"] + ",";
                    continue;
                }
                if (patrol.AppStatus == 0)
                {
                    offXunzu += patrol.GroupName + ",";
                    continue;
                }

                #endregion

                Result<bool> result = await alarm.ExcuteMession("请快速赶往事发地址", patrol.GroupName, patrol.GroupNo, 0, 1);
                if (result.Code == ResultCode.SUCCESS)
                {
                    if (result.Model)
                        //派出所，警情流水号，派警 巡组号 操作成功！";
                        EventCenter.PublishPrompt("派警到单兵设备成功");
                    else
                        ErrorString += m.Attributes["NAME"].ToString();
                    //NotifyMessage = "操作失败！";
                }
                else
                    EventCenter.PublishError("操作失败" + result.Code + " " + result.Message);
            }

            #region 执行结果判断

            if (!string.IsNullOrEmpty(TrimXunzu))
                EventCenter.PublishError(TrimXunzu + "不存在的警力单位");
            if (!string.IsNullOrEmpty(offXunzu))
            {
                OffXunzuWin(alarm, offXunzu, 0);
            }
            if (!string.IsNullOrEmpty(ErrorString))
                EventCenter.PublishError(ErrorString + "警力发送失败");

            #endregion

            #endregion

        }

        #endregion

        #region new alarm command

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="alarmBase"></param>
        /// <param name="s"></param>
        public static void GetVideoByNo(Patrol p)
        {
            //if (p != null)
            //{
            //    var window = ShowVideoWindow.GetInstance();
            //    window.Show();
            //    window.PlayVideo(p.AppNo);
            //}
        }

        /// <summary>
        /// 电台呼叫
        /// </summary>
        /// <param name="alarmBase"></param>
        /// <param name="s"></param>
        public static void PlatformInform(Patrol p)
        {
            //if (p != null)
            //{
            //    Task.Run(() =>
            //    {
            //        if (string.IsNullOrEmpty(p.ChannelNo))
            //        {
            //            EventCenter.PublishError("缺少号码");
            //            return;
            //        }
            //        //  UtilsHelper.CallPhone(p.ChannelNo);
            //        var phoneEvent = ServiceLocator.Current.GetInstance<IEventAggregator>();
            //        phoneEvent.GetEvent<CallPhoneEventArgs>().Publish(new CallPhoneEvent(p.ChannelNo, 1, true, 0));
            //    });
            //}
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="alarmBase"></param>
        /// <param name="s"></param>
        public static void SendMessager(Patrol p)
        {

            //if (p != null)
            //{
            //    WorkingAlarmInfoViewModel m = null;     //当前指令关联的警情
            //    Dictionary<string, string> para = new Dictionary<string, string>();
            //    para.Add(p.GroupNo, p.GroupName);
            //    //发送指令
            //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        MessagerWinViewModel viewModel = new MessagerWinViewModel(m, para);
            //        viewModel.Title = "发送指令";
            //        viewModel.Content = new MessagerWin();
            //        PopupWindows.GetInstance().Raise(
            //         viewModel,
            //             sendMessage =>
            //             {
            //             });
            //    }));
            //}
        }
        /// <summary>
        /// 通知/提示窗口
        /// </summary>
        public static void AlarmHintWin(AlarmBase alarmbase, List<string> s)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                UCAlarmHintViewModel viewModel = new UCAlarmHintViewModel(alarm, null, 0);
                viewModel.Title = "通知/提示窗口";
                viewModel.Content = new UCAlarmHint();
                PopupWindows.GetInstance().Raise(
                 viewModel,
                    sendMessage =>
                    {

                    });
            }));
        }

        /// <summary>
        /// 移交大联动
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="s"></param>
        /// 
        //private static Dictionary<string, Timer> m_timers = new Dictionary<string, Timer>();
        //public async static void SendLinkAge(AlarmBase alarmbase, List<string> s)
        //{
        //    if (MessageBox.Show("是否确认流转至大联动？", "请确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
        //    {
        //        return;
        //    }

        //    if (alarmbase.DldStatus == 2 || alarmbase.DldStatus == 3)
        //    {
        //        EventCenter.PublishPrompt("当前事件已成功流转至大联动！");
        //        return;
        //    }

        //    _eventAggregator.GetEvent<SendToBigLinkageArgs>().Publish(alarmbase.Jqlsh);
        //    _eventAggregator.GetEvent<BigLinkageProgressBarVisibilityArgs>().Publish(new Tuple<string, Visibility>(alarmbase.Jqlsh, Visibility.Visible));
        //    Timer timer = new Timer();
        //    timer.Interval = Global.GlobalConfig.GetInstance().DldTimeOut;
        //    timer.Elapsed += (sender, e) =>
        //    {
        //        timer.Stop();
        //        timer.Close();
        //        //await _commonRepository.SendQRCodeTimeOut(alarmbase.Jqlsh);
        //        //await _commonRepository.GenerateQRCode(alarmbase.Jqlsh);
        //        _eventAggregator.GetEvent<BigLinkageTimeOutArgs>().Publish(alarmbase.Jqlsh);
        //        _eventAggregator.GetEvent<BigLinkageProgressBarVisibilityArgs>().Publish(new Tuple<string, Visibility>(alarmbase.Jqlsh, Visibility.Collapsed));
        //    };
        //    var user = LocalDataCenter.GetLocalUser();
        //    timer.Start();
        //    var rs = await _commonRepository.SendQRCodeToDld(alarmbase.Jqlsh, user.UserNo, user.UserName);

        //    if (!string.IsNullOrWhiteSpace(rs))
        //    {
        //        timer.Close();
        //        _eventAggregator.GetEvent<BigLinkageProgressBarVisibilityArgs>().Publish(new Tuple<string, Visibility>(alarmbase.Jqlsh, Visibility.Collapsed));
        //        EventCenter.PublishError(rs);
        //    }
        //}

        ///// <summary>
        ///// 巡组作废
        ///// </summary>
        ///// <param name="alarmbase"></param>
        //public static void ErrorHandleWin(AlarmBase alarmbase, List<string> s)
        //{
        //    WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
        //    UtilsHelper.CopyEntity(alarm, alarmbase);
        //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        UCErrorHandleViewModel viewModel = new UCErrorHandleViewModel(alarm);
        //        viewModel.Title = "巡组作废";
        //        viewModel.Content = new UCErrorHandle();
        //        PopupWindows.GetInstance().Raise(
        //         viewModel,
        //            sendMessage =>
        //            {

        //            });
        //    }));
        //}

        /// <summary>
        /// 发送催促信息
        /// </summary>
        /// <param name="alrambase"></param>
        /// <param name="objs"></param>
        public async static void UrgeTimeOut(AlarmBase alarmbase, List<string> objs)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            //离线的巡组
            string offLine = null;
            //错误的对象
            string ErrorObj = null;

            foreach (var s in objs)
            {
                Patrol pt = LocalDataCenter.GetPatrol(s);
                if (pt == null)
                {
                    ErrorObj += s + "未识别的巡组编号";
                    continue;
                }
                if (pt.AppStatus == 0)
                {
                    offLine += pt.GroupName + ",";
                    continue;
                }
                Result<bool> backValue = await UrgeAlramByIdOrText(alarm, s, pt.AppStatus.ToString());
                if (backValue.Code == ResultCode.SUCCESS)
                    EventCenter.PublishPrompt("催促成功");
                else
                    EventCenter.PublishError("催促失败");
            }
            if (!string.IsNullOrEmpty(offLine))
                OffXunzuWin(alarm, offLine, 1);
        }

        /// <summary>
        /// 发送催促延迟记录
        /// </summary>
        /// <param name="alrambase"></param>
        /// <param name="objs"></param>
        public async static void NewAlarmTimeOut(AlarmBase alarmbase, List<string> objs)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            Result<bool> backValue = await SendActionRecord(alarm, "1", alarm.Jqlsh, Framework.Domain.GlobalConfig.GetInstance().TimeTick.ToString());
            if (backValue.Code == ResultCode.SUCCESS)
                EventCenter.PublishPrompt("系统已记录你延迟操作信息");
            else
                EventCenter.PublishError("操作失败");
        }

        private static void AddSendAlarmTime(Patrol patrol, WorkingAlarmInfoViewModel alarm)
        {
            var findItem = LocalDataCenter.lstSendAlarmTime.Find(o => o.Jqlsh == alarm.Jqlsh && o.GroupNo == patrol.GroupNo);
            if (findItem == null)
            {
                SendAlarmTime item = new SendAlarmTime();
                item.Count = 0;
                item.GroupNo = patrol.GroupNo;
                item.GroupName = patrol.GroupName;
                item.Jqlsh = alarm.Jqlsh;
                LocalDataCenter.lstSendAlarmTime.Add(item);
            }
            else
            {
                findItem.Count = 0;
            }

        }

        /// <param name="obj"></param>
        /// <summary>
        /// 根据输入警力处置警情
        /// </summary>
        public async static void SendAlarm(AlarmBase alarmbase, List<string> patrols)
        {
            if (patrols == null || patrols.Count == 0)
            {
                return;
            }
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            string ErrorString = string.Empty; //失败的巡组       
            var policeCaseRepository = ServiceLocator.Current.GetInstance<IPoliceCaseRepository>();
            string tmpPhone = string.Empty;
            if (policeCaseRepository != null)
            {
                string content = string.Format("姓名：{0},电话：{1},地址：{2},内容：{3}", alarmbase.Bjrxm, alarmbase.Lxdh, alarmbase.Sfdz, alarmbase.Bjnr);
                string phones = string.Empty;
                foreach (var item in patrols)
                {
                    Patrol patrol = LocalDataCenter.GetPatrol(item);
                    if (patrol != null)
                    {
                        if (string.IsNullOrEmpty(patrol.AppNo))
                        {
                            tmpPhone += patrol.GroupName;
                        }
                        else
                        {
                            phones += patrol.AppNo + ",";
                            ErrorString = await ExcuteMession(alarm, ErrorString, item, patrol);
                        }
                    }
                }
                phones = phones.TrimEnd(',');
                if (string.IsNullOrEmpty(phones))
                {
                    WinHelper.MessageWindow("提示", string.Format("当前巡组{0}手机号码未绑定!", tmpPhone, 3000));
                    return;
                }
                var result = await policeCaseRepository.SendSMS(GlobalConfig.GetInstance().SMSUrl, content, phones);
                if (result)
                {
                    //   EventCenter.PublishPrompt("派警到单兵设备成功");
                    WinHelper.MessageWindow("提示", "派警到单兵设备成功", 3000);
                }
            }

            //if (!string.IsNullOrEmpty(TrimXunzu))
            //    EventCenter.PublishError(TrimXunzu + "不存在的警力单位");
            //if (!string.IsNullOrEmpty(offXunzu))
            //    OffXunzuWin(alarm, offXunzu, 0);
            if (!string.IsNullOrEmpty(ErrorString))
                EventCenter.PublishError(ErrorString + "警力发送失败");
        }

        /// <summary>
        /// 记录派警
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="ErrorString"></param>
        /// <param name="s"></param>
        /// <param name="patrol"></param>
        /// <returns></returns>
        private static async Task<string> ExcuteMession(WorkingAlarmInfoViewModel alarm, string ErrorString, string s, Patrol patrol)
        {
            UtilsHelper.LogTexts("6666，派(在线)警" + s);
            Result<bool> result = await alarm.ExcuteMession("请快速赶往事发地址", patrol.GroupName, patrol.GroupNo, 0, 1);
            if (result.Code == ResultCode.SUCCESS)
            {
                if (result.Model)
                {
                    AddSendAlarmTime(patrol, alarm);
                    //派出所，警情流水号，派警 巡组号 操作成功！";
                    // EventCenter.PublishPrompt("派警到单兵设备成功");
                    //WinHelper.MessageWindow("提示", "派警到单兵设备成功",3000);
                }
                else
                    ErrorString += patrol.GroupName;
                //NotifyMessage = "操作失败！";
            }
            else
                EventCenter.PublishPrompt("操作失败" + result.Code + " " + result.Message);
            return ErrorString;
        }

        /// <param name="obj"></param>
        /// <summary>
        /// PDA端接受
        /// </summary>
        public async static void AcceptAlarm(AlarmBase alarmbase, List<string> pids)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            foreach (string s in pids)
            {
                UtilsHelper.LogTexts("5555，接受巡组号" + s);
                Result<bool> backValue = await AcceptMession(alarm, s);
                if (backValue.Code == ResultCode.SUCCESS)
                    EventCenter.PublishPrompt("接受成功");
                else
                    EventCenter.PublishError("接受失败");
            }
        }

        /// <summary>
        /// 处警
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="lits"></param>
        public static void SendAlarmWin(AlarmBase alarmbase, List<string> lits)
        {
            WorkingAlarmInfoViewModel model = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(model, alarmbase);
            model.Init();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                //  _popupdisposewindow
                var item = _popupdisposewindow.Find(o => o.viewModel != null && o.viewModel.Jqlsh == alarmbase.Jqlsh);
                if (item == null)
                {
                    DisposeAlarmViewModel viewModel = _container.Resolve<DisposeAlarmViewModel>(new ParameterOverride("alarm", model));
                    viewModel.Title = "处置警情";
                    viewModel.Content = new DisposeAlarmControl();
                    PopupWindows.GetInstance().Raise(
                     viewModel,
                         sendMessage =>
                         {
                             _popupdisposewindow.Remove(viewModel);
                         });
                    _popupdisposewindow.Add(viewModel);
                }
                else
                {
                    item.Activate();
                }
            }));
        }

        /// <summary>
        /// 催促警力
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="pids"></param>
        public async static void UrgeAlarm(AlarmBase alarmbase, List<string> pids)
        {
            //WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            //UtilsHelper.CopyEntity(alarm, alarmbase);
            //foreach (string s in pids)
            //{
            //    Result<bool> backValue = await UrgeAlramByIdOrText(alarm, s,"1");
            //    if (backValue.Code == ResultCode.SUCCESS)
            //        EventCenter.PublishPrompt("催促成功");
            //    else
            //        EventCenter.PublishError("催促失败");
            //}
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            //离线的巡组
            string offLine = null;
            //错误的对象
            string ErrorObj = null;

            foreach (var s in pids)
            {
                Patrol pt = LocalDataCenter.GetPatrol(s);
                if (pt == null)
                {
                    continue;
                }

                Result<bool> backValue = await UrgeAlramByIdOrText(alarm, s, pt.AppStatus.ToString());
                // if (backValue.Code == ResultCode.SUCCESS)
                //    EventCenter.PublishPrompt("催促成功")；
                // else
                //  EventCenter.PublishError("催促失败");
            }

            string offXunzu = null;  //离线的巡组
            string TrimXunzu = null;  //输入错误的巡组
            string ErrorString = null; //失败的巡组       
            var policeCaseRepository = ServiceLocator.Current.GetInstance<IPoliceCaseRepository>();
            string tmpPhone = string.Empty;
            if (policeCaseRepository != null)
            {
                string content = "催促:请速度到达事发地址。立即对周边环境布控，严查可疑人员,确保人员安全!";// string.Format("姓名：{0},电话：{1},地址：{2},内容：{3}", alarmbase.Bjrxm, alarmbase.Lxdh, alarmbase.Sfdz, alarmbase.Bjnr);
                string phones = string.Empty;
                foreach (var item in pids)
                {
                    Patrol patrol = LocalDataCenter.GetPatrol(item);
                    if (patrol != null)
                    {
                        if (string.IsNullOrEmpty(patrol.AppNo))
                        {
                            tmpPhone += patrol.GroupName;
                        }
                        else
                            phones = patrol.AppNo + ",";
                    }
                }
                phones = phones.TrimEnd(',');
                if (string.IsNullOrEmpty(phones))
                {
                    WinHelper.MessageWindow("提示", string.Format("当前巡组{0}手机号码未绑定!", tmpPhone));
                    return;
                }
                var result = await policeCaseRepository.SendSMS(GlobalConfig.GetInstance().SMSUrl, content, phones);
                if (result)
                {
                    EventCenter.PublishPrompt("催促短信已发送给巡组！");
                }
            }
            //if (!string.IsNullOrEmpty(offLine))
            //{
            //    OffXunzuWin(alarm, offLine, 1);
            //}
        }

        /// <summary>
        /// 结案
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="jams"></param>
        public static void CloseAlarm(AlarmBase alarmbase, List<string> jams)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var item = _popupclosewindow.Find(o => o.ViewModel != null && o.ViewModel.Jqlsh == alarmbase.Jqlsh);
                if (item == null)
                {
                    CloseCaseControlViewModel viewModel = _container.Resolve<CloseCaseControlViewModel>(new ParameterOverride("alarm", alarm));
                    viewModel.Title = "结案";
                    viewModel.Content = new CloseCaseControl();
                    PopupWindows.GetInstance().Raise(
                     viewModel,
                         sendMessage =>
                         {
                             _popupclosewindow.Remove(viewModel);
                             var notification = sendMessage as CloseCaseControlViewModel;
                             //if (notification.Confirmed)
                             //    EventCenter.PublishPrompt("结案成功");
                         });
                    _popupclosewindow.Add(viewModel);
                }
                else
                {
                    item.Activate();
                }
            }));
        }

        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="strs"></param>
        public static void AlarmDetail(AlarmBase alarmbase, List<string> strs)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var item = _popupexcutewindow.Find(o => o.viewModel != null && o.viewModel.Jqlsh == alarmbase.Jqlsh);
                if (item == null)
                {
                    ExcuteCaseViewModel viewModel = _container.Resolve<ExcuteCaseViewModel>(new ParameterOverride("alarm", alarm));
                    viewModel.Title = "详细信息";
                    viewModel.Content = new CaseExecuteProcess();
                    PopupWindows.GetInstance().Raise(
                     viewModel,
                         sendMessage =>
                         {
                             _popupexcutewindow.Remove(viewModel);
                         });
                    _popupexcutewindow.Add(viewModel);
                }
                else
                {
                    item.Activate();
                }
            }));
        }

        /// <summary>
        /// 回退
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="jams"></param>
        public async static void ReturnAlarm(AlarmBase alarmbase, string jams)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            Result<bool> backValue = await ReturnAlarmByText(alarm, jams);
            if (backValue.Code == ResultCode.SUCCESS)
                EventCenter.PublishPrompt("回退成功");
            else
                EventCenter.PublishError("回退失败");
        }

        /// <summary>
        /// 到达
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="groupNo"></param>
        public async static void ArriveAlarm(AlarmBase alarmbase, List<string> groupNo)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            foreach (string s in groupNo)
            {
                Result<bool> backValue = await ArriveMession(alarm, s);
                if (backValue.Code == ResultCode.SUCCESS)
                    EventCenter.PublishPrompt("到达成功");
                else
                    EventCenter.PublishError("到达失败");
            }
        }

        /// <summary>
        /// 出发
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="groupNo"></param>
        public async static void StartAlarm(AlarmBase alarmbase, List<string> groupNo)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            foreach (string s in groupNo)
            {
                Result<bool> backValue = await StartMession(alarm, s);
                if (backValue.Code == ResultCode.SUCCESS)
                    EventCenter.PublishPrompt("出发成功");
                else
                    EventCenter.PublishError("出发失败");
            }
        }

        /// <summary>
        /// 反馈
        /// </summary>
        /// <param name="alarmbase"></param>
        /// <param name="groupNo"></param>
        public static void FeedBackAlarm(AlarmBase alarmbase, List<string> groupNo)
        {
            WorkingAlarmInfoViewModel alarm = new WorkingAlarmInfoViewModel();
            UtilsHelper.CopyEntity(alarm, alarmbase);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var item = _popuptickWindow.Find(o => o.ThisModel != null && o.ThisModel.Jqlsh == alarmbase.Jqlsh);
                if (item == null)
                {
                    AddTicklingControlViewModel viewModel = _container.Resolve<AddTicklingControlViewModel>(new ParameterOverride("alarm", alarm));
                    viewModel.Title = "反馈";
                    viewModel.Content = new AddTicklingControl();
                    PopupWindows.GetInstance().Raise(
                    viewModel,
                        sendMessage =>
                        {
                            _popuptickWindow.Remove(viewModel);
                            var notification = sendMessage as AddTicklingControlViewModel;
                            if (notification.Confirmed)
                                EventCenter.PublishPrompt(notification.NotifyMessage);
                        });
                    _popuptickWindow.Add(viewModel);
                }
                else
                {
                    item.Activate();
                }
            }));
        }

        #endregion

        #region 调用接口

        /// <summary>
        /// 派警
        /// jqlsh:警情流水号
        /// zhgxrxm:最后更新人姓名
        /// zhgxrbh:最后更新人姓名
        /// cjybh:处警员编号
        /// cjcs:处警措施
        /// cjdwmz:执勤组名字
        /// cjdwbh:执勤组编号
        /// is2PDA:0：不发送 ，1：发送
        /// allowEnd:是否允许执勤组结案 int
        /// </summary>
        public static async Task<Result<bool>> ExcuteMession(MAlarmInfo info, string cjcs, string cjdwmz, string cjdwbh, int allowEnd, int is2PDA)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            User local = LocalDataCenter.GetLocalUser();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("zhgxrxm", local.UserName);
            param.Add("zhgxrbh", local.UserNo);
            param.Add("cjybh", local.UserNo);
            param.Add("cjcs", cjcs);
            param.Add("cjdwmz", cjdwmz);
            param.Add("cjdwbh", cjdwbh);
            param.Add("allowEnd", allowEnd);
            param.Add("is2PDA", is2PDA);
            return await CommandCenter.ExcuteObject<bool>("executeAlarmPC", param);
        }

        /// <summary>
        /// 接受
        /// jqlsh:警情流水号
        /// cjybh:处警员编号
        /// cjyxm:处警员姓名
        /// cjzt:处理警情状态 2:接受/3出发/4到达/5结案
        /// groupNo:执勤组编号
        /// </summary>
        public static async Task<Result<bool>> AcceptMession(MAlarmInfo info, string groupNo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "2");
            param.Add("groupNo", groupNo);
            return await CommandCenter.ExcuteObject<bool>("dealCasePC", param);
        }

        /// <summary>
        /// 结案
        /// jqlsh:警情流水号
        /// cjybh:处警员编号
        /// cjyxm:处警员姓名
        /// cjzt:处理警情状态 2:接受/3出发/4到达/5结案
        /// groupNo:执勤组编号
        /// jams:结案描述
        /// jqly:警情来源
        /// jjdbh:接警单编号
        /// replaytype:"01"
        /// delayworkday:"无"
        /// </summary>
        public static async Task<Result<bool>> CloseMession(MAlarmInfo info, string jams)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "5");
            param.Add("jams", jams);
            param.Add("jqly", info.Jqly);
            param.Add("jjdbh", info.Jjdbh);
            param.Add("replaytype", "01");
            param.Add("delayworkday", "无");
            return await CommandCenter.ExcuteObject<bool>("dealCasePC", param);
        }

        /// <summary>
        /// 警情回退
        /// jqlsh:警情流水号
        /// cjybh:处警员编号
        /// cjyxm:处警员姓名
        /// cjzt:处理警情状态 2:接受/3出发/4到达/5结案
        /// groupNo:执勤组编号
        /// jams:结案描述
        /// jqly:警情来源
        /// jjdbh:接警单编号
        /// replaytype:"02"
        /// delayworkday:"无"
        /// </summary>
        public static async Task<Result<bool>> ReturnAlarmByText(MAlarmInfo info, string jams)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "5");
            param.Add("jams", jams);
            param.Add("jqly", info.Jqly);
            param.Add("jjdbh", info.Jjdbh);
            param.Add("replaytype", "02");
            param.Add("delayworkday", "无");
            return await CommandCenter.ExcuteObject<bool>("dealCasePC", param);
        }

        /// <summary>
        /// 到达
        /// jqlsh:警情流水号
        /// cjybh:处警员编号
        /// cjyxm:处警员姓名
        /// cjzt:处理警情状态 2:接受/3出发/4到达/5结案
        /// groupNo:执勤组编号
        /// </summary>
        public static async Task<Result<bool>> ArriveMession(MAlarmInfo info, string groupNo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "4");
            param.Add("groupNo", groupNo);
            return await CommandCenter.ExcuteObject<bool>("dealCasePC", param);
        }

        /// <summary>
        /// 出发
        /// jqlsh:警情流水号
        /// cjybh:处警员编号
        /// cjyxm:处警员姓名
        /// cjzt:处理警情状态 2:接受/3出发/4到达/5结案
        /// groupNo:执勤组编号
        /// </summary>
        public static async Task<Result<bool>> StartMession(MAlarmInfo info, string groupNo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "3");
            param.Add("groupNo", groupNo);
            return await CommandCenter.ExcuteObject<bool>("dealCasePC", param);
        }

        /// <summary>
        /// 警情反馈
        /// jqlsh:警情流水号
        /// userNo:处警员编号
        /// fknr:反馈内容
        /// cjdwbh:出警单位编号
        /// cjdwmz:出警单位名称
        /// fkrmz:反馈人名字
        /// saje:涉案金额
        /// bjxl:报警细类编码
        /// bjlb:报警类别编码
        /// bjlx：报警类型编码
        /// isMain：是否主反馈内容。0是不会反馈到市局，1是，反馈到市局
        /// </summary>
        /// <returns></returns>
        public static async Task<Result<bool>> FeedBackMession(MAlarmInfo info, string fknr, string cjdwbh, string cjdwmz, string saje, string bjxl, string bjlb, string bjlx, int isMian)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("userNo", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("fkrmz", LocalDataCenter.GetLocalUser().UserName);
            param.Add("fknr", fknr);
            param.Add("cjdwbh", cjdwbh);
            param.Add("cjdwmz", cjdwmz);
            param.Add("saje", saje);
            param.Add("bjxl", bjxl);
            param.Add("bjlb", bjlb);
            param.Add("bjlx", bjlx);
            param.Add("isMian", isMian);
            return await CommandCenter.ExcuteObject<bool>("addFedBckAlarm", param);
        }

        /// <summary>
        /// 移交大联动
        /// </summary>
        /// <returns></returns>
        public static async Task<Result<bool>> SendAlarmInfoDldByNo(MAlarmInfo info)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", info.Jqlsh);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            return await CommandCenter.ExcuteObject<bool>("send2Dld", param);
        }

        /// <summary>
        /// 催促警情
        /// </summary>
        /// <param name="objNo">催促对象Id</param>
        /// <returns></returns>
        public static async Task<Result<bool>> UrgeAlramByIdOrText(MAlarmInfo info, string objNo, string v)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("groupNo", objNo);
            param.Add("jqlsh", info.Jqlsh);
            param.Add("ccnr", info.Bjnr + "，请你速度赶往事发地址，处置警情");
            param.Add("is2PDA", v);
            param.Add("cjr", LocalDataCenter.GetLocalUser().UserName);
            return await CommandCenter.ExcuteObject<bool>("urgeAlarmUnit", param);
        }

        /// <summary>
        /// 发送延迟出警记录
        /// </summary>
        /// <param name="info">警情对象</param>
        /// <param name="type">类型</param>
        /// <param name="jqlsh">警情流水号</param>
        /// <param name="thisTime">时间</param>
        /// <returns></returns>
        public static async Task<Result<bool>> SendActionRecord(MAlarmInfo info, string type, string jqlsh, string thisTime)
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("status", type);
            para.Add("cjymz", LocalDataCenter.GetLocalUser().UserName);
            para.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            para.Add("jqlsh", jqlsh);
            para.Add("time", thisTime);
            return await CommandCenter.ExcuteObject<bool>("addUnRecord", para);
        }

        /// <summary>
        /// 巡组作废
        /// </summary>
        /// <param name="info"></param>
        /// <param name="groupId">巡组ID</param>
        /// <param name="groupName">巡组Name</param>
        /// <param name="zflx">作废类型</param>
        /// <param name="zfnr">作废依据</param>
        /// <returns></returns>
        public static async Task<Result<bool>> AlarmCancleUnit(MAlarmInfo info, string groupId, string groupName, string zflx, string zfnr)
        {
            Dictionary<string, object> parm = new Dictionary<string, object>();
            parm.Add("jqlsh", info.Jqlsh);
            parm.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            parm.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            parm.Add("groupId", groupId);
            parm.Add("groupNo", groupName);
            parm.Add("zfyj", zfnr);
            parm.Add("zflx", zflx);
            return await CommandCenter.ExcuteObject<bool>("cancleUnit", parm);
        }

        #endregion

        #region 计划改造的代码II

        /// <param name="obj"></param>
        /// <summary>
        /// 根据输入警力处置警情
        /// </summary>
        public async static void SendMissonCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            try
            {
                #region 执行操作

                WorkingAlarmInfoViewModel alarm = para["this"] as WorkingAlarmInfoViewModel;
                if (alarm == null)
                    return;
                if (string.IsNullOrEmpty(alarm.SelectPatrols.SelectPatrolKeys))
                {
                    EventCenter.PublishError("请输入巡组");
                    return;
                }
                string[] objs = UtilsHelper.ByStringSplit(alarm.SelectPatrols.SelectPatrolKeys);
                if (objs == null || objs.Length == 0)
                {
                    EventCenter.PublishError("请输入有效的巡组");
                    return;
                }
                string offXunzu = null;  //离线的巡组
                string TrimXunzu = null;  //输入错误的巡组
                string ErrorString = null; //失败的巡组                
                foreach (string s in objs)
                {
                    #region 参数验证

                    Patrol patrol = LocalDataCenter.GetPatrol(s.Trim());
                    if (patrol == null)
                    {
                        TrimXunzu += s + ",";
                        continue;
                    }
                    if (patrol.AppStatus == 0)
                    {
                        offXunzu += patrol.GroupName + ",";
                        continue;
                    }

                    #endregion

                    Result<bool> result = await alarm.ExcuteMession("请快速赶往事发地址", patrol.GroupName, patrol.GroupNo, 0, 1);
                    if (result.Code == ResultCode.SUCCESS)
                    {
                        if (result.Model)
                            //派出所，警情流水号，派警 巡组号 操作成功！";
                            WinHelper.MessageWindow("提示", "派警到单兵设备成功", 1000);
                        else
                            ErrorString += s;
                        //NotifyMessage = "操作失败！";
                    }
                    else
                        WinHelper.MessageWindow("提示", "操作失败" + result.Code + " " + result.Message);
                }

                #region 执行结果判断

                if (!string.IsNullOrEmpty(TrimXunzu))
                    WinHelper.MessageWindow("提示", TrimXunzu + "不存在的警力单位");
                if (!string.IsNullOrEmpty(offXunzu))
                    OffXunzuWin(alarm, offXunzu, 0);
                if (!string.IsNullOrEmpty(ErrorString))
                    WinHelper.MessageWindow("提示", ErrorString + "警力发送失败");

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "AlarmRunCmd - SendMissonCmd");
            }
        }

        /// <summary>
        /// 离线弹窗
        /// </summary>
        /// <param name="model">警情对象</param>
        /// <param name="text">巡组信息</param>
        /// <param name="i">操作类型{0：派件；1：催促}</param>
        public static void OffXunzuWin(MAlarmInfo model, string text, int i)
        {
            #region 参数验证

            if (model == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            string[] objs = UtilsHelper.ByStringSplit(text);
            if (objs == null)
            {
                return;
            }
            List<Patrol> lit = new List<Patrol>();
            foreach (var item in objs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Patrol patrol = LocalDataCenter.GetPatrolByName(item);
                    if (patrol != null)
                    {
                        lit.Add(patrol);
                    }
                }
            }
            if (lit.Count > 0)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    AlarmHintControlViewModel viewModel = new AlarmHintControlViewModel(model, lit, i);
                    viewModel.Title = "离线提示";
                    viewModel.Content = new AlarmHintControl();
                    PopupWindows.GetInstance().Raise(
                     viewModel,
                         sendMessage =>
                         {
                             var notification = sendMessage as AlarmHintControlViewModel;
                         });
                }));
            }

            #endregion
        }

        /// <summary>
        /// 反馈
        /// 弹出反馈窗口
        /// </summary>
        /// <param name="info"></param>
        public static void BackMissonCmd(Dictionary<string, object> para)
        {
            #region 参数验证

            if (para == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            #region 执行操作

            WorkingAlarmInfoViewModel info = para["this"] as WorkingAlarmInfoViewModel;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {

                AddTicklingControlViewModel viewModel = _container.Resolve<AddTicklingControlViewModel>(new ParameterOverride("alarm", info));
                viewModel.Title = "反馈";
                viewModel.Content = new AddTicklingControl();
                PopupWindows.GetInstance().Raise(
                viewModel,
                    sendMessage =>
                    {
                        var notification = sendMessage as AddTicklingControlViewModel;
                        if (notification.Confirmed)
                            EventCenter.PublishError(notification.NotifyMessage);
                    });
            }));

            #endregion
        }


        #endregion

        #region 重要操作提示窗口(方法)

        public static bool HintWinByText(string title, string text)
        {
            bool value = false;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                UCHintWinViewModel viewModel = new UCHintWinViewModel();
                viewModel.Title = "温馨提示：";
                viewModel.Content = new UCHintWin();
                PopupWindows.GetInstance().Raise(
                viewModel,
                    sendMessage =>
                    {
                        var notification = sendMessage as UCHintWinViewModel;
                        value = notification.Confirmed;
                    });
            }));
            return value;
        }

        #endregion

    }
}
