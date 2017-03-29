using Prism.Mvvm;
using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    public class AlarmStatusViewModel : BindableBase
    {
        public Dictionary<int,AlarmStatusItem> RunStatus
        { get; set; }

        private void InitDefault()
        { 
            RunStatus=new Dictionary<int,AlarmStatusItem>();
            string[] itemName=new string[4] {"接警","派警","到达","结案"};
            
            for(int i=1;i<=4;i++)
            {
                AlarmStatusItem item=new AlarmStatusItem();
                item.StatusName=itemName[i-1];
                item.StatusStartDesc="00:00:00";
                item.IntervalDesc="";
                item.OrderNo=i;
                RunStatus.Add(i,item);
            }

        
        }
        public void Init(MAlarmInfo info)
        {
            InitDefault();

            //计算警情状态

            DateTime? First=null;
            DateTime? Last=null;

            AlarmStatusItem one = RunStatus[1];
            //接警
            if (!string.IsNullOrEmpty(info.BjsjDesc))
            {
                First=DateTime.Parse(info.BjsjDesc);

                one.StatusStartDesc = First.Value.ToString("HH:mm:ss");
                one.AlarmLevel = info.CaseLevel;
            }
            else
                return;


            //派警
            one = RunStatus[2];
            //未作废，有派警时间的第一个作为警情的派警时间
            MAlarmPatrol p = info.UsePatrols.Patrols.FirstOrDefault(aa => aa.Oper.IsCancle == 1 && 
                !string.IsNullOrEmpty(aa.Oper.Cjycjsj)
                );
            if(p!=null)
            {
                Last=DateTime.ParseExact(p.Oper.Cjycjsj, "yyyyMMddHHmmss", null);
                one.AlarmLevel = info.CaseLevel;
                one.StatusStartDesc =Last.Value.ToString("HH:mm:ss");
                one.IntervalDesc = GetTimeSpan(First, Last);
                First = Last;
               
            }

            //到达
            one = RunStatus[3];
            //找未废掉的巡组
            p = info.UsePatrols.Patrols.FirstOrDefault(aa => aa.Oper.IsCancle == 1 && !string.IsNullOrEmpty(aa.Oper.Ddsj));
            if(p!=null)
            {
                Last=DateTime.ParseExact(p.Oper.Ddsj, "yyyyMMddHHmmss", null);
                one.AlarmLevel = info.CaseLevel;
                one.StatusStartDesc =Last.Value.ToString("HH:mm:ss");
                one.IntervalDesc = GetTimeSpan(First, Last);
                First = Last;
            }


  /**动作类型   1：接收确认（设备自动回复）2： 接受警情   3： 出发   4：到达现场   
     * 5 ： 处理完成  6:催促  7:处警 8:违规操作（30秒未处警，5分钟未催促）
     * 9：修改单兵能否结案  10:作废巡组   11：反馈 */

            //巡组处理完成
            one = RunStatus[4];
            //找结束的日志状态
            List<AlarmOperationRecord> LogList=LocalDataCenter.GetAlarmOperRcd(info.Jqlsh);
            AlarmOperationRecord log = LogList.FirstOrDefault(aa => aa.IsDelete == 1 && (aa.Dzlx == 5 || aa.Dzlx == 11));
            if(log!=null)
            {
                Last=DateTime.ParseExact(log.StartTime,"yyyyMMddHHmmss", null);
                one.AlarmLevel = info.CaseLevel;
                one.StatusStartDesc =Last.Value.ToString("HH:mm:ss");
                one.IntervalDesc = GetTimeSpan(First, Last);
            }

        }
        private string GetTimeSpan(DateTime? one, DateTime? two)
        {
            if (one == null || two == null || two<one)
                return "";

            TimeSpan span = two.Value - one.Value;
            string ret = "";
            if (span.Days > 0)
                ret += span.Days.ToString() + "天";

            if (span.Hours > 0)
                ret += span.Hours.ToString() + "小时";

            if (span.Minutes > 0)
                ret += span.Minutes.ToString() + "分";

            if (span.Seconds > 0)
                ret += span.Seconds.ToString() + "秒";

            return ret;
        }

    }

    public class AlarmStatusItem
    {
        public int OrderNo
        {get;set;}
        //状态名称
        
        
        public string StatusName
        { get; set; }

        //开始时间描述
        public string StatusStartDesc
        {
            get;set;
        }

        //间隔时间
        public string IntervalDesc
        {
            get;set;
        }
        public int AlarmLevel { get; set; }

    }
}
