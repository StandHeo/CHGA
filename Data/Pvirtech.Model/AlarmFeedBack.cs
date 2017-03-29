using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public class AlarmFeedBack
    {
        /** 系统id */
        public long Id{get;set;}

        /** 警情流水号 */
        public string Jqlsh{get;set;}

        /** 反馈人名 */
        public string Fkrmz{get;set;}

        /** 反馈人编号 */
        public string Fkrbh{get;set;}

        /** 出警单位名字*/
        public string Cjdwmz{get;set;}

        /** 出警单位编号 */
        public string Cjdwbh{get;set;}

        /** 作案性质 */
        public string Zaxz{get;set;}

        /** 报警类别 */
        public string Bjlb{get;set;}

        /** 报警类型 */
        public string Bjlx{get;set;}

        /** 报警细类（作案手法 ） */
        public string Bjxl{get;set;}

        /** 反馈内容 */
        public string Fknr{get;set;}

        /** 反馈时间 */
        public string Fksj{get;set;}

        /** 作案手法 */
        public string Zasf{get;set;}

        /// <summary>
        /// 反馈来源
        /// /**执勤组 ？PDA 反馈    0：pc 反馈   1： PDA反馈 */
        /// </summary>
        public int PcOrPDA { get; set; } 

        /**
         * 是否为回执市局的反馈内容（主反馈内容）
         * 0：否  1：是
         */
        public int IsMain{get;set;}

        /** 是否删除  0：删除  1：未删除 */
        public int IsDelete{get;set;}

        /** 涉案金额  */
        public string Saje{get;set;}

        /** 反馈编号 */
        public string FeedBackNo{get;set;}

        /** 更新反馈人姓名 */
        public string Gxrxm{get;set;}

        /** 更新反馈人编号  */
        public string Gxrbh{get;set;}

        /** 更新反馈时间  */
        public string Gxsj{get;set;}
        /**
         * 附件编号json数组：
         */
        public string Fjbh{get;set;}

        public string Ext1{get;set;}

        public string Ext2{get;set;}//扩展字段

        public string Ext3{get;set;}//扩展字段

        public string Ext4{get;set;}//扩展字段

        public string Ext5{get;set;}//扩展字段
    }
}
