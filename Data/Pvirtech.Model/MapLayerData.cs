using System.Xml.Serialization;

namespace Pvirtech.Model
{
    public class MapLayerData
    {
        [XmlAttribute("Title")]
        public string Title { get; set; }

        public double Radius { get; set; }

        [XmlAttribute("ClustererImage")]
        public string ClustererImage { get; set; }
        
        [XmlAttribute("NormalStyle")]
        public string NormalStyle { get; set; }
    
        [XmlAttribute("PressedStyle")]
        public string PressedStyle { get; set; }
       
        [XmlAttribute("IsChecked")]
        public bool IsChecked { get; set; }
      
        [XmlAttribute("IsEnabled")]
        public bool IsEnabled { get; set; }
      
        [XmlAttribute("LayerType")]
        public int LayerType { get; set; }
      
        [XmlAttribute("LayerId")]
        public string LayerId { get; set; }

        [XmlAttribute("LayerUrl")]
        public string LayerUrl { get; set; }

        [XmlAttribute("FeatureName")]
        public string FeatureName { get; set; }

        [XmlAttribute("Where")]
        public string Where { get; set; }

        [XmlAttribute("TipTemplate")]
        public string TipTemplate { get; set; }

        [XmlAttribute("IsShow")]
        public bool IsShow { get; set; }
    }
}
