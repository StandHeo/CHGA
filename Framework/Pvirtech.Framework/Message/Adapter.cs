using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Threads;
using Pvirtech.Framework.Common;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Pvirtech.Framework.Message
{
    public class MQAdapter:ServerAdapter
    {

        private IConnectionFactory factory = null;
        private IConnection connection = null;
        private ISession session = null;

        public MQAdapter()
        {
            Channels = new Dictionary<string, MQChannel>();
        }

        public string ServerUrl
        {
            get {
                return string.Format("activemq:failover:(tcp://{0}:{1})", ServerName, Port);
            }
        }

        public string ClientID
        {
            get
            {
                return "CLIENT_"+WinHelper.GetHostIP();
            }
        }

        public Dictionary<string,MQChannel> Channels
        { get; set; }

        public override void Start()
        {
            try
            {
                factory = new ConnectionFactory(ServerUrl); 
                TaskRunnerFactory task = new TaskRunnerFactory();
                
                connection = factory.CreateConnection();
                connection.RequestTimeout = TimeSpan.FromSeconds(30);
                connection.ClientId = ClientID; 
                connection.ConnectionInterruptedListener += Connection_ConnectionInterruptedListener;
                connection.ExceptionListener += Connection_ExceptionListener;
                connection.ConnectionResumedListener += Connection_ConnectionResumedListener;
                session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                foreach (MQChannel one in Channels.Values)
                {
                    one.Create(session);
                }
                connection.Start();
            }
            catch
            {
                MessageBox.Show("MQ服务连接失败!");
            }
           
            
        }

        private void Connection_ConnectionResumedListener()
        {
            LogHelper.WriteLog("ResumedListener:" + DateTime.Now.ToString());
        }

        private void Connection_ExceptionListener(Exception exception)
        {
            LogHelper.ErrorLog(exception);
        }

        private void Connection_ConnectionInterruptedListener()
        {
            LogHelper.WriteLog("InterruptedListener:" + DateTime.Now.ToString());
        }

        public override void Stop()
        {
            connection.Stop();
            
        }

        public bool IsStarted
        {
            get
            {
                return connection.IsStarted;
            }
        }

    }

    public class MQClientAdapter
    {
        public string Source
        { get; set; }

        public string MessageType
        { get; set; }

        public string Filter
        { get; set; }

        public delegate void WhenMessageReceiveHandle(object sender,string filter, string msgType, string msgBody);

        public WhenMessageReceiveHandle WhenMessageReceive;

    }


}
