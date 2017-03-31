using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pvirtech.Services
{
    /// <summary>
    /// 警情模块
    /// 功能：警情模块与服务器接口对接函数
    /// </summary>
    public class PoliceCaseRepository : IPoliceCaseRepository
    {
        /// <summary>
        /// 获取未完成警情
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<AlarmBase>>> GetAlarmInfoData(string departmentNo)
        {
            Result<List<AlarmBase>> result = new Result<List<AlarmBase>>();
            List<AlarmBase> collection = new List<AlarmBase>();
            try
            {
                Debug.WriteLine(DateTime.Now);
                string jsonResult = await CommandCenter.Excute("searchUnfshAlarmPC");
                if (string.IsNullOrEmpty(jsonResult))
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "返回值为空";
                    return result;
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"] != null && obj["result"].HasValues)
                {
                    JArray jarray = JArray.Parse(obj["result"].ToString());

                    // collection = JsonConvert.DeserializeObject<List<AlarmBase>>(obj["result"].ToString());
                    if (!string.IsNullOrEmpty(departmentNo))
                    {
                        foreach (var item in jarray)
                        {
                            var model = JsonConvert.DeserializeObject<AlarmBase>(item.ToString());
                            if (model.Gxdwbh == departmentNo)
                            {
                                collection.Add(model);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in jarray)
                        {
                            var model = JsonConvert.DeserializeObject<AlarmBase>(item.ToString());
                            collection.Add(model);
                        }

                    }
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
                Debug.WriteLine(DateTime.Now);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "GetAlarmInfoData");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 催促警员
        /// </summary>
        /// <returns></returns>
        public async Task<Result<bool>> UrgePolice(Dictionary<string, object> param = null)
        {
            Result<bool> rtnResult = new Result<bool>();
            try
            {
                //Dictionary<string, object> param = new Dictionary<string, object>();
                //param.Add("cjjl", deparId);
                //param.Add("ccnr", text);
                //param.Add("cjr",operatorUser);
                string jsonResult = await CommandCenter.Excute("UrgePolice", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    rtnResult.Code = ResultCode.FAILE;
                    rtnResult.Message = "";
                    rtnResult.Model = false;
                    return rtnResult;
                }
                JObject jo = JObject.Parse(jsonResult);
                if (jo.Count > 0)
                {
                    if (jo["result"] == null)
                    {
                        if (jo["error"] != null)
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = jo["error"]["message"].ToString();
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                        }
                    }
                    else
                    {
                        string result = jo["result"].ToString();
                        if (result == "True")
                        {
                            rtnResult.Code = ResultCode.SUCCESS;
                            rtnResult.Message = "操作成功！";
                            rtnResult.Model = true;
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                            rtnResult.Model = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "UrgePolice");
                rtnResult.Code = ResultCode.EXCEPTION;
                rtnResult.Message = ex.Message;
            }
            return rtnResult;
        }

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
        public async Task<Result<bool>> PoliceDispose(Dictionary<string, object> param = null)
        {
            Result<bool> rtnResult = new Result<bool>();
            try
            {
                string jsonResult = await CommandCenter.Excute("executeAlarmPC", param);//await GetJsonResultData(methodName, param, postUrl);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    JObject obj = JObject.Parse(jsonResult);
                    if (obj["result"] != null)
                    {
                        string result = obj["result"].ToString();
                        if (result.Contains("True"))
                        {
                            rtnResult.Code = ResultCode.SUCCESS;
                            rtnResult.Message = "操作成功！";
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                        }
                    }
                    else
                    {
                        if (obj["error"] != null)
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = obj["error"]["message"].ToString();
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "PoliceDispose");
                rtnResult.Code = ResultCode.EXCEPTION;
                rtnResult.Message = ex.Message;
            }
            return rtnResult;
        }

        /// <summary>
        /// 获取出警单位
        /// </summary>
        /// <param name="polId">警情流水编号</param>
        /// <returns></returns>
        //public async Task<Result<List<DeparUnit>>> GetDepartUnitById(Dictionary<string, object> param = null)
        //{
        //    Result<List<DeparUnit>> result = new Result<List<DeparUnit>>();
        //    try
        //    {
        //        string jsonResult = await CommandCenter.Excute("getAlarmUnitPC", param);
        //        if (!string.IsNullOrEmpty(jsonResult))
        //        {
        //            JObject obj = JObject.Parse(jsonResult);
        //            if (obj["result"] != null)
        //            {
        //                result.Code = ResultCode.SUCCESS;
        //                result.Message = "操作成功！";
        //                result.Model =JsonConvert.DeserializeObject<List<DeparUnit>>(obj["result"].ToString());
        //            }
        //            else
        //            {
        //                if (obj["error"] != null)
        //                {
        //                    result.Code = ResultCode.FAILE;
        //                    result.Message = obj["error"]["message"].ToString();
        //                }
        //                else
        //                {
        //                    result.Code = ResultCode.FAILE;
        //                    result.Message = "操作失败！";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            result.Code = ResultCode.FAILE;
        //            result.Message = "获取数据失败！";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Code = ResultCode.EXCEPTION;
        //        result.Message = ex.Message;
        //    }
        //    return result;
        //}

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
        public async Task<Result<bool>> PoliceAction(Dictionary<string, object> param)
        {
            Result<bool> rtnResult = new Result<bool>();
            try
            {
                string jsonResult = await CommandCenter.Excute("dealCasePC", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    rtnResult.Code = ResultCode.FAILE;
                    rtnResult.Message = "";
                    rtnResult.Model = false;
                    return rtnResult;
                }
                JObject jo = JObject.Parse(jsonResult);
                if (jo.Count > 0)
                {
                    if (jo["result"] == null)
                    {
                        if (jo["error"] != null)
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = jo["error"]["message"].ToString();
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                        }
                    }
                    else
                    {
                        string result = jo["result"].ToString();
                        if (result == "True")
                        {
                            rtnResult.Code = ResultCode.SUCCESS;
                            rtnResult.Message = "操作成功！";
                            rtnResult.Model = true;
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                            rtnResult.Model = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "PoliceAction");
                rtnResult.Code = ResultCode.EXCEPTION;
                rtnResult.Message = ex.Message;
            }
            return rtnResult;
        }

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
        public async Task<Result<bool>> TicklingInfo(Dictionary<string, object> param, string isMain = "0")
        {
            Result<bool> rtnResult = new Result<bool>();
            //        string json = "{'jqlsh':'" + model.Jqlsh + "','userNo':'" + TicklingId
            //+ "','fknr':'" + model.Jzfknr + "','cjdwbh':'" + cjdwbh + "','cjdwmz':'"
            //+ cjdwmc + "'," + "'fkrmz':" + "'" + fkrmz + "'" + "," + "'saje':" + "'" + saje
            //+ "'" + "," + "'bjxl':" + "'" + zasf + "'," + "'bjlb':" + "'" + zaxz + "'"
            //+ "," + "'bjlx':" + "'" + ajlx + "','isMain':'" + isMain + "'}";

            try
            {
                param.Add("isMain", isMain);
                string jsonResult = await CommandCenter.Excute("addFedBckAlarm", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    rtnResult.Code = ResultCode.FAILE;
                    rtnResult.Message = "";
                    rtnResult.Model = false;
                    return rtnResult;
                }
                JObject jo = JObject.Parse(jsonResult);
                if (jo.Count > 0)
                {
                    if (jo["result"] == null)
                    {
                        if (jo["error"] != null)
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = jo["error"]["message"].ToString();
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                        }
                    }
                    else
                    {
                        string result = jo["result"].ToString();
                        if (result == "True")
                        {
                            rtnResult.Code = ResultCode.SUCCESS;
                            rtnResult.Message = "操作成功！";
                            rtnResult.Model = true;
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                            rtnResult.Model = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "TicklingInfo");
                rtnResult.Code = ResultCode.EXCEPTION;
                rtnResult.Message = ex.Message;
            }
            return rtnResult;
        }

        /// <summary>
        /// 获取警情的处置操作记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Result<List<AlarmOperation>>> GetAlarmOperation(Dictionary<string, object> param)
        {
            Result<List<AlarmOperation>> result = new Result<List<AlarmOperation>>();
            try
            {
                string jsonResult = await CommandCenter.Excute("getAlarmOperation", param);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    JObject obj = JObject.Parse(jsonResult);
                    if (obj["result"] != null)
                    {
                        result.Code = ResultCode.SUCCESS;
                        result.Message = "操作成功！";
                        result.Model =JsonConvert.DeserializeObject<List<AlarmOperation>>(obj["result"].ToString());
                       
                    }
                    else
                    {
                        if (obj["error"] != null)
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = obj["error"]["message"].ToString();
                        }
                        else
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = "操作失败！";
                        }
                    }
                }
                else
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "获取数据失败！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "GetAlarmOperation");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取警情的处境记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Result<List<AlarmOperationRecord>>> GetAlarmOperRcd(Dictionary<string, object> param)
        {
            Result<List<AlarmOperationRecord>> result = new Result<List<AlarmOperationRecord>>();
            try
            {
                string jsonResult = await CommandCenter.Excute("getAlarmOptRcd", param);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    JObject obj = JObject.Parse(jsonResult);
                    if (obj["result"] != null)
                    {
                        result.Code = ResultCode.SUCCESS;
                        result.Message = "操作成功！";
                        result.Model = JsonConvert.DeserializeObject<List<AlarmOperationRecord>>(obj["result"].ToString());
                    }
                    else
                    {
                        if (obj["error"] != null)
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = obj["error"]["message"].ToString();
                        }
                        else
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = "操作失败！";
                        }
                    }
                }
                else
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "获取数据失败！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "GetAlarmOperRcd");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 巡组作废
        /// </summary>
        /// <param name="jqlsh">警情流水号</param>
        /// <param name="groupId">巡组编号</param>
        /// <param name="groupName">巡组名称</param>
        /// <param name="zflx">作废类型</param>
        /// <param name="zfnr">作废内容</param>
        /// <returns></returns>
        public async Task<Result<bool>> AlarmCancleUnit(Dictionary<string, object> param = null)
        {
            Result<bool> rtnResult = new Result<bool>();
            try
            {
                string jsonResult = await CommandCenter.Excute("cancleUnit", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    rtnResult.Code = ResultCode.FAILE;
                    rtnResult.Message = "";
                    rtnResult.Model = false;
                    return rtnResult;
                }
                JObject jo = JObject.Parse(jsonResult);
                if (jo.Count > 0)
                {
                    if (jo["result"] == null)
                    {
                        if (jo["error"] != null)
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = jo["error"]["message"].ToString();
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                        }
                    }
                    else
                    {
                        string result = jo["result"].ToString();
                        if (result == "True")
                        {
                            rtnResult.Code = ResultCode.SUCCESS;
                            rtnResult.Message = "操作成功！";
                            rtnResult.Model = true;
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                            rtnResult.Model = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "AlarmCancleUnit");
                rtnResult.Code = ResultCode.EXCEPTION;
                rtnResult.Message = ex.Message;
            }
            return rtnResult;

        }

        /// <summary>
        /// 获取警情反馈内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Result<List<AlarmFeedBack>>> GetAlarmFeedBack(Dictionary<string, object> param = null)
        {
            Result<List<AlarmFeedBack>> result = new Result<List<AlarmFeedBack>>();
            List<AlarmFeedBack> collection = new List<AlarmFeedBack>();
            try
            {
                string jsonResult = await CommandCenter.Excute("searchFedBckAlarm", param);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    JObject obj = JObject.Parse(jsonResult);
                    if (obj["result"] != null)
                    {
                        result.Code = ResultCode.SUCCESS;
                        result.Message = "操作成功！";
                        JArray jarray = JArray.Parse(obj["result"].ToString());                         
                        foreach (var item in jarray)
                        {
                            var model = JsonConvert.DeserializeObject<AlarmFeedBack>(item.ToString());
                            collection.Add(model);
                        }
                        result.Model = collection;
                    }
                    else
                    {
                        if (obj["error"] != null)
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = obj["error"]["message"].ToString();
                        }
                        else
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = "操作失败！";
                        }
                    }
                }
                else
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "获取数据失败！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "GetAlarmFeedBack");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取已完成警情
        /// </summary>
        /// <param name="param">查询条件</param>
        /// <returns></returns>
        public async Task<Result<PageResult<AlarmBase>>> GetFinshAlarm(Dictionary<string, object> param = null,string departmentNo="")
        {
            Result<PageResult<AlarmBase>> result = new Result<PageResult<AlarmBase>>();
            try
            {
                string jsonResult = await CommandCenter.Excute("searchFinishAlarmPC", param);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    JObject obj = JObject.Parse(jsonResult);
                    if (obj["result"] != null)
                    {
                        result.Code = ResultCode.SUCCESS;
                        result.Message = "操作成功！";
                        result.Model = (PageResult<AlarmBase>)JsonConvert.DeserializeObject<PageResult<AlarmBase>>(obj["result"].ToString());
                        if (result.Model.bean!=null)
                        {
                           // result.Model.bean = result.Model.bean.Where(o => o.Gxdwbh == "510108510000").ToList();
                        }
                    }
                    else
                    {
                        if (obj["error"] != null)
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = obj["error"]["message"].ToString();
                        }
                        else
                        {
                            result.Code = ResultCode.FAILE;
                            result.Message = "操作失败！";
                        }
                    }
                }
                else
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "获取数据失败！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "GetAlarmFeedBack");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            
            return result;
        }

        /// <summary>
        /// 移交大联动
        /// </summary>
        /// <param name="jqlsh">警情流水号</param>
        /// <param name="cjyxm">处境员姓名</param>
        /// <returns></returns>
        public async Task<Result<bool>> PoliceYiJiaoDLD(Dictionary<string, object> param)
        {
            Result<bool> rtnResult = new Result<bool>();
            try
            {
                string jsonResult = await CommandCenter.Excute("send2Dld", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    rtnResult.Code = ResultCode.FAILE;
                    rtnResult.Message = "";
                    rtnResult.Model = false;
                    return rtnResult;
                }
                JObject jo = JObject.Parse(jsonResult);
                if (jo.Count > 0)
                {
                    if (jo["result"] == null)
                    {
                        if (jo["error"] != null)
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = jo["error"]["message"].ToString();
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                        }
                    }
                    else
                    {
                        string result = jo["result"].ToString();
                        if (result == "True")
                        {
                            rtnResult.Code = ResultCode.SUCCESS;
                            rtnResult.Message = "操作成功！";
                            rtnResult.Model = true;
                        }
                        else
                        {
                            rtnResult.Code = ResultCode.FAILE;
                            rtnResult.Message = "操作失败！";
                            rtnResult.Model = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "PoliceAction");
                rtnResult.Code = ResultCode.EXCEPTION;
                rtnResult.Message = ex.Message;
            }
            return rtnResult;
        }

        public async Task<Result<bool>> MapHandleAlarm(string jqlsh,string userNo,string userName, string cjcs, string cjdwmz, string cjdwbh, int allowEnd, int is2PDA)
        { 
            Dictionary<string, object> param = new Dictionary<string, object>(); 
            param.Add("jqlsh", jqlsh);
            param.Add("zhgxrxm", userName);
            param.Add("zhgxrbh",userNo);
            param.Add("cjybh",userNo);
            param.Add("cjcs", cjcs);
            param.Add("cjdwmz", cjdwmz);
            param.Add("cjdwbh", cjdwbh);
            param.Add("allowEnd", allowEnd);
            param.Add("is2PDA", is2PDA);
            return await CommandCenter.ExcuteObject<bool>("executeAlarmPC", param);
        }

        /// <summary>
        /// 上传GPS地图坐标点
        /// </summary>
        /// <returns></returns>
        public  async Task<bool> UploadMapPoint(string jqlsh, string xpoint, string ypoint)
        {
            bool flag = false;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("jqlsh", jqlsh);
                param.Add("sddwxzb", xpoint);
                param.Add("sddwyzb", ypoint);  
                string jsonResult = await CommandCenter.Excute("changeAlarmPC", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    return flag;
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"].ToString() == "True")
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex, "Module.cs-UploadMapPoint");
            }
            return flag;
        }


        //public async Task<Dictionary<string,IList<CarTrackModel>>> GetCarTrack(DateTime? beginTime,DateTime? endTime,string carPlate, string trackType)
        //{
        //    Dictionary<string, IList<CarTrackModel>> dic = new Dictionary<string, IList<CarTrackModel>>();
        //    try
        //    { 
        //        Dictionary<string, object> param = new Dictionary<string, object>();
        //        param.Add("begintime",beginTime!=null?beginTime.Value.ToString("yyyy-MM-dd HH:mm:ss"):string.Empty);
        //        param.Add("endtime", endTime != null ? endTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
        //        param.Add("name", carPlate);
        //        param.Add("type", trackType);
        //        string jsonResult = await CommandCenter.Excute("selGpsTrack", param);
        //        if (!string.IsNullOrEmpty(jsonResult))
        //        {
        //            JObject obj = JObject.Parse(jsonResult);
        //            var js = JArray.Parse(obj["lines"].ToString());
        //            for (int i=0;i<js.Count;i++)
        //            {
        //                var line = JsonConvert.DeserializeObject<IList<CarTrackModel>>(js[i].ToString());
        //                dic.Add(string.Format("line{0}", i), line);
        //            }
        //            var model = JsonConvert.DeserializeObject<IList<CarTrackModel>>(obj["points"].ToString());
        //            dic.Add("points",model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.ErrorLog(ex, "selGpsTrack");
        //    }
        //    return dic;
        //}

        public async Task<string> ReceiveAlarmDld(Dictionary<string, object> param = null)
        {
            try
            {
                string jsonResult = await CommandCenter.Excute("receiveAlarmDld", param);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    JObject obj = JObject.Parse(jsonResult);
                   
                    if (obj["code"].ToString() != "000000")
                    {
                        return obj["info"].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public async Task<bool> SendSMS(string url, string content, string telphones)
        { 
            bool flag = false;
            string urlStr = string.Empty;
            string postString = string.Format("password=123456&text={0}&recipient={1}&encoding=U&priority=1", content, telphones);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpContent httpcontent = new StringContent(postString);
                    httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync(url, httpcontent);
                    response.EnsureSuccessStatusCode();
                    string resultStr = await response.Content.ReadAsStringAsync();

                    XDocument doc = XDocument.Parse(resultStr);
                    var text = (from t in doc.Descendants("send")
                                select new
                                {
                                    Error = t.Element("error").Value,
                                    Status = t.Element("message_status").Value,

                                }).FirstOrDefault();
                    if (text.Error == "0")
                    {
                        switch (text.Status)
                        {
                            case "SENT":
                                //已发送
                                break;
                            case "UNSENT":
                                //未发送
                                break;
                            default:
                                break;
                        }
                        flag = true;
                    }
                    else
                    {
                        //发送失败
                    }
                    return flag;
                }
                catch (Exception ex)
                {
                    LogHelper.ErrorLog(ex);
                    return false;
                }
            }
        }
 
    }
}
