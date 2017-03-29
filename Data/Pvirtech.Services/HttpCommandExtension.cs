using Newtonsoft.Json;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    public static class HttpCommandExtension
    {
        public static async Task<string> Excute(this HttpCommand command,Dictionary<string, object> param = null)
        {
            string jsonParam = JsonConvert.SerializeObject(param);
            string postData;
            if (string.IsNullOrEmpty(command.JsonDataTemplete))
                postData = "data={\"jsonrpc\":\"2.0\",\"method\":\"$methodName\",\"params\" :$params,\"id\":\"$id\"}";
            else
                postData = command.JsonDataTemplete;
            postData = postData.Replace("$params", jsonParam);
            postData = postData.Replace("$methodName", command.MethodName);
            postData = postData.Replace("$id", UtilsHelper.GenerateRandomNumber(4));
            return await HttpPost("http://" + command.Host + "/" + command.ProjectName + "/" + command.ModuleName + "?method=" + command.MethodName, postData);
        }

        /// <summary>
        /// 执行接口访问方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<string> Excute(this HttpCommand command,string param)
        {
            string postData;
            if (string.IsNullOrEmpty(command.JsonDataTemplete))
                postData = "data={\"jsonrpc\":\"2.0\",\"method\":\"$methodName\",\"params\" :$params,\"id\":\"$id\"}";
            else
                postData = command.JsonDataTemplete;
            postData = postData.Replace("$params", param);
            postData = postData.Replace("$methodName", command.MethodName);
            postData = postData.Replace("$id", UtilsHelper.GenerateRandomNumber(4));
            return await HttpPost("http://" + command.Host + "/" + command.ProjectName + "/" + command.ModuleName + "?method=" + command.MethodName, postData);
        }

        /// <summary>
        /// 执行接口访问方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ExcuteSync(this HttpCommand command, Dictionary<string, object> param = null)
        {
            string jsonParam = JsonConvert.SerializeObject(param);
            string postData;
            if (string.IsNullOrEmpty(command.JsonDataTemplete))
                postData = "data={\"jsonrpc\":\"2.0\",\"method\":\"$methodName\",\"params\" :$params,\"id\":\"$id\"}";
            else
                postData = command.JsonDataTemplete;
            postData = postData.Replace("$params", jsonParam);
            postData = postData.Replace("$methodName", command.MethodName);
            postData = postData.Replace("$id", UtilsHelper.GenerateRandomNumber(4));
            return HttpPostSync("http://" + command.Host + "/" + command.ProjectName + "/" + command.ModuleName + "?method=" + command.MethodName, postData);
        }

        /// <summary>
        /// HTTP Post 异步获取
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public async static Task<string> HttpPost(string Url, string postData)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpContent content = new StringContent(postData, Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    UtilsHelper.LogTexts("发送数据", postData);
                    //LogHelper.WriteLog(postData);
                    //UtilsHelper.LogTexts()
                    HttpResponseMessage response = await client.PostAsync(Url, content);
                    response.EnsureSuccessStatusCode();
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    UtilsHelper.LogTexts("返回数据", jsonStr);
                    //LogHelper.WriteLog(jsonStr);
                    return jsonStr;
                }
                catch (Exception ex)
                {
                    LogHelper.ErrorLog(ex);
                    return ex.ToString();
                }
            }
        }

        /// <summary>
        /// HTTP Post 同步获取
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string HttpPostSync(string Url, string postData)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    Uri uri = new Uri(Url);
                    Encoding utf = Encoding.GetEncoding("utf-8");
                    client.Encoding = utf;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                    byte[] byteData = null;
                    if (string.IsNullOrWhiteSpace(postData))
                    {
                        byteData = new byte[0];
                    }
                    else
                    {
                        byteData = utf.GetBytes(postData);
                    }
                    byte[] result = client.UploadData(uri, "POST", byteData);
                    string xml = utf.GetString(result);
                    return xml;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return string.Empty;
            }
        }
    }
}
