using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pvirtech.Model
{
    [XmlRoot("HttpCommand")]
    public class HttpCommand
    {
        [XmlAttribute("Name")]
        public string Key;

        [XmlElement("Host")]
        public string Host;
        /// <summary>
        /// 项目名
        /// </summary>
        [XmlElement("ProjectName")]
        public string ProjectName;
        /// <summary>
        /// 模块名
        /// </summary>
        [XmlElement("ModuleName")]
        public string ModuleName;
        /// <summary>
        /// 方法名
        /// </summary>
        [XmlElement("MethodName")]
        public string MethodName;

        /// <summary>
        /// JSON 数据模板
        /// </summary>
        [XmlElement("JsonData")]
        public string JsonDataTemplete;

    }
}
