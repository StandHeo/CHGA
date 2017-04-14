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
        public string BaseUrl { get; set; } = "http://ip:port/CHGAPlatform/alarm/";
        public async Task<string> AlarmDetail()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    jqlsh = ""
                }
            };

            var url = BaseUrl + "alarmDetail";

            return await Post(url, param);
        }

        public async Task<string> ArrangePolice()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    jqlsh = "",
                    zhgxrbh = "",
                    zhgxrxm = "",
                    rwnr = "",
                    cjdwbh = "",
                    cjdwmc = "",
                    yqsx = "",
                    rwdd = ""
                }
            };

            var url = BaseUrl + "arrangePolice";

            return await Post(url, param);
        }

        public async Task<string> EndCase()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    jams = "",
                    jqlsh = "",
                    cjyj = "",
                    fkqk = ""
                }
            };

            var url = BaseUrl + "endCase";

            return await Post(url, param);
        }

        public async Task<string> GetCopHandleRecord()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    jqlsh = ""
                }
            };

            var url = BaseUrl + "endCase";

            return await Post(url, param);
        }

        public async Task<string> GetPoliceUnit()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                   
                }
            };

            var url = BaseUrl + "listFinishedAlarms";

            return await Post(url, param);
        }

        public async Task<string> ListFinishedAlarms()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    gxdwbh = "",
                    bjnr = "",
                    startTime = "",
                    endTime = ""
                }
            };

            var url = BaseUrl + "listFinishedAlarms";

            return await Post(url, param);
        }

        public async Task<string> ListUnFinishedAlarms()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    gxdwbh = "",
                    bjnr = ""
                }
            };

            var url = BaseUrl + "listUnFinishedAlarms";

            return await Post(url, param);
        }

        public async Task<string> Login()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    loginname = "",
                    password = ""
                }
            };

            var url = BaseUrl + "login";

            return await Post(url, param);
        }

        public async Task<string> policeNum()
        {
            throw new NotImplementedException();
        }



        public async Task<string> UpdateAddress()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    jqlsh = "",
                    hshdwxzb = "",
                    hshdwyzb="",
                    hshdwdz=""
                }
            };

            var url = BaseUrl + "updateAddress";

            return await Post(url, param);
        }

        public async Task<string> Urge()
        {
            var param = new
            {
                id = 1,
                version = 1,
                @params = new
                {
                    pjdid = "",
                    ccnr = ""
                }
            };

            var url = BaseUrl + "urge";

            return await Post(url, param);
        }


        public async Task<string> Post(string url, object param)
        {
            try
            {
                var postStr = JsonConvert.SerializeObject(param);
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpContent content = new StringContent(postStr, Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
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
