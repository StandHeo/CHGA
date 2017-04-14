using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    public class CommonRepository : ICommonRepository
    {
        //public List<string> GetAlarmModule()
        //{
        //    List<string> list = new List<string>();
        //    list.Add("处理警情");
        //    list.Add("结案警情");
        //    list.Add("查询统计");
        //    return list;
        //}
        /// <summary>
        /// 获取字段信息
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public async Task<Result<List<DictItem>>> GetDictItem(string kind)
        {
            Result<List<DictItem>> result = new Result<List<DictItem>>();
            List<DictItem> collection = new List<DictItem>();
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("kind", kind);
                string jsonResult = await CommandCenter.Excute("dictionaryQuery", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "返回值为空";
                    return result;
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"] != null && obj["result"].HasValues)
                {
                    collection = new List<DictItem>(JsonConvert.DeserializeObject<List<DictItem>>(obj["result"].ToString()));
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
                LogHelper.ErrorLog(ex, "GetAlarmInfoData");
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }

        public virtual List<AppModuleInfo> GetAlarmModule()
        {
            List<AppModuleInfo> list = new List<AppModuleInfo>();
            list.Add(new AppModuleInfo() { Header = "处理警情" });
            list.Add(new AppModuleInfo() { Header = "结案警情" });
            list.Add(new AppModuleInfo() { Header = "查询统计" });
            return list;
        }

        public async Task<bool> SendQRCodeTimeOut(string jqlsh)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("jqlsh", jqlsh);
                string jsonResult = await CommandCenter.Excute("sendQRCodeTimeOut", param);
                if (string.IsNullOrEmpty(jsonResult))
                {

                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
            }
            return true;
        }

        public async Task<string> SendQRCodeToDld(string jqlsh, string opUserNo, string opUserName)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("jqlsh", jqlsh);
                param.Add("opUserNo", opUserNo);
                param.Add("opUserName", opUserName);
                string jsonResult = await CommandCenter.Excute("sendQRCodeToDld", param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    JObject obj = JObject.Parse(jsonResult);
                    if (obj["code"].ToString() == "000000")
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return obj["info"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
            }

            return string.Empty;
        }

        public async Task<bool> GenerateQRCode(string jqlsh)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("jqlsh", jqlsh);
                string jsonResult = await CommandCenter.Excute("generateQRCode", param);
                if (string.IsNullOrEmpty(jsonResult))
                {

                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
            }
            return true;
        }
    }
}
