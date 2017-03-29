using Pvirtech.Model;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Pvirtech.Framework.Domain
{
    /// <summary>
    /// 图层xml配置文件
    /// </summary>
    public class MapGroups : List<MapGroup>
    { 

    }
    public class MapGroup
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        public List<MapLayerData> MapLayers { get; set; }
    }
}
