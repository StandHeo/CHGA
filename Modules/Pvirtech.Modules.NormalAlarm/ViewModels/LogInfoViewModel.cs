using Prism.Mvvm;
using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    public class LogInfoViewModel:BindableBase
    {
        private string _InfoText;
        public string InfoText
        {
            get
            {
                return _InfoText;
            }
            set
            {
                if (_InfoText != value)
                    _InfoText = value;
                OnPropertyChanged("InfoText");
            }
        }

        /// <summary>
        /// PDA反馈
        /// </summary>
        public string PDAFanKuiText
        {
            get
            {
                return _PDAFanKuiText;
            }

            set
            {
                _PDAFanKuiText = value;
            }
        }

        /// <summary>
        /// 全部反馈信息
        /// </summary>
        public string AllFanKui
        {
            get
            {
                return allFanKui;
            }
            set
            {
                allFanKui = value;
            }
        }

        private string logText;

        /// <summary>
        /// 操作记录
        /// </summary>
        public string LogText
        {
            get
            {
                return logText;
            }

            set
            {
                logText = value;
            }
        }

        private string allFanKui;
        
        private string _PDAFanKuiText;
       
        public void Init(string key)
        {
            StringBuilder sb=new StringBuilder();
            //string logformat=GlobalConfig.GetInstance().LogInfo.Replace("@","\r\n");
            //string fkformat = GlobalConfig.GetInstance().FkInfo.Replace("@", "\r\n");
            string logformat = "";
            string fkformat = "";
            string sTime = "";
            string StartName = "";
            string TargetName = "";
         //   设备接收 = 1,
         //接受警情 = 2,
         //出发 = 3,
         //已到达现场 = 4,
         //处理完成 = 5,
         //催促尽快处理警情 = 6,
         //派警 = 7,
         //操作违规 = 8,
         //修改单兵能否结案 = 9,
         //作废巡组 = 10
            //logformat = logformat.Replace("#", "\t");
            //fkformat = fkformat.Replace("#", "\t");

            List<AlarmOperationRecord> LogList = LocalDataCenter.GetAlarmOperRcd(key).OrderBy(aa=>aa.StartTime).ToList();
            if (LogList!=null)
            {
                foreach (var item in LogList)
                {
                    
                    switch (item.Dzlx)
                    {
                        case 3:
                            logformat = "\t{0},{1} 出发";
                            
                            break;
                        case 2:
                            logformat = "\t{0},{1} 接受";
                            break;
                        case 7:
                            logformat = "派警\r\n\t{0},{1} 派警给 {2} 执行";
                            break;
                        case 6:
                            logformat = "催促\r\n\t{0},{2} 催促 {1}";
                            break;
                        case 4:
                            logformat = "到达\r\n\t{0},{1} 到达";
                            break;
                        default:
                            continue;
                    }

                    if (!string.IsNullOrEmpty(item.StartTime))
                        sTime = DateTime.ParseExact(item.StartTime, "yyyyMMddHHmmss", null).ToString("HH:mm:ss");

                    if (string.IsNullOrEmpty(item.StartName) || item.StartName == "null")
                        StartName = "";
                    else
                        StartName = item.StartName;

                    if (string.IsNullOrEmpty(item.TargetName) || item.TargetName == "null")
                        TargetName = "";
                    else
                        TargetName = item.TargetName;

                    sb.AppendFormat(logformat, sTime,StartName, TargetName);
                    sb.AppendLine();
                }
                LogText = sb.ToString();
            }


            List<AlarmFeedBack> FkList=LocalDataCenter.GetAlarmFdBack(key).OrderBy(aa=>aa.Fksj).ToList();
            sb.AppendLine("反馈");

            if (FkList != null && FkList.Count!=0)
            {
                StringBuilder newsb = new StringBuilder();
                StringBuilder pdasb = new StringBuilder();
                fkformat = "\t{0},{1}反馈:{2}\r\n";
                foreach (var item in FkList)
                {
                    if (!string.IsNullOrEmpty(item.Fksj))
                        sTime = DateTime.ParseExact(item.Fksj, "yyyyMMddHHmmss", null).ToString("HH:mm:ss");
                    if (item.PcOrPDA == 1)
                        //如果是PDA反馈
                        pdasb.AppendFormat(fkformat, sTime, item.Fkrmz, item.Fknr);
                    sb.AppendFormat(fkformat, sTime, item.Fkrmz, item.Fknr);
                    newsb.AppendFormat(fkformat, sTime, item.Fkrmz, item.Fknr);
                }
                AllFanKui = newsb.ToString();
                PDAFanKuiText = pdasb.ToString();
            }
            else
                sb.AppendLine("\t无");
            InfoText = sb.ToString() ;

        }
    }
}
