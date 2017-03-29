using Prism.Events;
using Pvirtech.Model;
using System;

namespace Pvirtech.Services
{
    public class EventCenter
    {
        private static EventAggregator Agg; 

        public static void Publish(CommandMessage one)
        {
            if (Agg == null)
                Agg = new EventAggregator();

            //Agg.GetEvent<RunCommandArgs>().Publish(one);
        }

        public static void Subscribe(Action<CommandMessage> action)
        {
            if (Agg == null)
                Agg = new EventAggregator();

            //Agg.GetEvent<RunCommandArgs>().Subscribe(action, ThreadOption.UIThread);
        }

        public static void Unsubscribe(Action<CommandMessage> action)
        {
            if (Agg == null)
                Agg = new EventAggregator();

            //Agg.GetEvent<RunCommandArgs>().Unsubscribe(action);
        }

        public static void PublishPatrolTip(string cmdName,string No)
        {
            CommandMessage one = new CommandMessage();
            one.Group = "PatrolCommand";
            one.Key = cmdName;
            one.Paras.Add("PID", No);
            Publish(one);
        }

        public static void PublishPrompt(string msg)
        {
            CommandMessage one = new CommandMessage();
            one.Group = "SystemMessage";
            one.Key = "Prompt";
            one.Paras.Add("Source", msg);
            Publish(one);
        }
        public static void PublishError(string msg)
        {
            CommandMessage one = new CommandMessage();
            one.Group = "SystemMessage";
            one.Key = "Error";
            one.Paras.Add("Source", msg);
            Publish(one);
        }
        public static void PublishAlarm(string cmdName,string alarmID,string patrolIDs)
        {
            CommandMessage one = new CommandMessage();
            one.Group = "Alarm";
            one.Key = cmdName;
            one.Paras.Add("AID", alarmID);
            if(patrolIDs!="NO")
                one.Paras.Add("PID", patrolIDs);
            Publish(one);
        }

        public static void PublishAlarmPrompt(string cmdName,string alarmID,string patrolIDs)
        {
            CommandMessage one = new CommandMessage();
            one.Group = "AlarmPrompt";
            one.Key = cmdName;
            one.Paras.Add("AID", alarmID);
            if (patrolIDs != "NO")
                one.Paras.Add("PID", patrolIDs);
            Publish(one);
        } 
        
    }
}
