using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    public class PoliceGroupRepository:IPoliceGroupRepository
    {
        /// <summary>
        /// 加载所有警力
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<Patrol>>> GetPatorl(string departmentNo) 
        {
            Result<List<Patrol>> result = new Result<List<Patrol>>();
            List<Patrol> collection = new List<Patrol>();
            try
            {
                string jsonResult =await CommandCenter.Excute("queryReportInfo");
                if (string.IsNullOrEmpty(jsonResult))
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "返回值为空";
                    return result;
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"] != null && obj["result"].HasValues)
                {
                    collection = new List<Patrol>(JsonConvert.DeserializeObject<List<Patrol>>(obj["result"].ToString()));
                    result.Code = ResultCode.SUCCESS;
                    result.Message = "操作成功！";
                    result.Model = collection;
                }
                else if (obj["error"] != null)
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = obj["error"]["message"].ToString();
                }
                else
                {
                    result.Code = ResultCode.OTHER;
                    result.Message = "未知错误！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "GetPatorl");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            if (!string.IsNullOrEmpty(departmentNo))
            {
                if (result.Model!=null)
                {
                    result.Model = result.Model.Where(o => o.GroupNo.Contains(departmentNo)).ToList();
                }
              
            }
            return result;
        }

        /// <summary>
        /// 统计当前数量
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<TodayCaseCount>>> GetTodayCaseInfo(string beginTime,string endTime)
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(beginTime))
                para.Add("beginTime", beginTime);
            else
                para.Add("beginTime", "");
            if (!string.IsNullOrEmpty(endTime))
                para.Add("endTime", endTime);
            else
                para.Add("endTime", "");
            Result<List<TodayCaseCount>> result = new Result<List<TodayCaseCount>>();
            List<TodayCaseCount> collection = new List<TodayCaseCount>();
            string jsonResult = await CommandCenter.Excute("searchAlarmTypeCount", para);
            try
            {
                if (string.IsNullOrEmpty(jsonResult))
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "返回值为空";
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"] != null && obj["result"].HasValues)
                {
                    collection = new List<TodayCaseCount>(JsonConvert.DeserializeObject<List<TodayCaseCount>>(obj["result"].ToString()));
                    result.Code = ResultCode.SUCCESS;
                    result.Message = "操作成功！";
                    result.Model = collection;
                }
                else if (obj["error"] != null)
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = obj["error"]["message"].ToString();
                }
                else
                {
                    result.Code = ResultCode.OTHER;
                    result.Message = "未知错误！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "queryReportInfo");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据指定的日期统计警情数量
        /// </summary>
        /// <param name="jqjb"></param>
        /// <param name="type"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<Result<List<TodayCaseCount>>> GetAlarmAmount(string type, string Code, string beginTime ="", string endTime="")
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("type", type);
            para.Add("typeCode", Code);
            if (!string.IsNullOrEmpty(beginTime))
                para.Add("beginTime", beginTime);
            else
                para.Add("beginTime", "");
            if (!string.IsNullOrEmpty(endTime))
                para.Add("endTime", endTime);
            else
                para.Add("endTime", "");
            Result<List<TodayCaseCount>> result = new Result<List<TodayCaseCount>>();
            List<TodayCaseCount> collection = new List<TodayCaseCount>();
            string jsonResult = await CommandCenter.Excute("searchAlarmsNumByType", para);
            try
            {
                if (string.IsNullOrEmpty(jsonResult))
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "返回值为空";
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"] != null && obj["result"].HasValues)
                {
                    collection = new List<TodayCaseCount>(JsonConvert.DeserializeObject<List<TodayCaseCount>>(obj["result"].ToString()));
                    result.Code = ResultCode.SUCCESS;
                    result.Message = "操作成功！";
                    result.Model = collection;
                }
                else if (obj["error"] != null)
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = obj["error"]["message"].ToString();
                }
                else
                {
                    result.Code = ResultCode.OTHER;
                    result.Message = "未知错误！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "queryReportInfo");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
