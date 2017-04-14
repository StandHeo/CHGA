using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS.ActiveMQ;
using Apache.NMS;
using Apache.NMS.Util;
using System.Threading;
using Pvirtech.Framework.Message;

namespace Pvirtech.Framework
{
    public class MQChannel
    {
        private IMessageConsumer Consumer = null;
        private IMessageProducer Producer = null;


        public List<MQClientAdapter> ClientAdapters
        {
            get;
            set;
        }

        public string Key
        { get; set; }
        public string ChannelName
        { get; set; }

        public MQChannelType ChannelType
        {
            get;
            set;
        }


        public MQChannel(string key,string channelName, MQChannelType type)
        {
            Key = key;
            ChannelName = channelName;
            ChannelType = type;
            ClientAdapters = new List<MQClientAdapter>();
        }
        public void Send(string msg)
        { 
            if(Producer!=null)
            {
                ITextMessage message = Producer.CreateTextMessage();
                message.Text = msg;
                Producer.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
            }
        }

        public void Create(ISession session)
        {

            if (ChannelType == MQChannelType.Consumer || ChannelType == MQChannelType.ProducerAndConsumer)
            {
                Consumer = session.CreateConsumer(SessionUtil.GetDestination(session, ChannelName));
                Consumer.Listener += new MessageListener(WhenMessageReceive);
            }
            if (ChannelType == MQChannelType.Producer || ChannelType == MQChannelType.ProducerAndConsumer)
            {
                Producer=session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(ChannelName));
            }
        }


        private void WhenMessageReceive(IMessage message)
        {
            try
            {
                if (message == null)
                {
                    return;
                }
                ITextMessage msg = (ITextMessage)message;
                string filter = msg.Properties.GetString("filter");
                string msgType = msg.Properties.GetString("msgtype");

                foreach (MQClientAdapter one in ClientAdapters)
                {
                    if (filter == "ALL" || string.IsNullOrEmpty(one.Filter) || one.Filter == filter)
                    {
                        if ((string.IsNullOrEmpty(one.MessageType) || one.MessageType == msgType) && one.WhenMessageReceive != null)
                        {
                            byte[] bpath = Convert.FromBase64String(msg.Text);
                            string msgStr = Encoding.Default.GetString(bpath);
                            one.WhenMessageReceive(one,filter, msgType, msgStr);
                        }
                    }
                }
            }
            //补充写日志，或发特殊消息
            catch
            { 
                
            }
 
        }


    }

    public enum MQChannelType
    {
        Producer=1,
        Consumer=2,
        ProducerAndConsumer=3
    }
}
