using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Pvirtech.Services
{
    /// <summary>
    /// 数据中心
    /// 维护本地数据的唯一类
    /// </summary>
    public class LocalDataCenter : LocalData
    {

        public static ZhiYinXMLModel ZhiYinXML { get; set; }
        public static void SetZhiYin()
        {
            ZhiYinXML = XmlHelper.GetXmlEntities<ZhiYinXMLModel>("Alarm.Zhiyinguifan.xml");
        }

        /// <summary>
        /// 接收服务端推送消息
        /// </summary>
        

       

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
        //public static IList<Graphic> XiaQuPolygons { get; set; }


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
