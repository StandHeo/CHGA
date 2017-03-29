using Pvirtech.Model;
using System.Collections.Generic;

namespace Pvirtech.Services
{
    /// <summary>
    /// 本地资源数据
    /// </summary>
    public class LocalData
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        protected static User LocalUser { get; set; }

        protected static List<AlarmBase> alarmCollections = new List<AlarmBase>();

        protected static List<AlarmOperationRecord> alarmOperRcd = new List<AlarmOperationRecord>();

        protected static List<AlarmOperation> alarmOper = new List<AlarmOperation>();

        protected static List<AlarmFeedBack> alarmFeedBack = new List<AlarmFeedBack>();
        /// <summary>
        /// 全部巡组
        /// </summary>
        protected static List<Patrol> patrols = new List<Patrol>();
        /// <summary>
        /// 获取辖区
        /// </summary>
        protected static List<DictItem> xiaQus = new List<DictItem>();
        /// <summary>
        /// 报警类别
        /// </summary>
        protected static List<DictItem> bjlb = new List<DictItem>();
        /// <summary>
        /// 报警类型
        /// </summary>
        protected static List<DictItem> bjlx = new List<DictItem>();

        /// <summary>
        /// 报警细类
        /// </summary>
        protected static List<DictItem> bjxl = new List<DictItem>();
         
    }
}
