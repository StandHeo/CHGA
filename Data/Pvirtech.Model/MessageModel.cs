using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    /// <summary>
    /// 消息体
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageTypes MessageType { get; set; }
        public object MessageBody { get; set; }
        public string UserNo { get; set; }
    }

    
}
