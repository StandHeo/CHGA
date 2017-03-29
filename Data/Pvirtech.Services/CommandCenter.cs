using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pvirtech.Services
{
    public class CommandCenter
    {
        /// <summary>
        /// 初始化命令中心
        /// </summary>
        public static void Start()
        {
            InitHttpCmd();
        }

        public static void Start(string bc1,string bc2)
        {
            InitHttpCmd();
            foreach (HttpCommand one in HttpCollection.Values)
            {
                one.Host = one.Host.Replace("@BC1", bc1);
                one.Host = one.Host.Replace("@BC2", bc2);
            }
        }

        #region Http Cmd
        /// <summary>
        /// Http请求集合
        /// 系统与服务端可用的Http接口命令
        /// </summary>
        private static Dictionary<string, HttpCommand> HttpCollection;
        private static void InitHttpCmd()
        {
            string xmlPath = string.Format("Pvirtech.Framework.Resources.ServerInterface.{0}", "ServerInterface.xml");
            Assembly myAssembly = Assembly.Load("Pvirtech.Framework.Resources");
            Stream stream = myAssembly.GetManifestResourceStream(xmlPath);
            XmlSerializer serializer =XmlHelper.CreateDefaultXmlSerializer(typeof(List<HttpCommand>));
            List<HttpCommand> list = serializer.Deserialize(stream) as List<HttpCommand>;
            if (HttpCollection == null)
                HttpCollection = new Dictionary<string, HttpCommand>();
            foreach (HttpCommand cmd in list)
            {
                HttpCollection.Add(cmd.Key, cmd);
            }
            stream.Close();        
        }
        public static async Task<string> Excute(string cmd, Dictionary<string, object> param=null)
        {
            return await HttpCollection[cmd].Excute(param);
        }

        public static async Task<string> Excute(string cmd, string value)
        {
            return await HttpCollection[cmd].Excute(value);
        }

        /// <summary>
        /// 执行命令 返回处理结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<Result<T>> ExcuteObject<T>(string cmd, Dictionary<string, object> param = null)
        {
            Result<T> result = new Result<T>();
            try
            {
                string jsonResult = await HttpCollection[cmd].Excute(param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "返回值为空";
                    return result;
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"] != null)
                {
                    result.Code = ResultCode.SUCCESS;
                    result.Message = "操作成功！";
                    if (obj["result"].Type == JTokenType.Boolean)
                        result.Model = JsonConvert.DeserializeObject<T>(obj["result"].ToString().Replace("False","false").Replace("True","true"));
                    else
                        result.Model = JsonConvert.DeserializeObject<T>(obj["result"].ToString());
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
                LogHelper.ErrorLog(ex, cmd);
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 执行命令 返回处理结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Result<T> ExcuteObjectSync<T>(string cmd, Dictionary<string, object> param = null)
        {
            Result<T> result = new Result<T>();
            try
            {
                string jsonResult = HttpCollection[cmd].ExcuteSync(param);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    result.Code = ResultCode.FAILE;
                    result.Message = "返回值为空";
                    return result;
                }
                JObject obj = JObject.Parse(jsonResult);
                if (obj["result"] != null)
                {
                    result.Code = ResultCode.SUCCESS;
                    result.Message = "操作成功！";
                    if (obj["result"].Type == JTokenType.Boolean)
                        result.Model = JsonConvert.DeserializeObject<T>(obj["result"].ToString().Replace("False", "false").Replace("True", "true"));
                    else
                        result.Model = JsonConvert.DeserializeObject<T>(obj["result"].ToString());
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
                LogHelper.ErrorLog(ex, cmd);
                result.Code = ResultCode.EXCEPTION;
                result.Message = ex.Message;
            }
            return result;
        }

        #endregion


    }
}


