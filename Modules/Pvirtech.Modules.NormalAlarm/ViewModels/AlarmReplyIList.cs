using Pvirtech.Framework.Common;
using Pvirtech.Services;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    #region Base

    [XmlRoot("Root")] 
    public class AlarmReplyIList
    {
        [XmlArray("AlarmType")]
        [XmlArrayItem("AlarmItem")]
        public List<AlarmReplyItems> Items { get; set; }

        [XmlArray("AlarmWord")]
        [XmlArrayItem("ItemWord")]
        public List<AllItemWord> AllWords { get; set; }
    }

    public class AlarmReplyItems
    {
        [XmlAttribute("Number")]
        public string Number { get; set; }

        [XmlAttribute("WordKey")]
        public string WordKey { get; set; }

    }
    public class AllItemWord
    {
        [XmlAttribute("key")]
        public string WordNo { get; set; }

        [XmlElement("Word")]
        public List<string> Words { get; set; }
    }

    #endregion

    public static class GetAlarmReply
    {
        public static AlarmReplyIList AlarmReplys { get; set; }

        /// <summary>
        /// 根据选择的编号匹配回复语言
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static List<string> GetWordByNo(string no)
        {
            #region 参数验证

            List<string> values = new List<string>();
            if (string.IsNullOrEmpty(no))
            {
                EventCenter.PublishError("参数不能为空");
            }
            if (AlarmReplys == null)
            {
                EventCenter.PublishError("未读取到资源数据");
            }

            #endregion

            #region 执行操作

            AlarmReplyItems vs = AlarmReplys.Items.FirstOrDefault(A => A.Number == no);
            if (vs == null)
            {
                AllItemWord model = AlarmReplys.AllWords.FirstOrDefault(W => W.WordNo == "通用");
                values.AddRange(model.Words);
                return values;
            }
            string[] s = UtilsHelper.ByStringSplit(vs.WordKey);
            foreach (var item in s)
            {
                AllItemWord wm = AlarmReplys.AllWords.FirstOrDefault(W => W.WordNo == item);
                if (wm != null)
                    values.AddRange(wm.Words);
            }

            return values;

            #endregion
        }

        /// <summary>
        /// 根据选择的编号匹配回复语言
        /// </summary>
        /// <param name="bjlb">报警类别</param>
        /// <param name="bjlx">报警类型</param>
        /// <param name="bjxl">报警细类</param>
        /// <returns></returns>
        public static List<string> GetWordByNo(string bjlb, string bjlx, string bjxl)
        {
            List<string> values = new List<string>();

            //判断报警类别
            if (string.IsNullOrEmpty(bjlb))
            {
                AllItemWord model = AlarmReplys.AllWords.FirstOrDefault(W => W.WordNo == "通用");
                values.AddRange(model.Words);
            }
            else
               values.AddRange(GetWordByNo(bjlb));

            //判断报警类型
            if (string.IsNullOrEmpty(bjlx))
                return values;
            else
                values.AddRange(GetWordByNo(bjlx));
            
            //判断报警细类
            if (string.IsNullOrEmpty(bjxl))
                return values;
            else
                values.AddRange(GetWordByNo(bjlx));
            return values;
        }
    }
}
