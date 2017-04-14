using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    public interface IHttpService
    {
        //Task<string> Post(string url,object param);
        /// <summary>
        /// 未结案警情列表
        /// </summary>
        /// <returns></returns>
        Task<string> ListUnFinishedAlarms();
        /// <summary>
        /// 结案警情列表
        /// </summary>
        /// <returns></returns>
        Task<string> ListFinishedAlarms();
        /// <summary>
        /// 获取警情详细信息
        /// </summary>
        /// <returns></returns>
        Task<string> AlarmDetail();
        /// <summary>
        /// 派警接口
        /// </summary>
        /// <returns></returns>
        Task<string> ArrangePolice();
        /// <summary>
        /// 获取警组
        /// </summary>
        /// <returns></returns>
        Task<string> GetPoliceUnit();
        /// <summary>
        /// 更新位置
        /// </summary>
        /// <returns></returns>
        Task<string> UpdateAddress();
        /// <summary>
        /// 催促接口
        /// </summary>
        /// <returns></returns>
        Task<string> Urge();
        /// <summary>
        /// 结案
        /// </summary>
        /// <returns></returns>
        Task<string> EndCase();
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        Task<string> Login();
        /// <summary>
        /// 巡警处警（移交）要记表接口
        /// </summary>
        /// <returns></returns>
        Task<string> GetCopHandleRecord();
        /// <summary>
        /// 警力查询
        /// </summary>
        /// <returns></returns>
        Task<string> policeNum();
    }
}
