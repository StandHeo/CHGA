using ESRI.ArcGIS.Client.Geometry;
using System.Collections.Generic;
namespace Pvirtech.Model
{
    /// <summary>
    /// 警情信息对象
    /// </summary>
    public class AlarmBase
    {
        public AlarmBase()
        {
        }
        public long Id { get; set; }

        public string Jqlsh { get; set; }
        /// <summary>
        ///  0 市局 1 大联动 2 自接警情 3重大警情
        /// </summary>
        public int Jqly { get; set; }

        public string Jjdbh { get; set; }

        public string Jjdwbh { get; set; }

        public string Jjybh { get; set; }

        public string Jjyxm { get; set; }

        public string Jjsj { get; set; }

        public int Bjfsbh { get; set; }

        public string Bjdh { get; set; }

        public string Bjdhyhxm { get; set; }

        public string Bjdhyhdz { get; set; }

        public string Bjrxm { get; set; }

        public int Bjrxb { get; set; }

        public string Lxdh { get; set; }

        public string Zagj { get; set; }

        public string Gxdwbh { get; set; }

        public double Zddwxzb { get; set; }

        public double Zddwyzb { get; set; }

        public string Bjlb { get; set; }

        public string Bjlx { get; set; }

        public string Bjxl { get; set; }

        public string Rcsjc { get; set; }

        public int Isdelete { get; set; }

        public string Bjnr { get; set; }

        public string Sfdz { get; set; }

        public int Ajzt { get; set; }

        public int Ywbkry { get; set; }

        public int Ywwxwz { get; set; }

        public int Ywbzw { get; set; }

        public double Sddwyzb { get; set; }

        public double Sddwxzb { get; set; }

        public string Zhgxrxm { get; set; }

        public string Zhgxrbh { get; set; }

        public int Zhgxqd { get; set; }

        public string Zhgxsj { get; set; }

        public string Jams { get; set; }

        public string PDAJanr { get; set; }

        public string Jarxm { get; set; }

        public string Jarbh { get; set; }

        public string Jasj { get; set; }
        /**流转大联动状态 -1：流转超时 0：流转失败 1:流转中，2：流转成功 3：结案*/

        public int DldStatus { get; set; }
        /**流转大联动成功失败消息*/

        public string DldCloseInfo { get; set; }

        public string DldInfo { get; set; }
        /**流转大联动操作人姓名*/
        public string DldOpUserName { get; set; }
        /**流转大联动时间*/
        public string DldTime { get; set; }
        /**是否流转大联动  1：流转*/
        public int IsDld { get; set; }

        public int CaseLevel { get; set; }

        public int AllowEnd { get; set; }
        //public MapPoint MapPoint { get; set; }
        /// <summary>
        /// 共享单位名称
        /// </summary>
        public string GxdwName { get; set; }
        /// <summary>
        /// 是否初始化警情操作记录
        /// </summary>
        public bool IsInitRcd { get; set; }
        /// <summary>
        /// 是否初始化处警记录
        /// </summary>
        public bool IsInitExu { get; set; }
        /// <summary>
        /// 是否初始化反馈记录
        /// </summary>
        public bool IsInitFdb { get; set; }
        public MapPoint MapPoint { get; set; }

        public override string ToString()
        {
            return Jqlsh;
        }
       

    }
    public enum AlarmStatus
    {
        /// <summary>
        /// 新警情
        /// </summary>
        NEWALARM = 1,
        /// <summary>
        /// 已处警
        /// </summary>
        EXCUTE = 3,

        /// <summary>
        /// 设备接收
        /// </summary>
        RECIVE = 5,

        /// <summary>
        /// 警力接收
        /// </summary>
        OTHER = 6,
        /// <summary>
        /// 出发
        /// </summary>
        START = 7,
        /// <summary>
        /// 到达
        /// </summary>
        ARRIVE = 9,
        /// <summary>
        /// 反馈
        /// </summary>
        FEEDBACK = 11,

        /// <summary>
        /// 巡组处理完成
        /// </summary>
        PDAFULFILL = 12,
        /// <summary>
        /// 结案
        /// </summary>
        END = 13
    }
}
