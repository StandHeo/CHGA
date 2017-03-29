using Pvirtech.Framework.Common;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Pvirtech.Framework.Domain
{
    public class ProTimerWebClient : WebClient
    {

        private int timeout;
        public ProTimerWebClient(int timeOut)
        {

            this.timeout = timeOut;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = timeout;
            request.ReadWriteTimeout = timeout;
            return request;
        }
        /// <summary>
        /// post提交数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postData)
        {
            try
            {
                // postData = postData.Substring(1, postData.Length - 1);

                using (ProTimerWebClient client = new ProTimerWebClient(30000))
                {

                    Uri uri = new Uri(Url);
                    Encoding utf = Encoding.GetEncoding("utf-8");

                    client.Encoding = utf;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                    HttpWebRequest request = (HttpWebRequest)client.GetWebRequest(uri);
                    request.Method = "POST";

                    byte[] byteData = null;
                    if (string.IsNullOrWhiteSpace(postData))
                    {
                        byteData = new byte[0];
                    }
                    else
                    {
                        byteData = utf.GetBytes(postData);
                    }
                    request.ContentLength = byteData.Length;
                    request.GetRequestStream().Write(byteData, 0, byteData.Length);

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string result = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

                    return result;
                }


            }
            catch (Exception ex)
            {
                string errmsg = "ERROR:" + ex.Message;

                return errmsg;

            }


        }
    }
    public class GlobalConfig
    {


        public string WCFService { get; set; }
        public string LogInfo { get; set; }

        public string FkInfo { get; set; }
        public string QRCode { get; set; }
        public string SMSUrl { get; set; }
        public string QWBBUrl { get; set; }
        public string JQTJUrl { get; set; }
        public string YUANUrl { get; set; }
        public string BBXT { get; set; }
        public string SignalrServer { get; set; }
        public string HelperUrl { get; set; }
        public int DldTimeOut { get; set; }
        /// <summary>
        /// 业务中心地址
        /// </summary>
        public string BusinessCenter1 { get; set; }

        /// <summary>
        /// 业务中心地址备用
        /// </summary>
        public string BusinessCenter2 { get; set; }

        /// <summary>
        /// Mq服务地址
        /// </summary>
        public string MqServerIp { get; set; }
        /// <summary>
        /// Mq端口
        /// </summary>
        public string MqServerPort { get; set; }
        /// <summary>
        /// 地图地图url
        /// </summary>
        public string BaseMapUrl { get; set; }

        /// <summary>
        /// 影像地图
        /// </summary>
        public string BaseYxMapUrl { get; set; }
        /// <summary>
        /// 动态图层
        /// </summary>
        public string DynamicMapUrl { get; set; }
        /// <summary>
        /// 地图计算服务
        /// </summary>
        public string GeometryServiceUrl { get; set; }
        public string FeatureMapUrl { get; set; }
        public string MapServer { get; set; }
        /// <summary>
        /// 时钟计时
        /// </summary>
        public int TimeTick { get; set; }
        /// <summary>
        /// 语音调度信虹
        /// </summary>
        public string XHProgram { get; set; }

        /// <summary>
        /// 作废内容
        /// </summary>
        public string Fknr { get; set; }

        /// <summary>
        /// 催促时间
        /// </summary>
        public long Urge { get; set; }
        /// <summary>
        ///  视频服务
        /// </summary>
        public string VideoServer { get; set; }
        /// <summary>
        /// 视频服务端口地址
        /// </summary>
        public int VideoServerPort { get; set; }
        /// <summary>
        /// 退出确认
        /// </summary>
        public bool QuitConfirmation { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public bool IsDefaultEncode { get; set; }
        /// <summary>
        /// //播放声音
        /// </summary>
        public bool IsPlaySound { get; set; }
        /// <summary>
        /// 是否弹窗提示
        /// </summary>
        public bool IsPopupTip { get; set; }
        public bool IsPopupWindow { get; set; }
        public bool IsBusy { get; set; }

        public string SmsPhones { get; set; }
        /// <summary>
        /// 空闲安抚短信
        /// </summary>
        public string SendLeisureWord { get; set; }

        /// <summary>
        /// 查询完成的警情数据
        /// </summary>
        public int AlarmFinishNumber { get; set; }

        /// <summary>
        /// 忙碌安抚短信
        /// </summary>

        public string SendBusyWord { get; set; }

        /// <summary>
        /// 发送离线通知指令
        /// </summary>
        public string SendZLInform { get; set; }

        /// <summary>
        /// 警务报备
        /// </summary>
        public string BbWebUrl { get; set; }
        public int TrendTimeTick { get; set; }
        public MapGroups MapGroups { get; set; }
        public string SendAlarmWord { get; set; }
        public string AreaNo { get; set; }


        private static GlobalConfig globalConfig;

        public static GlobalConfig GetInstance()
        {
            return globalConfig;
        }
        public static string InitLoadConfig(string url)
        {
            string xmlfile = ProTimerWebClient.HttpPost(url, "");

            string errorMsg = string.Empty;
            if (xmlfile.Contains("ERROR"))
            {
                errorMsg = xmlfile;
                return errorMsg;
            }
            globalConfig = XmlHelper.Deserialize<GlobalConfig>(xmlfile);
            return errorMsg;
        }

        public static string GetUrl
        {
            get;
            set;
        }
        public static string PostDataFormat { get; set; }
        private static char[] constant =
        {
        '0','1','2','3','4','5','6','7','8','9'
       };
        private  static string GenerateRandomNumber(int Length)
        {
            StringBuilder newRandom = new StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
        public static string InitQureyMapConfig(string strUserNo, string modelName)
        {
            Dictionary<string, object> Dic = new Dictionary<string, object>();
            Dic.Add("userNo", strUserNo);
            Dic.Add("modelName", modelName);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(Dic);
            string PostData = string.Format(PostDataFormat, "config", data, GenerateRandomNumber(4));
            string uri = GetUrl;
            string xmlfile = ProTimerWebClient.HttpPost(uri, PostData);

            string errorMsg = string.Empty;
            if (xmlfile.Contains("ERROR"))
            {
                errorMsg = xmlfile;
                return errorMsg;
            }
            return xmlfile;
        }


        public static void SaveConfig()
        {
            //try
            //{
            //    string file = AppDomain.CurrentDomain.BaseDirectory + "Config.xml";
            //    XmlSerializer xs = new XmlSerializer(typeof(GlobalConfig));
            //    StreamWriter sw = new StreamWriter(file);
            //    xs.Serialize(sw, globalConfig);
            //    sw.Close();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.ErrorLog(ex);
            //}

        }
        public IList<MapLayerData> GetMapLayerData()
        {
            IList<MapLayerData> list = new List<MapLayerData>() 
            {
             new MapLayerData(){ Title="派出所",LayerType=0,LayerId="5",LayerUrl=UtilsHelper.GetMapFeatureUrl(MapServer, 5), NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/gajg.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/gajg_press.png",IsChecked=true,IsEnabled=true},

              new MapLayerData(){ Title="视频", LayerType=0, LayerId="3", LayerUrl=UtilsHelper.GetMapFeatureUrl(MapServer,3), NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/4gvideo.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/4gvideo_press.png",IsChecked=false,IsEnabled=true},
                new MapLayerData(){ Title="警力",LayerType=0,  LayerId="1", LayerUrl=UtilsHelper.GetMapFeatureUrl(MapServer,1), NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/zqdd.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/zqdd_press.png",IsChecked=true,IsEnabled=true},
              new MapLayerData(){ Title="单兵", LayerType=0, LayerId="2", LayerUrl=UtilsHelper.GetMapFeatureUrl(MapServer,2), NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/zqzzd.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/zqzzd_press.png",IsChecked=false,IsEnabled=true},
            
                 new MapLayerData(){ Title="天网",LayerType=0, LayerId="4",  LayerUrl=UtilsHelper.GetMapFeatureUrl(MapServer,4), NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/tianwang.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/tianwang_press.png",IsChecked=false,IsEnabled=true},
                new MapLayerData(){ Title="辖区边界",LayerType=0, LayerId="7",LayerUrl=UtilsHelper.GetMapFeatureUrl(MapServer,7), NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/hospital.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/hospital_press.png",IsChecked=true,IsEnabled=true},
           
             new MapLayerData(){ Title="今日态势",LayerType=0, LayerId="6",LayerUrl=UtilsHelper.GetMapFeatureUrl(MapServer,6), NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/lsjqts.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/lsjqts_press.png",IsChecked=true,IsEnabled=true},
              new MapLayerData(){ Title="备勤点",LayerType=0,  LayerId="14",NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/lsbqd.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/lsbqd_press.png",IsChecked=false,IsEnabled=false},
                 new MapLayerData(){ Title="路干灯",LayerType=0, LayerId="11", NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/ldg.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/ldg_press.png",IsChecked=false,IsEnabled=false},
              new MapLayerData(){ Title="联动单位",LayerType=0, LayerId="12", NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/lddw.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/lddw_press.png",IsChecked=false,IsEnabled=false},
               new MapLayerData(){ Title="卡点", LayerType=0,LayerId="13", NormalStyle="pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/bayonet.png",PressedStyle = "pack://application:,,,/Pvirtech.Framework.Resources;component/images/resourcearound/bayonet_press.png",IsChecked=false,IsEnabled=false},
            };
            return list;
        }
    }
}
