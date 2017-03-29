using Prism.Events;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Pvirtech.Framework.Domain
{
    public class CommonEventArgs : PubSubEvent<MessageModel>
    {

    }
    public class CustomMapEventArgs : PubSubEvent<object>
    {

    }
    public class TimeSpanEventArgs : PubSubEvent<long>
    {

    }

    public class MapTipEventArgs : PubSubEvent<Dictionary<string, object>>
    {

    }


    /// <summary>
    /// 达联动警情数量
    /// </summary>
    public class DLDAlarmCountEventArgs : PubSubEvent<object>
    {

    }
    /// <summary>
    /// 信息流转中心警情数量
    /// </summary>
    public class XXLZAlarmCountEventArgs : PubSubEvent<object>
    {

    }
    public class CGCJAlarmCountEvent : PubSubEvent<object>
    {

    }
    public class YJCLAlarmCountEvent : PubSubEvent<object>
    {
    }
    public class MapMoveEventArgs : PubSubEvent<string>
    {

    }


    public class UserStageChangedArgs : PubSubEvent<object>
    {

    }

    public class EventMsgSignFlagChangedArgs : PubSubEvent<KeyValuePair<string,int>>
    {

    }

    public class ZDJQSendAlarmEvent : PubSubEvent<Dictionary<string,string>>
    {

    }
    public class BigLinkageStatusChangedArgs : PubSubEvent<KeyValuePair<string,int>>
    {

    }

    public class BigLinkageProgressBarVisibilityArgs : PubSubEvent<Tuple<string, Visibility>>
    {

    }

    public class BigLinkageTimeOutArgs : PubSubEvent<string>
    {

    }

    public class SendToBigLinkageArgs : PubSubEvent<string>
    {

    }


}
