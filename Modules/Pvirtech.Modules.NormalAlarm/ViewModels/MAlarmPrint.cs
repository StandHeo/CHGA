using Pvirtech.Framework.Common;
using Pvirtech.Services;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{

    #region Base

    [XmlRoot("Root")]
    public class AlarmPrintXML
    {
        [XmlArray("AlarmClose")]
        [XmlArrayItem("AlarmInfo")]
        public List<AlarmPrintItem> Items { get; set; }
    }

    public class AlarmPrintItem
    {
        [XmlAttribute("PrintKey")]
        public string Key { get; set; }


        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Url")]
        public string Url { get; set; }
    }

    #endregion

    /// <summary>
    /// 打印类库
    /// </summary>
    public static class MAlarmPrint
    {
        private static AlarmPrintXML alarmXml;
        private delegate void DoPrintMethod(PrintDialog pdlg, DocumentPaginator paginator);
        private delegate void EnableButtonMethod();

        /// <summary>
        /// 模板对象
        /// </summary>
        public static AlarmPrintXML AlarmXml
        {
            get
            {
                return alarmXml;
            }

            set
            {
                alarmXml = value;
            }
        }

        #region 方法

        static MAlarmPrint()
        {
            if (AlarmXml == null)
            {
                alarmXml = XmlHelper.GetXmlEntities<AlarmPrintXML>("Alarm.PrintPageUrl.xml");
            }
        }
            
        /// <summary>
        /// 打印方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="flowDocument"></param>
        public static void PrintPageClick(object model,FlowDocument flowDocument)
        {
            if (model == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }
            if (flowDocument == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            PrintDialog pdlg = new PrintDialog();
            pdlg.PrintDocument(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, "接处警记录单打印");
        }

        #endregion

    }
}
