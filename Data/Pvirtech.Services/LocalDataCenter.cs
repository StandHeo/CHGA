using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pvirtech.Framework.Common;
using Pvirtech.Framework.Domain;
using Pvirtech.Framework.Message;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Pvirtech.Services
{
    /// <summary>
    /// 数据中心
    /// 维护本地数据的唯一类
    /// </summary>
    public class LocalDataCenter : LocalData
    {
        private static IMessageAgent _messageAgent;
        private static IPoliceCaseRepository policeCaseRepository;

        static LocalDataCenter()
        {
            policeCaseRepository = ServiceLocator.Current.GetInstance<IPoliceCaseRepository>();
        }
        /// <summary>
        /// 启动数据中心，开启消息监听服务
        /// </summary>
        public static void Start(IMessageAgent messageAgent)
        {
            policeCaseRepository = new PoliceCaseRepository();
            _messageAgent = messageAgent;
            messageAgent.CreateAdapter(LocalUser.UserNo, LocalUser.Token, "", new MQClientAdapter.WhenMessageReceiveHandle(ReceiveServiceMsg));
        }

        
        public static ZhiYinXMLModel ZhiYinXML { get; set; }
        public static void SetZhiYin()
        {
            ZhiYinXML = XmlHelper.GetXmlEntities<ZhiYinXMLModel>("Alarm.Zhiyinguifan.xml");
        }

        /// <summary>
        /// 接收服务端推送消息
        /// </summary>
        public static async void ReceiveServiceMsg(object sender, string filter, string msgType, string body)
        {
            try
            {
                if (filter != "ALL")
                    return;
                JObject obj = new JObject();
                obj = JObject.Parse(body);
                string type = obj["method"].ToString();
                switch (msgType)
                {
                    //更新警情
                    case "alarmChange":
                        #region 更新警情
                        if (obj["result"] != null)
                        {
                            AlarmBase item = JsonConvert.DeserializeObject<AlarmBase>(obj["result"].ToString());
                            if (!LocalUser.IsAllFunction)
                            {
                                if (item.Gxdwbh != LocalUser.DeptNo)
                                {
                                    return;
                                }
                            }
                            //AlarmBase tmp = AlarmCollections.FirstOrDefault(alarm => alarm.jqlsh.Equals(item.jqlsh));
                            /* 收到警情的通知
                             * 1、移除原有警情内容
                             * 2、移除原有警情处警记录
                             * 3、移除原有警情操作记录
                             * 4、移除原有警情反馈内容
                             * */
                            if (item.Sddwxzb == 0 || item.Sddwyzb == 0)
                            {
                                //绑定警情坐标  
                                item.MapPoint = new MapPoint(item.Zddwxzb, item.Zddwyzb, new SpatialReference(4490));
                            }
                            else
                            {
                                item.MapPoint = new MapPoint(item.Sddwxzb, item.Sddwyzb, new SpatialReference(4490));
                            }

                            alarmCollections.RemoveAll(aa => aa.Jqlsh == item.Jqlsh);
                            if (item.Ajzt == (int)AlarmStatus.END)
                            {
                                /* 结案通知
                                 * 1、移除警情内容
                                 * 2、移除警情处警记录
                                 * 3、移除警情操作记录
                                 * 4、移除警情反馈内容
                                 * */
                            }
                            else
                            {
                                /* 非结案警情通知
                                 * 
                                 * 1、添加（更新）到警情集合
                                 * 2、查询处警记录,添加（更新）
                                 * 3、查询操作记录,添加（更新）
                                 * 4、查询反馈记录,添加（更新）
                                 * */
                                Dictionary<string, object> param = new Dictionary<string, object>();
                                param.Add("jqlsh", item.Jqlsh);
                                Result<List<AlarmOperation>> listOper = await policeCaseRepository.GetAlarmOperation(param);
                                Result<List<AlarmOperationRecord>> listOperRcd = await policeCaseRepository.GetAlarmOperRcd(param);
                                Result<List<AlarmFeedBack>> listFedBack = await policeCaseRepository.GetAlarmFeedBack(param);
                                alarmCollections.Add(item);
                                if (listOper != null)
                                {
                                    if (listOper.Code == ResultCode.SUCCESS)
                                    {
                                        alarmOper.RemoveAll(aa => aa.Jqlsh == item.Jqlsh);
                                        alarmOper.AddRange(listOper.Model);
                                    }
                                }
                                if (listOperRcd != null)
                                {
                                    if (listOperRcd.Code == ResultCode.SUCCESS)
                                    {
                                        alarmOperRcd.RemoveAll(aa => aa.Jqlsh == item.Jqlsh);
                                        alarmOperRcd.AddRange(listOperRcd.Model);
                                    }
                                }
                                if (listFedBack != null)
                                {
                                    if (listFedBack.Code == ResultCode.SUCCESS)
                                    {
                                        alarmFeedBack.RemoveAll(aa => aa.Jqlsh == item.Jqlsh);
                                        alarmFeedBack.AddRange(listFedBack.Model);
                                    }
                                }
                            }
                            /*
                             * 发布警情更新消息
                             */
                            MessageModel msg = new MessageModel();
                            try
                            {
                                msg.MessageType = (MessageTypes)Enum.Parse(typeof(MessageTypes), type.ToUpper());
                            }
                            catch (System.Exception)
                            {
                                msg.MessageType = MessageTypes.UNKONW;
                            }
                            msg.MessageBody = item;
                            _messageAgent.EventAggregator.GetEvent<CommonEventArgs>().Publish(msg);
                        }
                        #endregion
                        break;
                    //警力报备更新数据同步
                    case "dutyChange":

                        #region 报备更新
                        if (obj["result"] != null)
                        {
                            // string s = string.Format("{0}:{1}\r\n", System.DateTime.Now.ToString("hh:mm:ss"), obj["result"].ToString());
                            // System.IO.File.AppendAllText("D:\\LOG.TXT", s);
                            Patrol item = JsonConvert.DeserializeObject<Patrol>(obj["result"].ToString());

                            LogHelper.WriteLog(item.GroupNo + "--" + item.GroupName + "--" + item.AppStatus.ToString() + "--" + item.GroupStatus + "--" + item.Status);
                            bool tmp = patrols.Exists(aa => aa.GroupNo == item.GroupNo);
                            if (tmp)
                            {
                                patrols.RemoveAll(aa => aa.GroupNo == item.GroupNo);
                                patrols.Add(item);
                            }
                            else
                            {
                                if (!LocalUser.IsAllFunction)
                                {
                                    if (item.GroupNo.Contains(LocalUser.DeptNo.Substring(0, 8)))
                                    {
                                        patrols.Add(item);
                                    }
                                }
                                else
                                    patrols.Add(item);
                            }
                            /*
                             * 发布巡组状态变更
                             */
                            MessageModel msg = new MessageModel();
                            try
                            {
                                msg.MessageType = (MessageTypes)Enum.Parse(typeof(MessageTypes), type.ToUpper());
                            }
                            catch (System.Exception)
                            {
                                msg.MessageType = MessageTypes.UNKONW;
                            }
                            msg.MessageBody = item;
                            _messageAgent.EventAggregator.GetEvent<CommonEventArgs>().Publish(msg);
                        }
                        #endregion
                        break;
                    case "classChange":
                        UpdateReportInfo();
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(body);
                LogHelper.ErrorLog(ex, "DataCenter  ReceiveServiceMsg");
            }
        }

        private static async void UpdateReportInfo()
        {
            var policeGroupRepository = ServiceLocator.Current.GetInstance<IPoliceGroupRepository>();

            Result<List<Patrol>> patols = await policeGroupRepository.GetPatorl(departmentNo: !LocalUser.IsAllFunction ? LocalUser.DeptNo : string.Empty);
            if (patols.Code == ResultCode.SUCCESS)
            {
                List<Patrol> oItems = new List<Patrol>();
                foreach (var item in patols.Model)
                {
                    var findItem = oItems.Find(o => o.CallNo == item.CallNo);
                    if (findItem == null)
                    {
                        oItems.Add(item);
                    }
                }
                LocalDataCenter.SetPatrols(oItems);
            }
            MessageModel msg = new MessageModel();

            msg.MessageType = MessageTypes.GROUPSTATUS;

            msg.MessageBody = new Patrol();
            _messageAgent.EventAggregator.GetEvent<CommonEventArgs>().Publish(msg);
        }

        #region 设置数据
        /// <summary>
        /// 设置报警类别
        /// </summary>
        /// <param name="list"></param>
        public static void SetBjlb(List<DictItem> list)
        {
            if (list != null)
                bjlb = list;
        }

        public static void SetBjlx(List<DictItem> list)
        {
            if (list != null)
                bjlx = list;
        }

        public static void SetBjxl(List<DictItem> list)
        {
            if (list != null)
                bjxl = list;
        }

        public static void SetXiaQus(List<DictItem> list)
        {
            if (list != null)
            {
                list.Add(new DictItem() { code = "510108000000", note = "成华分局", detail = "成华分局" });
                xiaQus = new List<DictItem>(list.OrderBy(pp => pp.code));
            }
        }

        public static void SetPatrols(List<Patrol> list)
        {
            if (list != null)
                patrols = list;
        }

        public static void SetAlarmCollections(List<AlarmBase> list)
        {
            if (list != null)
                alarmCollections = list;
        }

        public static void SetAlarmOperRcd(List<AlarmOperationRecord> list)
        {
            if (list != null)
                alarmOperRcd = list;
        }

        public static void SetAlarmOper(List<AlarmOperation> list)
        {
            if (list != null)
                alarmOper = list;
        }

        public static void SetAlarmFdBack(List<AlarmFeedBack> list)
        {
            if (list != null)
                alarmFeedBack = list;
        }

        public static void SetLoginUser(User user)
        {
            LocalUser = user;
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 登录用户
        /// </summary>
        public static User GetLocalUser()
        {
            //User user = new User();
            //UtilsHelper.CopyEntity(user, LocalUser);
            //return user;
            return LocalUser;
        }
        /// <summary>
        /// 未完成警情集合
        /// </summary>
        public static List<AlarmBase> GetAlarmCollections()
        {
            return alarmCollections;
        }

        public static List<AlarmBase> UnFinishedAlarms
        {
            get
            {
                return alarmCollections;
            }
        }
        /// <summary>
        /// 未完成警情的操作记录
        /// </summary>
        public static List<AlarmOperationRecord> GetAlarmOperRcd(string jqlsh)
        {
            InitAlarmOperRcd(jqlsh);
            return alarmOperRcd.Where(o => o.Jqlsh == jqlsh).ToList();
        }
        private static void InitAlarmOperRcd(string jqlsh)
        {
            try
            {
                //判断jqlsh警情是否初始化操作记录
                AlarmBase alarm = alarmCollections.FirstOrDefault(aa => aa.Jqlsh == jqlsh);
                //alarm 为空报错
                if (alarm == null)
                    return;
                ///警情存在且没有初始化或者警情不存在
                //TODO:2016-9-6修改为每次查询都先去更新操作记录
                //if (!alarm.IsInitRcd)
                {
                    alarm.IsInitRcd = true;
                    Dictionary<string, object> param = new Dictionary<string, object>();
                    param.Add("jqlsh", jqlsh);
                    Result<List<AlarmOperationRecord>> listOperRcd = CommandCenter.ExcuteObjectSync<List<AlarmOperationRecord>>("getAlarmOptRcd", param);
                    if (listOperRcd == null)
                    {
                        return;
                    }
                    if (listOperRcd.Code == ResultCode.SUCCESS)
                    {
                        if (listOperRcd.Model != null)
                        {
                            alarmOperRcd.RemoveAll(aa => aa.Jqlsh == jqlsh);
                            alarmOperRcd.AddRange(listOperRcd.Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
            }

        }
        /// <summary>
        /// 未完成警情的处境记录
        /// </summary>
        public static List<AlarmOperation> GetAlarmOper(string jqlsh)
        {
            InitAlarmOper(jqlsh);
            return alarmOper.Where(o => o.Jqlsh == jqlsh).ToList();
        }
        private static void InitAlarmOper(string jqlsh)
        {
            //判断jqlsh警情是否初始化操作记录
            AlarmBase alarm = alarmCollections.FirstOrDefault(aa => aa.Jqlsh == jqlsh);
            if (alarm == null)
            {
                return;
            }
            ///警情存在且没有初始化或者警情不存在
            if (!alarm.IsInitExu)
            {
                alarm.IsInitExu = true;
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("jqlsh", jqlsh);
                Result<List<AlarmOperation>> listOper = CommandCenter.ExcuteObjectSync<List<AlarmOperation>>("getAlarmOperation", param);
                if (listOper.Code == ResultCode.SUCCESS)
                {
                    alarmOper.RemoveAll(aa => aa.Jqlsh == jqlsh);
                    alarmOper.AddRange(listOper.Model);
                }
            }
        }

        /// <summary>
        /// 未完成警情反馈记录
        /// </summary>
        public static List<AlarmFeedBack> GetAlarmFdBack(string jqlsh)
        {
            InitAlarmFdBack(jqlsh);
            return alarmFeedBack.Where(o => o.Jqlsh == jqlsh).ToList();
        }

        private static void InitAlarmFdBack(string jqlsh)
        {
            //判断jqlsh警情是否初始化操作记录
            AlarmBase alarm = alarmCollections.FirstOrDefault(aa => aa.Jqlsh == jqlsh);
            if (alarm == null)
                return;
            ///警情存在且没有初始化或者警情不存在
            if (alarm != null && !alarm.IsInitFdb)
            {
                alarm.IsInitFdb = true;
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("jqlsh", jqlsh);
                Result<List<AlarmFeedBack>> listFeb = CommandCenter.ExcuteObjectSync<List<AlarmFeedBack>>("searchFedBckAlarm", param);
                if (listFeb.Code == ResultCode.SUCCESS)
                {
                    if (listFeb.Model != null)
                    {
                        alarmFeedBack.RemoveAll(aa => aa.Jqlsh == jqlsh);
                        alarmFeedBack.AddRange(listFeb.Model);
                    }
                }
            }
        }
        /// <summary>
        /// 巡组巡防区域
        /// </summary>
        public static IList<Graphic> XiaQuPolygons { get; set; }


        /// <summary>
        /// 全部巡组
        /// </summary>
        public static List<Patrol> GetPatrols()
        {
            return patrols;
        }
        /// <summary>
        /// 获取辖区
        /// </summary>
        public static List<DictItem> GetXiaQus()
        {
            return xiaQus;
        }
        /// <summary>
        /// 报警类别
        /// </summary>
        public static List<DictItem> GetBjlb()
        {
            return bjlb;
        }

        /// <summary>
        /// 报警类型
        /// </summary>
        public static List<DictItem> GetBjlx()
        {
            return bjlx;
        }
        /// <summary>
        /// 报警细类
        /// </summary>
        public static List<DictItem> GetBjxl()
        {
            return bjxl;
        }

        /// <summary>
        /// 根据巡组编号获取对象
        /// </summary>
        /// <param name="PatrolNo"></param>
        /// <returns></returns>
        public static Patrol GetPatrol(string PatrolNo)
        {
            return patrols.FirstOrDefault(aa => aa.GroupNo == PatrolNo);
        }

        // <summary>
        /// 根据巡名称取对象第一个对象
        /// </summary>
        /// <param name="PatrolNo"></param>
        /// <returns></returns>
        public static Patrol GetPatrolByName(string PatrolName)
        {
            return patrols.FirstOrDefault(aa => aa.GroupName.Contains(PatrolName));
        }
        /// <summary>
        /// 派警计时集合
        /// </summary>
        public static List<SendAlarmTime> lstSendAlarmTime = new List<SendAlarmTime>();
        #endregion

    }
}
