using Prism.Events;
using Pvirtech.Framework.Message;

namespace Pvirtech.Services
{
    public interface IMessageAgent
    {
        void Start(string serverIP, string serverPort);
        void StartDispatcherTimer();
         void CreateAdapter(string source, string filter, string type, MQClientAdapter.WhenMessageReceiveHandle handle);


         void SendMessage(string msg);

        void Stop();
        IEventAggregator EventAggregator { get; }
        


    }
}
