using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    /// <summary>
    /// 警情处置记录
    /// </summary>
    public class AlarmOperationRecord
    {
        /** 系统id */
         public long Id{get;set;}

        /** 警情流水号  */
         public string Jqlsh { get; set; }

        /** 动作发起对象名字  */
         public string StartName { get; set; }

        /** 动作发起对象编号 */
         public string StartNum { get; set; }

        /** 动作发起时间  */
         public string StartTime { get; set; }

        /** 目标对象名字  */
         public string TargetName { get; set; }

        /** 目标对象编号  */
         public string TargetNum { get; set; }

        /** 动作记录信息 */
         public string Info { get; set; }

        /** 备注 */
         public string Bz { get; set; }

        /** 是否删除   0： 删除   1：未删除 */
         public int IsDelete { get; set; }

        /**动作类型   1：接收确认（设备自动回复）2： 接受警情   3： 出发   4：到达现场   
         * 5 ： 处理完成  6:催促  7:处警 8:违规操作（30秒未处警，5分钟未催促）
         * 9：修改单兵能否结案  10:作废巡组  */
         public int Dzlx { get; set; }

         public string Ext1 { get; set; }//扩展字段

         public string Ext2 { get; set; }//扩展字段

         public string Ext3 { get; set; }//扩展字段

         public string Ext4 { get; set; }//扩展字段

         public string Ext5 { get; set; }//扩展字段
    }
    public enum AlarmLogStatus
    {
         设备接收=1,
         接受警情=2,
         出发=3,
         已到达现场=4,
         处理完成=5,
         催促尽快处理警情 =6,
         派警=7,
         操作违规=8,
         修改单兵能否结案 = 9,
         作废巡组 = 10
    }

}
