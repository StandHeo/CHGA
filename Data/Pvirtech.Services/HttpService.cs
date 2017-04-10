using Newtonsoft.Json;
using Pvirtech.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    public class HttpService : IHttpService
    {
        public async Task<string> Post(string url, object param)
        {
            try
            {
                var postStr = JsonConvert.SerializeObject(param);
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpContent content = new StringContent(postStr, Encoding.UTF8);
                    content.Headers.ContentType= new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    UtilsHelper.LogTexts("发送数据", postStr);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();

                    var jsonStr = await response.Content.ReadAsStringAsync();
                    UtilsHelper.LogTexts("返回数据", jsonStr);

                    return jsonStr;
                }

            }
            catch (Exception)
            {

            }

            return string.Empty;
        }
    }
}
