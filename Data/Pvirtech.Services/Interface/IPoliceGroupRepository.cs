using Pvirtech.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    /// <summary>
    /// 警力查询接口
    /// </summary>
    public interface IPoliceGroupRepository
    {
        /// <summary>
        /// 加载所有警力
        /// </summary>
        /// <returns></returns>
        Task<Result<List<Patrol>>> GetPatorl(string departmentNo="");

        /// <summary>
        /// 统计当日警情数量
        /// </summary>
        /// <param name="beginTime">统计开始时间</param>
        /// <param name="endTime">统计结束时间</param>
        /// <returns></returns>
        Task<Result<List<TodayCaseCount>>> GetTodayCaseInfo(string beginTime="", string endTime="");

        /// <summary>
        /// 获取指定日期的警情数量
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="jqjb"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Result<List<TodayCaseCount>>> GetAlarmAmount(string type, string Code, string beginTime ="", string endTime="");
    }
}
