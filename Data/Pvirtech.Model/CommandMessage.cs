using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public class CommandMessage
    {
        public CommandMessage()
        {
            Paras = new Dictionary<string, object>();
        }

        public string Group
        { get; set; }
        public string Key
        { get; set; }

        public Dictionary<string, object> Paras
        { get; set; }

        public string SenderName
        { get; set; }

        public string ObjectName
        { get; set; }

        public override string ToString()
        {
            if (Paras.Count == 0)
                return "";

            return Group + "." + Key;// + ":AID=" + Paras["AID"].ToString() +",PID="+ Paras["PID"].ToString();
        }

    }
}
