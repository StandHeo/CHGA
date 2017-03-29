using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public class AlarmOperation
    {
        /** 系统id */
        public long Id{get;set;}

        /** 警情流水号 */
        public string Jqlsh{get;set;}

        /** 处警员编号 */
        public string Cjybh{get;set;}

        /** 处警员名字 */
        public string Cjymz{get;set;}

        /** 系统出警时间  */
        public string Xtcjsj{get;set;}

        /** 处警员处警时间（处警员点击确认按钮时间） */
        public string Cjycjsj{get;set;}

        /** 处警措施  */
        public string Cjcs{get;set;}

        /** 出警单位编号 */
        public string Cjdwbh{get;set;}

        /** 出警单位名 */
        public string Cjdwmz{get;set;}

        /** 派单到达时间（警情到达单兵系统时间） */
        public string Pdddsj{get;set;}

        /** 接受确认时间（单兵点击接受按钮时间） */
        public string Jsqrsj{get;set;}

        /** 出发时间（出警单位出发时间） */
        public string Cfsj{get;set;}

        /** 到达时间（出警单位到达案发现场时间） */
        public string Ddsj{get;set;}

        /** 是否作废    0:作废     1：未作废 */
        public int IsCancle{get;set;}

        /** 作废依据 */
        public string Zfyj{get;set;}

        /** 出警状态  -1：发送失败  0:待发送  1： 接收确认（设备自动回复） 2： 接受警情        3：  出发      4：到达现场   5 ： 处理完成    */
        public int Cjzt{get;set;}

        /** 是否删除   0：删除 1： 未删除*/
        public int IsDelete{get;set;}

        public string Ext1{get;set;}//扩展字段

        public string Ext2{get;set;}//扩展字段

        public string Ext3{get;set;}//扩展字段

        public string Ext4{get;set;}//扩展字段

        public string Ext5{get;set;}//扩展字段
    }
}
