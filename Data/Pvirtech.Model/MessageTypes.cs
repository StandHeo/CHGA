namespace Pvirtech.Model
{
    public enum MessageTypes
    {

        /// <summary>
        /// 警力接受
        /// </summary>
        OTHER,

        /// <summary>
        ///  结案
        /// </summary>
        ENDALARM,

        /// <summary>
        ///  到达现场
        /// </summary>
        ALARMARRIVE,

        /// <summary>
        ///  出发
        /// </summary>
        ALARMSTART,

        /// <summary>
        ///  单兵接收
        /// </summary>
        ALARMRECIVE,

        /// <summary>
        ///  新警情
        /// </summary>
        NEWALARM,

        /// <summary>
        /// 警情反馈
        /// </summary>
        FEEDBACK,

        /// <summary>
        /// 警情信息变更
        /// </summary>
        ALARMCHANGE,

        /// <summary>
        /// 未知
        /// </summary>
        UNKONW,

        /// <summary>
        /// 处警
        /// </summary>
        EXCUTEALARM,

        /// <summary>
        /// 报备警力状态变更
        /// </summary>
        GROUPSTATUS,

        /// <summary>
        /// 报备警力其他信息变更
        /// </summary>

        GROUPOTHER,
        //系统一般提示

        PROMPT,
        //系统错误消息提示

        ERROR,
        //催促

        ALARMURGE,

        //催促
        ALARMHASURGE,

        /// <summary>
        /// 巡组处理完成
        /// </summary>
        PDAFULFILL,


        /// <summary>
        /// 成功流转至大联动
        /// </summary>
        SEND_TO_DLD_SUCCESS,

        /// <summary>
        /// 流转至大联动失败
        /// </summary>
        SEND_TO_DLD_FAIL,


        /// <summary>
        /// 结案成功
        /// </summary>
        CLOSE_DLD_SUCCESS,

        /// <summary>
        /// 结案失败
        /// </summary>
        CLOSE_DLD_FAIL


    }
}