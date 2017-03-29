using Pvirtech.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    /// <summary>
    /// 警情相关服务接口
    /// </summary>
    public interface IPoliceCaseRepository
    {
        /// <summary>
        /// 获取未完成警情
        /// </summary>
        /// <returns></returns>
        Task<Result<List<AlarmBase>>> GetAlarmInfoData(string departmentNo="");
                /// <summary>
        /// 催促警员
        /// </summary>
        /// <returns></returns>
        Task<Result<bool>> UrgePolice(Dictionary<string, object> param = null);

        /// <summary>
        /// 处警
        /// </summary>
        /// <param name="polId">警情流水号</param>
        /// <param name="UserNo">处警员编号</param>
        /// <param name="disText">处警措施</param>
        /// <param name="disName">出警单位名称</param>
        /// <param name="idsNumber">出警单位编号</param>
        /// <param name="IsSettle">是否允许结案</param>
        /// <returns></returns>
        Task<Result<bool>> PoliceDispose(Dictionary<string, object> param = null);
        /// <summary>
        /// 获取出警单位
        /// </summary>
        /// <param name="polId">警情流水编号</param>
        /// <returns></returns>
        //Task<Result<List<DeparUnit>>> GetDepartUnitById(Dictionary<string, object> param = null);

        /// <summary>
        /// 2:接受/3出发/4到达/5结案
        /// </summary>
        /// <param name="polNumber">警情流水号</param>
        /// <param name="disposeId">处境员编号</param>
        /// <param name="disposeName">处境员姓名</param>
        /// <param name="typeId">处警状态（变更后）</param>
        /// <param name="typeId">状态{ 4:到达；5：结案 }</param>
        /// <param name="typeId">出警员ID{出警员ID可以为空}</param>
        /// <param name="describeString">结案描述(非结案不用传入)</param>
        /// <param name="GroupNo">出警单位编号</param>
        /// <param name="jjdbh">接警单编号(结案时必须传入)</param>
        /// <param name="replaytype">大联动警情结案使用</param>
        /// <param name="delayworkday">大联动警情结案使用</param>
        /// <param name="jqly">警情来源</param>
        /// <returns></returns>
        Task<Result<bool>> PoliceAction(Dictionary<string, object> param = null);

        /// <summary>
        /// 添加反馈
        /// </summary>
        /// <param name="TicklingId">填写单位编号</param>
        /// <param name="fkrmz">反馈人姓名</param>
        /// <param name="saje">涉案金额</param>
        /// <param name="cjdwbh">作案性质</param>
        /// <param name="cjdwmc">警情类型</param>
        /// <param name="model">作案手法</param>
        /// <param name="model">出警单位编号</param>
        /// <param name="model">出警单位名称</param>
        /// <param name="model">警情对象</param>
        /// <param name="isMain">是否为主反馈内容</param>
        /// <returns></returns>
        Task<Result<bool>> TicklingInfo(Dictionary<string, object> param, string isMain = "0");

        /// <summary>
        /// 获取警情的处置操作记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<Result<List<AlarmOperation>>> GetAlarmOperation(Dictionary<string, object> param);
        
        /// <summary>
        /// 获取警情的处境记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<Result<List<AlarmOperationRecord>>> GetAlarmOperRcd(Dictionary<string, object> param);

        /// <summary>
        /// 巡组作废
        /// </summary>
        /// <param name="jqlsh">警情流水号</param>
        /// <param name="groupId">巡组编号</param>
        /// <param name="groupName">巡组名称</param>
        /// <param name="zflx">作废类型</param>
        /// <param name="zfnr">作废内容</param>
        /// <returns></returns>
        Task<Result<bool>> AlarmCancleUnit(Dictionary<string, object> param = null);

        /// <summary>
        /// 获取警情反馈内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<Result<List<AlarmFeedBack>>> GetAlarmFeedBack(Dictionary<string, object> param = null);

        /// <summary>
        /// 获取已完成警情
        /// </summary>
        /// <param name="param">查询条件</param>
        /// <returns></returns>
        Task<Result<PageResult<AlarmBase>>> GetFinshAlarm(Dictionary<string, object> param = null,string departmentNo="");
                /// <summary>
        /// 移交大联动
        /// </summary>
        /// <param name="jqlsh">警情流水号</param>
        /// <param name="cjyxm">处境员姓名</param>
        /// <returns></returns>
        Task<Result<bool>> PoliceYiJiaoDLD(Dictionary<string, object> param);

        /// <summary>
        /// 地图派警
        /// </summary>
        /// <param name="jqlsh"></param>
        /// <param name="userNo"></param>
        /// <param name="userName"></param>
        /// <param name="cjcs"></param>
        /// <param name="cjdwmz"></param>
        /// <param name="cjdwbh"></param>
        /// <param name="allowEnd"></param>
        /// <param name="is2PDA"></param>
        /// <returns></returns>
        Task<Result<bool>> MapHandleAlarm(string jqlsh, string userNo, string userName, string cjcs, string cjdwmz, string cjdwbh, int allowEnd, int is2PDA);

        /// <summary>
        /// 上传GPS地图坐标点
        /// </summary>
        /// <returns></returns>
        Task<bool> UploadMapPoint(string jqlsh, string xpoint, string ypoint);
        //Task<Dictionary<string,IList<CarTrackModel>>> GetCarTrack(DateTime? beginTime, DateTime? endTime, string carPlate, string trackType);
        Task<string> ReceiveAlarmDld(Dictionary<string, object> param = null);
        Task<bool>  SendSMS(string url,string content,string telphones);
    }
}
