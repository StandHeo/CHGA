using Pvirtech.Model;
using Pvirtech.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    /// <summary>
    /// 未完成警情
    /// </summary>
    public class WorkingAlarmInfoViewModel :MAlarmInfo
    {
        public WorkingAlarmInfoViewModel()
        {
        }
        public WorkingAlarmInfoViewModel(WorkingAlarmInfoViewModel old)
            : base(old as MAlarmInfo)
        {
         
        }

        private bool isOpenWin;

        /// <summary>
        /// 是否打开窗体
        /// </summary>
        public bool IsOpenWin
        {
            get { return isOpenWin; }
            set 
            { 
                isOpenWin = value;
                OnPropertyChanged(() => IsOpenWin);
            }
        }

        private bool isLinkAge;
        /// <summary>
        /// 是否已转大联动
        /// </summary>
        public bool IsLinkAge
        {
            get { return isLinkAge; }
            set
            {
                if (isLinkAge != value)
                {
                    isLinkAge = value;
                    OnPropertyChanged(() => IsLinkAge);
                }
            }
        }

        private bool isUrge;

        /// <summary>
        /// 是否催促
        /// </summary>
        public bool IsUrge
        {
            get { return isUrge; }
            set 
            { 
                isUrge = value;
                OnPropertyChanged(() => IsUrge);
            }
        }

        /// <summary>
        /// 全部巡组
        /// </summary>
        public List<Patrol> VisiblePartol
        {
            get 
            { 
                List<Patrol> patrols= LocalDataCenter.GetPatrols();
                return patrols.Where(aa => aa.GroupType == 2001 || aa.GroupType == 1004).ToList();
            }
        }

        private string thisStageDate;

        /// <summary>
        /// 当前阶段计时
        /// </summary>
        public string ThisStageDate
        {
            get { return thisStageDate; }
            set 
            { 
                thisStageDate = value;
                OnPropertyChanged("ThisStageDate");
            }
        }

        private string totalDate;

        /// <summary>
        /// 总计时
        /// </summary>
        public string TotalDate
        {
            get { return totalDate; }
            set 
            { 
                totalDate = value;
                OnPropertyChanged("TotalDate");
            }
        }


        private List<Patrol> _SelectObj;

        /// <summary>
        /// 当前选中的巡组
        /// </summary>
        public List<Patrol> SelectObj
        {
            get { return _SelectObj; }
            set { _SelectObj = value; }
        }

        /// <summary>
        /// 催促的巡组
        /// </summary>
        public List<MAlarmPatrol> UrgePatrol { get; set; }

        /// <summary>
        /// 选择到达的巡组
        /// </summary>
        public List<MAlarmPatrol> ArrivedPatrol { get; set; }

        /// <summary>
        /// 选择出发巡组
        /// </summary>
        public List<MAlarmPatrol> DepartPatrol { get; set; }

        /// <summary>
        /// 选择接收的巡组
        /// </summary>
        public List<MAlarmPatrol> ReceivePatrol { get; set; }

        private string _text;

        public string SelectPatrolText
        {
            get
                {
                    return _text;
                }
            set
              {
                _text = value;
              }
        }


    


        #region 业务处理方法

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
        public async Task<Result<bool>> ExcuteMession(string cjcs, string cjdwmz, string cjdwbh, int allowEnd, int is2PDA)
        {
            Dictionary<string, object> param = new Dictionary<string,object>();
            User local = LocalDataCenter.GetLocalUser();
            param.Add("jqlsh", Jqlsh);
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
        public async Task<Result<bool>> CloseMession(string jams)
        {
            Dictionary<string, object> param = new Dictionary<string,object>();
            param.Add("jqlsh", Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "5");
            param.Add("jams", jams);
            param.Add("jqly",Jqly);
            param.Add("jjdbh", Jjdbh);
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
        public async Task<Result<bool>> ReturnAlarmByText(string jams)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "5");
            param.Add("jams", jams);
            param.Add("jqly", Jqly);
            param.Add("jjdbh", Jjdbh);
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
        public async Task<Result<bool>> ArriveMession(string groupNo,string No)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "4");
            param.Add("groupNo", groupNo);
            return await CommandCenter.ExcuteObject<bool>("dealCasePC", param);
        }

        /// <summary>
        /// 接受
        /// jqlsh:警情流水号
        /// cjybh:处警员编号
        /// cjyxm:处警员姓名
        /// cjzt:处理警情状态 2:接受/3出发/4到达/5结案
        /// groupNo:执勤组编号
        /// </summary>
        public async Task<Result<bool>> AcceptMession(string groupNo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            param.Add("cjzt", "2");
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
        public async Task<Result<bool>> StartMession(string groupNo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", Jqlsh);
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
        public async Task<Result<bool>> FeedBackMession(string fknr,string cjdwbh, string cjdwmz,string saje,string bjxl,string bjlb,string bjlx,int isMian)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("jqlsh", Jqlsh);
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
        public async Task<Result<bool>> SendAlarmInfoDldByNo()
        {
            Dictionary<string,object> param = new Dictionary<string,object>();
            param.Add("jqlsh",Jqlsh);
            param.Add("cjybh", LocalDataCenter.GetLocalUser().UserNo);
            param.Add("cjyxm", LocalDataCenter.GetLocalUser().UserName);
            return await CommandCenter.ExcuteObject<bool>("send2Dld", param);
        }

        /// <summary>
        /// 催促警情
        /// </summary>
        /// <param name="objNo">催促对象Id</param>
        /// <returns></returns>
        public async Task<Result<bool>> UrgeAlramByIdOrText(string objNo,string v)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("groupNo", objNo);
            param.Add("jqlsh", Jqlsh);
            param.Add("ccnr", Bjnr+"，请你速度赶往事发地址，处置警情");
            param.Add("is2PDA", v);
            param.Add("cjr", LocalDataCenter.GetLocalUser().UserName);
            return await CommandCenter.ExcuteObject<bool>("urgeAlarmUnit", param);
        }

        #endregion

    }
}
