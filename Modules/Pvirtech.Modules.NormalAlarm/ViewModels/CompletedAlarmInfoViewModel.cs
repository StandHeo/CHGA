using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    /// <summary>
    /// 已完成警情
    /// </summary>
    public class CompletedAlarmInfoViewModel : AlarmBase
    {
        private List<AlarmFeedBack> feedBacks;
        /// <summary>
        /// 全部反馈记录
        /// </summary>
        public List<AlarmFeedBack> FeedBacks
        {
            get { return feedBacks; }
            set 
            { 
                feedBacks = value;
                SplitFeedBacks(FeedBacks);
            }
        }

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

        #region 管辖单位扩展字段

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
                    return value.detail.Substring(0, 2);
                return "- -";
            }
        }

        #endregion

        #region 扩展报警日期/时间/日期时间

        /// <summary>
        /// 报警时间
        /// </summary>
        public string BjriOrBjsjDesc
        {
            get
            {
                if (string.IsNullOrEmpty(Jjsj))
                    return "- -";
                return DateTime.ParseExact(Jjsj, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        
        /// <summary>
        /// 结案时间
        /// </summary>
        public string BjriOrJasjDesc
        {
            get
            {
                if (string.IsNullOrEmpty(Jasj))
                    return "- -";
                return DateTime.ParseExact(Jasj, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        #endregion

        #region 报警方式
        
        public string BjfsDesc
        {
            get
            {
                AllEnum.PolSource p_Sort = (AllEnum.PolSource)Bjfsbh;
                return p_Sort.ToString();
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

        /// <summary>
        /// PDA反馈信息
        /// </summary>
        public List<AlarmFeedBack> PDAFeedBacks
        {
            get;
            set;
        }

        /// <summary>
        /// 处警记录
        /// </summary>
        public List<AlarmOperationRecord> OperRecord
        {
            get;
            set;
        }

       

        #region 扩展方法

        public void SplitFeedBacks(List<AlarmFeedBack> lit)
        {
            #region 参数验证

            if (lit == null)
            {
                return;
            }

            #endregion

            #region 执行操作

            PDAFeedBacks = lit.Where(M => M.PcOrPDA == 1).ToList();

            #endregion

        }

        #endregion

    }

}
