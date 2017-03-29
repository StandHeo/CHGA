using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pvirtech.Model
{
    [XmlRoot("ZhiYin")]
    public class ZhiYinXMLModel
    {
        [XmlArrayAttribute("ZhiYinItems")]
        public List<AlarmLevel> AlarmLevels { get; set; }

        [XmlArrayAttribute("JQZYItems")]
        public List<JQZYItem> ZYItems { get; set; }
    }

    public class AlarmLevel
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlArrayAttribute("items")]
        public List<LocaleResource> LocaleResources { get; set; }
    }
    public class LocaleResource
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElementAttribute("Value")]
        public string Value { get; set; }
    }

    public class JQZYItem
    {
        [XmlAttribute("Category")]
        public string Category { get; set; }

        [XmlAttribute("AlarmType")]
        public string AlarmType { get; set; }

        [XmlAttribute("AlarmTypeDetail")]
        public string AlarmTypeDetail { get; set; }

        [XmlAttribute("Keywords")]
        public string Keywords { get; set; }

        [XmlArray("Infos")]
        public List<Info> Infos { get; set; }
    }

    public class Info
    {
        [XmlElement("Short")]
        public string Short { get; set; }

        [XmlElement("Detail")]
        public string Detail { get; set; }

    }
}
