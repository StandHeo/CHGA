using Prism.Mvvm;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    /// <summary>
    /// 警情基础ViewModel
    /// </summary>
    public class MAlarmInfo : BindableBase
    {
        public MAlarmInfo()
        {

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="old"></param>
        public MAlarmInfo(MAlarmInfo old)
        {
            var ParentType = typeof(MAlarmInfo);
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    Propertie.SetValue(this, Propertie.GetValue(old, null), null);
                }
            }
        }

        #region Model

        /**流转大联动状态 -1：流转超时 0：流转失败 1:流转中，2：流转成功 3：结案*/
        private int _dldStatus;

        public int DldStatus
        {
            get
            {
                return _dldStatus;
            }
            set
            {
                _dldStatus = value;
                OnPropertyChanged(() => DldStatus);
                OnPropertyChanged(() => DescDldStatus);
                OnPropertyChanged(() => ShowBusyContent);
                OnPropertyChanged(() => NotShowBusyContent);
            }
        }

        /**流转大联动成功失败消息*/
        private string _dldInfo;

        public string DldInfo
        {
            get
            {
                return _dldInfo;
            }

            set
            {
                _dldInfo = value;
                OnPropertyChanged(() => DldInfo);
            }
        }

        /**是否流转大联动  1：流转*/
        private int _isDld;
        public int IsDld
        {
            get
            {
                return _isDld;
            }

            set
            {
                _isDld = value;
                OnPropertyChanged(() => IsDld);
                OnPropertyChanged(() => DescAjzt);
            }
        }

        private string _dldCloseInfo;

        public string DldCloseInfo
        {
            get
            {
                return _dldCloseInfo;
            }

            set
            {
                _dldCloseInfo = value;
                OnPropertyChanged(() => DldCloseInfo);
            }
        }

        /**流转大联动操作人姓名*/
        private string _dldOpUserName;

        public string DldOpUserName
        {
            get
            {
                return _dldOpUserName;
            }

            set
            {
                _dldOpUserName = value;
                OnPropertyChanged(() => DldOpUserName);
            }
        }

        /**流转大联动时间*/
        private string _dldTime;

        public string DldTime
        {
            get
            {
                return _dldTime;
            }

            set
            {
                _dldTime = value;
                OnPropertyChanged(() => DldTime);
            }
        }

        public Visibility ShowBusyContent
        {
            get
            {
                bool rs = IsDld == 1;
                if (IsDld != 1)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return DldStatus == 1 ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public Visibility NotShowBusyContent
        {
            get
            {
                bool rs = IsDld == 1;
                if (IsDld != 1)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return DldStatus == 1 ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }


        private long _id;

        /// <summary>
        /// Id
        /// </summary>
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }

        private string jqlsh;

        /// <summary>
        /// 内部警情流水号
        /// </summary>
        public string Jqlsh
        {
            get { return jqlsh; }
            set
            {
                jqlsh = value;
                OnPropertyChanged(() => Jqlsh);
            }
        }

        private int jqly;

        /// <summary>
        /// 警情来源
        /// </summary>
        public int Jqly
        {
            get { return jqly; }
            set
            {
                jqly = value;
                OnPropertyChanged(() => Jqly);
                OnPropertyChanged(() => JqlyDesc);
                OnPropertyChanged(() => Jqlyjc);
            }
        }

        private string jjdbh;

        /// <summary>
        /// 警情来源
        /// </summary>
        public string Jjdbh
        {
            get { return jjdbh; }
            set
            {
                jjdbh = value;
                OnPropertyChanged(() => Jjdbh);
            }
        }

        private string jjdwbh;

        /// <summary>
        /// 接警单位编号
        /// </summary>
        public string Jjdwbh
        {
            get { return jjdwbh; }
            set
            {
                jjdwbh = value;
                OnPropertyChanged(() => Jjdwbh);
            }
        }

        private string jjybh;

        /// <summary>
        /// 接警员编号
        /// </summary>
        public string Jjybh
        {
            get { return jjybh; }
            set
            {
                jjybh = value;
                OnPropertyChanged(() => Jjybh);
            }
        }

        private string _jjyxm;

        /// <summary>
        /// 接警员姓名
        /// </summary>
        public string Jjyxm
        {
            get { return _jjyxm; }
            set
            {
                _jjyxm = value;
                OnPropertyChanged(() => Jjyxm);
            }
        }

        private string _jjsj;
        /// <summary>
        /// 接警时间
        /// </summary>
        public string Jjsj
        {
            get { return _jjsj; }
            set
            {
                _jjsj = value;
                OnPropertyChanged(() => Jjsj);
                OnPropertyChanged(() => BjrqDesc);
                OnPropertyChanged(() => BjsjDesc);
                OnPropertyChanged(() => BjriOrBjsjDesc);
            }
        }

        private int bjfsbh;

        public int Bjfsbh
        {
            get { return bjfsbh; }
            set
            {
                bjfsbh = value;
                OnPropertyChanged(() => Bjfsbh);
            }
        }

        private string bjdh;

        public string Bjdh
        {
            get { return bjdh; }
            set
            {
                bjdh = value;
                OnPropertyChanged(() => Bjdh);
            }
        }

        private string bjdhyhxm;

        public string Bjdhyhxm
        {
            get { return bjdhyhxm; }
            set
            {
                bjdhyhxm = value;
                OnPropertyChanged(() => Bjdhyhxm);
            }
        }

        private string bjdhyhdz;

        public string Bjdhyhdz
        {
            get { return bjdhyhdz; }
            set
            {
                bjdhyhdz = value;
                OnPropertyChanged(() => Bjdhyhdz);
            }
        }

        private string bjrxm;

        public string Bjrxm
        {
            get { return bjrxm; }
            set
            {
                bjrxm = value;
                OnPropertyChanged(() => Bjrxm);
            }
        }

        private int bjrxb;

        public int Bjrxb
        {
            get { return bjrxb; }
            set
            {
                bjrxb = value;
                OnPropertyChanged(() => Bjrxb);
                OnPropertyChanged(() => BjrxbDesc);
            }
        }

        private string lxdh;

        public string Lxdh
        {
            get { return lxdh; }
            set
            {
                lxdh = value;
                OnPropertyChanged(() => Lxdh);
            }
        }

        private string zagj;

        public string Zagj
        {
            get { return zagj; }
            set
            {
                zagj = value;
                OnPropertyChanged(() => Zagj);
            }
        }

        private string _gxdwbh;

        /// <summary>
        /// 管辖单位编号
        /// </summary>
        public string Gxdwbh
        {
            get { return _gxdwbh; }
            set
            {
                _gxdwbh = value;
                OnPropertyChanged(() => Gxdwmc);
                OnPropertyChanged(() => Gxdwjc);
            }
        }

        private double _zddwxzb;

        /// <summary>
        /// 自动定位X坐标
        /// </summary>
        public double Zddwxzb
        {
            get { return _zddwxzb; }
            set
            {
                _zddwxzb = value;
                OnPropertyChanged(() => Zddwxzb);
            }
        }

        private double _zddwyzb;

        /// <summary>
        /// 自动定位Y坐标
        /// </summary>
        public double Zddwyzb
        {
            get { return _zddwyzb; }
            set
            {
                _zddwyzb = value;
                OnPropertyChanged(() => Zddwyzb);
            }
        }

        private string _bjlb;

        /// <summary>
        /// 报警类别
        /// </summary>
        public string Bjlb
        {
            get { return _bjlb; }
            set
            {
                _bjlb = value;
                OnPropertyChanged(() => Bjlb);
                OnPropertyChanged(() => BjlbDesc);
                OnPropertyChanged(() => NatureFormat);
                OnPropertyChanged(() => BjlbjcDesc);
                OnPropertyChanged(() => BjlxjcDesc);
                OnPropertyChanged(() => BjxljcDesc);
            }
        }

        private string _bjlx;

        /// <summary>
        /// 报警类型
        /// </summary>
        public string Bjlx
        {
            get { return _bjlx; }
            set
            {
                _bjlx = value;
                OnPropertyChanged(() => Bjlx);
                OnPropertyChanged(() => BjlxDesc);
                OnPropertyChanged(() => NatureFormat);
            }
        }

        private string _bjxl;

        /// <summary>
        /// 报警细类
        /// </summary>
        public string Bjxl
        {
            get { return _bjxl; }
            set
            {
                _bjxl = value;
                OnPropertyChanged(() => Bjxl);
                OnPropertyChanged(() => BjxlDesc);
                OnPropertyChanged(() => NatureFormat);
            }
        }

        private string rcsjc;

        public string Rcsjc
        {
            get { return rcsjc; }
            set
            {
                rcsjc = value;
                OnPropertyChanged(() => Rcsjc);
            }
        }

        private int isdelete;

        public int Isdelete
        {
            get { return isdelete; }
            set
            {
                isdelete = value;
                OnPropertyChanged(() => Isdelete);
            }
        }

        private string _bjnr;

        /// <summary>
        /// 报警内容 
        /// </summary>
        public string Bjnr
        {
            get { return _bjnr; }
            set
            {
                _bjnr = value;
                OnPropertyChanged(() => Bjnr);
            }
        }

        private string _sfdz;

        /// <summary>
        /// 事发地址
        /// </summary>
        public string Sfdz
        {
            get { return _sfdz; }
            set
            {
                _sfdz = value;
                OnPropertyChanged(() => Sfdz);
            }
        }

        private int _ajzt;

        public int Ajzt
        {
            get { return _ajzt; }
            set
            {
                _ajzt = value;
                OnPropertyChanged(() => Ajzt);
                OnPropertyChanged(() => DescAjzt);
                OnPropertyChanged(() => ColorAjzt);
            }
        }

        private int ywbkry;

        public int Ywbkry
        {
            get { return ywbkry; }
            set
            {
                ywbkry = value;
                OnPropertyChanged(() => Ywbkry);
            }
        }

        private int ywwxwz;

        public int Ywwxwz
        {
            get { return ywwxwz; }
            set
            {
                ywwxwz = value;
                OnPropertyChanged(() => Ywwxwz);
            }
        }

        private int ywbzw;

        public int Ywbzw
        {
            get { return ywbzw; }
            set
            {
                ywbzw = value;
                OnPropertyChanged(() => Ywbzw);
            }
        }

        private double sddwyzb;

        public double Sddwyzb
        {
            get { return sddwyzb; }
            set
            {
                sddwyzb = value;
                OnPropertyChanged(() => Sddwyzb);
            }
        }

        private double sddwxzb;

        private string pDAJanr;

        /// <summary>
        /// PDA结案内容
        /// </summary>
        public string PDAJanr
        {
            get { return pDAJanr; }
            set
            {
                pDAJanr = value;
                OnPropertyChanged(() => PDAJanr);
            }
        }
        public double Sddwxzb
        {
            get { return sddwxzb; }
            set
            {
                sddwxzb = value;
                OnPropertyChanged(() => Sddwxzb);
            }
        }

        private string zhgxrxm;

        public string Zhgxrxm
        {
            get { return zhgxrxm; }
            set
            {
                zhgxrxm = value;
                OnPropertyChanged(() => Zhgxrxm);
            }
        }

        private string zhgxrbh;

        public string Zhgxrbh
        {
            get { return zhgxrbh; }
            set
            {
                zhgxrbh = value;
                OnPropertyChanged(() => Zhgxrbh);
            }
        }

        private int zhgxqd;

        public int Zhgxqd
        {
            get { return zhgxqd; }
            set
            {
                zhgxqd = value;
                OnPropertyChanged(() => Zhgxqd);
            }
        }

        private string zhgxsj;

        public string Zhgxsj
        {
            get { return zhgxsj; }
            set
            {
                zhgxsj = value;
                OnPropertyChanged(() => Zhgxsj);
            }
        }

        private string jams;

        public string Jams
        {
            get { return jams; }
            set
            {
                jams = value;
                OnPropertyChanged(() => Jams);
            }
        }

        private string jarxm;

        public string Jarxm
        {
            get { return jarxm; }
            set
            {
                jarxm = value;
                OnPropertyChanged(() => Jarxm);
            }
        }

        private string jarbh;

        public string Jarbh
        {
            get { return jarbh; }
            set
            {
                jarbh = value;
                OnPropertyChanged(() => Jarbh);
            }
        }

        private string jasj;

        public string Jasj
        {
            get { return jasj; }
            set
            {
                jasj = value;
                OnPropertyChanged(() => Jasj);
            }
        }

        private int caseLevel;

        public int CaseLevel
        {
            get { return caseLevel; }
            set
            {
                caseLevel = value;
                OnPropertyChanged(() => CaseLevel);
                OnPropertyChanged(() => CaseLevelDesc);
            }
        }

        private int allowEnd;

        public int AllowEnd
        {
            get { return allowEnd; }
            set
            {
                allowEnd = value;
                OnPropertyChanged(() => AllowEnd);
            }
        }

    
        private bool showOne;

        /// <summary>
        /// 是否打开窗体
        /// </summary>
        public bool ShowOne
        {
            get { return showOne; }
            set
            {
                showOne = value;
                OnPropertyChanged(() => ShowOne);
            }
        }

        private bool showSecond;

        /// <summary>
        /// 是否打开窗体
        /// </summary>
        public bool ShowSecond
        {
            get { return showSecond; }
            set
            {
                showSecond = value;
                OnPropertyChanged(() => ShowSecond);
            }
        }

        public List<Info> ZYItems
        {
            get
            {
                foreach (var zyItem in LocalDataCenter.ZhiYinXML.ZYItems)
                {
                    if (IsSame(BjxljcDesc, zyItem.AlarmTypeDetail))
                    {
                        return zyItem.Infos;
                    }
                }

                foreach (var zyItem in LocalDataCenter.ZhiYinXML.ZYItems)
                {
                    if (IsSame(BjlxjcDesc, zyItem.AlarmType))
                    {
                        return zyItem.Infos;
                    }
                }

                var defaultInfo = LocalDataCenter.ZhiYinXML.AlarmLevels.FirstOrDefault(a => a.Name == CaseLevelDesc);
                if (defaultInfo != null)
                {
                    var dfRs = new List<Info>();
                    var val = defaultInfo.LocaleResources.Find(a => a.Name == "到达").Value;
                    dfRs.Add(new Info { Short = val, Detail = val });
                    return dfRs;
                }

                return null;

            }
        }

        private bool showThree;


        private bool IsSame(string value, string other)
        {
            var val1 = value.Trim().ToUpper().Replace(" ", "");
            var val2 = other.Trim().ToUpper().Replace(" ", "");
            if (val1.Contains(val2) || val2.Contains(val1))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否打开窗体
        /// </summary>
        public bool ShowThree
        {
            get { return showThree; }
            set
            {
                showThree = value;
                OnPropertyChanged(() => ShowThree);
            }
        }

        private bool showFour;

        /// <summary>
        /// 是否打开窗体
        /// </summary>
        public bool ShowFour
        {
            get { return showFour; }
            set
            {
                showFour = value;
                OnPropertyChanged(() => ShowFour);
            }
        }

        #endregion

        #region 反馈记录

        private List<AlarmFeedBack> _FdBacklit;

        public List<AlarmFeedBack> FdBacklit
        {
            get { return _FdBacklit; }
            set
            {
                _FdBacklit = value;
                OnPropertyChanged(() => FdBacklit);
            }
        }

        #endregion

  

        #region 扩展管辖单位名称

        /// <summary>
        /// 管辖单位名称
        /// </summary>
        public string Gxdwmc
        {
            get
            {
                var value = LocalDataCenter.GetXiaQus().FirstOrDefault(L => L.code == this.Gxdwbh);
                if (value != null)
                    return value.detail;
                return "- -";
            }
        }

        public string Gxdwjc
        {
            get
            {
                var value = LocalDataCenter.GetXiaQus().FirstOrDefault(L => L.code == this.Gxdwbh);
                if (value != null)
                {
                    if (value.detail != null)
                    {
                        return value.detail.Substring(0, 2);
                    }
                }
                return "- -";
            }
        }

        #endregion

        #region 扩展类别/类型/细类

        /// <summary>
        /// 报警类别名称
        /// </summary>
        public string BjlbDesc
        {
            get
            {
                var value = LocalDataCenter.GetBjlb().FirstOrDefault(B => B.code == Bjlb);
                if (value != null)
                    return value.note;
                return "- -";
            }
        }

        /// <summary>
        /// 报警类别简称
        /// </summary>
        public string BjlbjcDesc
        {
            get
            {
                var value = LocalDataCenter.GetBjlb().FirstOrDefault(B => B.code == Bjlb);
                if (value != null)
                    return value.note.Substring(0, 2);
                return "- -";
            }
        }

        /// <summary>
        /// 报警类型名称
        /// </summary>
        public string BjlxDesc
        {
            get
            {
                var value = LocalDataCenter.GetBjlx().FirstOrDefault(B => B.code == Bjlx);
                if (value != null)
                    return value.note;
                return "- -";
            }
        }

        /// <summary>
        /// 报警类型简称
        /// </summary>
        public string BjlxjcDesc
        {
            get
            {
                var value = LocalDataCenter.GetBjlx().FirstOrDefault(B => B.code == Bjlx);

                if (value != null)
                    return value.note.Substring(0, 2);
                return "- -";
            }
        }

        /// <summary>
        /// 报警细类名称
        /// </summary>
        public string BjxlDesc
        {
            get
            {
                var value = LocalDataCenter.GetBjxl().FirstOrDefault(B => B.code == Bjxl);
                if (value != null)
                    return value.note;
                return "- -";
            }
        }

        /// <summary>
        /// 报警细类简称
        /// </summary>
        public string BjxljcDesc
        {
            get
            {
                var value = LocalDataCenter.GetBjxl().FirstOrDefault(B => B.code == Bjxl);
                if (value != null)
                    return value.note.Substring(0, 2);
                return "- -";
            }
        }


        /// <summary>
        /// 格式化类别/类型/细类
        /// </summary>
        public string NatureFormat
        {
            get
            {
                return BjlbDesc + "/" + BjlxDesc + "/" + BjxlDesc;
            }
        }

        #endregion

        #region 扩展警情级别字段

        public string CaseLevelDesc
        {
            get
            {
                switch (CaseLevel)
                {
                    case 1:
                        return "一级";
                    case 2:
                        return "二级";
                    case 3:
                        return "三级";
                    default:
                        return "- -";
                }
            }
        }

        #endregion

        #region 流转至大联动状态

        public string DescDldStatus
        {
            get
            {
                switch (DldStatus)
                {
                    case -1:
                        return "流转失败";
                    case 0:
                        return "流转失败";
                    case 1:
                        return "流转中";
                    case 2:
                        return "流转成功";
                    case 3:
                        return "已结案";
                    default:
                        return " ";
                }
            }
        }

        #endregion

        #region 警情状态显示名称

        public string DescAjztAttach
        {
            get
            {
                if (IsDld == 1)
                {
                    return "已流转";
                }
                return DescAjzt;
            }
        }

        public string DescAjzt
        {
            get
            {
                var value = (AlarmStatus)Enum.Parse(typeof(AlarmStatus), this.Ajzt.ToString());
                switch (value)
                {
                    case AlarmStatus.NEWALARM:
                        return "新警情";
                    case AlarmStatus.EXCUTE:
                        return "已派警";
                    case AlarmStatus.RECIVE:
                        return "PDA接收";
                    case AlarmStatus.OTHER:
                        return "警力接受";
                    case AlarmStatus.START:
                        return "已出发";
                    case AlarmStatus.ARRIVE:
                        return "已到达";
                    case AlarmStatus.PDAFULFILL:
                        return "警力结案";
                    case AlarmStatus.FEEDBACK:
                        return "已反馈";
                    case AlarmStatus.END:
                        return "已结案";
                    default:
                        return "- -";
                }
            }
        }
        public string ColorAjzt
        {
            get
            {
                var value = (AlarmStatus)Enum.Parse(typeof(AlarmStatus), this.Ajzt.ToString());
                switch (value)
                {
                    case AlarmStatus.NEWALARM:
                    case AlarmStatus.EXCUTE:
                        return "Red";
                    case AlarmStatus.RECIVE:
                    case AlarmStatus.OTHER:
                        return "#FF9c33";
                    case AlarmStatus.START:
                        return "Gold";
                    case AlarmStatus.ARRIVE:
                        return "#00DE1F";
                    case AlarmStatus.FEEDBACK:
                        return "#1300DB";
                    case AlarmStatus.PDAFULFILL:
                        return "#747474";
                    case AlarmStatus.END:
                        return "Black";
                    default:
                        return "Red";
                }
            }
        }

        #endregion

        #region 扩展报警日期/时间/日期时间

        public string BjrqDesc
        {
            get
            {
                if (string.IsNullOrEmpty(Jjsj))
                    return "- -";
                return DateTime.ParseExact(Jjsj, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd");
            }
        }

        public string BjsjDesc
        {
            get
            {
                if (string.IsNullOrEmpty(Jjsj))
                    return "- -";
                return DateTime.ParseExact(Jjsj, "yyyyMMddHHmmss", null).ToString("HH:mm:ss");
            }
        }

        public string BjriOrBjsjDesc
        {
            get
            {
                if (string.IsNullOrEmpty(Jjsj))
                    return "- -";
                return DateTime.ParseExact(Jjsj, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        #endregion

        #region 扩展警情来源

        public string JqlyDesc
        {
            get
            {
                switch (this.jqly)
                {
                    case 0:
                        return "市局";
                    case 1:
                        return "大联动";
                    case 2:
                        return "分局";
                    default:
                        return "- -";
                }
            }
        }

        public string Jqlyjc
        {
            get
            {
                switch (this.jqly)
                {
                    case 0:
                        return "市";
                    case 1:
                        return "大";
                    case 2:
                        return "分";
                    default:
                        return "- -";
                }
            }
        }

        #endregion

        #region 报警人性别

        public string BjrxbDesc
        {
            get
            {
                if (Bjrxb == 0)
                    return "男";
                else
                    return "女";
            }
        }

        #endregion

        #region 流转至大联动状态


        #endregion

        public AlarmPatrolList UsePatrols
        { get; set; }

        public LogInfoViewModel LogInfo
        { get; set; }

        public AlarmStatusViewModel RunStatus
        {
            get;
            set;
        }

        public void Init()
        {
            try
            {
                FdBacklit = LocalDataCenter.GetAlarmFdBack(Jqlsh);

                LogInfo = new LogInfoViewModel();
                LogInfo.Init(jqlsh);
                OnPropertyChanged("LogInfo");


                UsePatrols = new AlarmPatrolList();
                UsePatrols.Init(this);
                OnPropertyChanged("UsePatrols");

                /*
                    初始化警力
                */
                PatrolInit();

                RunStatus = new AlarmStatusViewModel();
                RunStatus.Init(this);
                OnPropertyChanged("RunStatus");
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "MAlarmInit");
            }
        }

        public void PatrolInit()
        {
            SelectPatrols = new SelectPatrolViewModel();
            SelectPatrols.Init(this);
            OnPropertyChanged("SelectPatrols");
        }

        public SelectPatrolViewModel SelectPatrols
        { get; set; }


    }
}
