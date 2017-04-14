using Prism.Events;
using Pvirtech.Framework;
using Pvirtech.Framework.Domain;
using Pvirtech.Framework.Message;
using System;
using System.Windows.Threading;

namespace Pvirtech.Services
{

    //[Export(typeof(IMessageAgent))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class MessageAgent:IMessageAgent
    {
        private IEventAggregator _eventAggregator;
        //private IPvirtechClient _ipvirtechClient;
        private  MQAdapter MQServer = null; 
        public  MQChannel MQSender = null;
        public  MQChannel MQReceiver = null;
        private DispatcherTimer dispatcherTimer;
        public IEventAggregator EventAggregator
        {
            get
            {
              return  this._eventAggregator;
            }
        }
 
        //[ImportingConstructor]
        public MessageAgent(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public  void Start(string serverIP, string serverPort)
        {
            MQServer = new MQAdapter();
            MQServer.Init(serverIP, serverPort);
            MQSender = new MQChannel("Sender", "recvQueue", MQChannelType.Producer);
            MQServer.Channels.Add("Sender", MQSender);

            MQReceiver = new MQChannel("Receiver", "topic://testTopic", MQChannelType.Consumer);
            MQServer.Channels.Add("Receiver", MQReceiver);

            MQServer.Start();
        }

        public void StartDispatcherTimer()
        {
            //启动计时器
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            this._eventAggregator.GetEvent<TimeSpanEventArgs>().Publish(1);           
        }

        public  void CreateAdapter(string source, string filter, string type, MQClientAdapter.WhenMessageReceiveHandle handle)
        {
            MQClientAdapter MQCa = new MQClientAdapter();
            MQCa.WhenMessageReceive = handle;
            MQCa.MessageType = type;
            MQCa.Source = source;
            MQCa.Filter = filter;
            MQReceiver.ClientAdapters.Add(MQCa);
        }

        public  void SendMessage(string msg)
        {
            MQSender.Send(msg);
        }
        public void Stop()
        {
            MQServer.Stop();
        }
    }
}
