using Prism.Commands;
using Prism.Mvvm;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using Pvirtech.Modules.NormalAlarm.ViewModels;
using Pvirtech.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    public class AlarmPatrolList: BindableBase
    {
        public void Init(MAlarmInfo model)
        {
            List<AlarmOperation> twos = LocalDataCenter.GetAlarmOper(model.Jqlsh);
            _Patrols = new List<MAlarmPatrol>();
            for (int i = 0; i < twos.Count; i++)
            {
                //如果有编号无单位，则不初始化
                Patrol patrol = LocalDataCenter.GetPatrol(twos[i].Cjdwbh);
                if (patrol == null)
                    LogHelper.WriteLog("未找到处警单位：" + twos[i].Cjdwbh);
                else
                {
                    MAlarmPatrol two = new MAlarmPatrol();
                    two.AlarmID = model.Jqlsh;
                    two.Oper = twos[i];
                    two.patrol = patrol;
                    //two.CurOper = AlarmRunCmd.OperItems.Items.FirstOrDefault(aa => aa.Status == twos[i].Cjzt);
                    two.Init();
                    _Patrols.Add(two);
                }
            }
            OnPropertyChanged(() => Patrols);
        }

        private List<MAlarmPatrol> _Patrols;

        public List<MAlarmPatrol> Patrols
        {
            get
            {
                return _Patrols;
            }
            set
            {
                if (_Patrols != value)
                    _Patrols = value;

                OnPropertyChanged("Patrols");
            }
        }

    }

    public class MAlarmPatrol : BindableBase
    {

        public MAlarmPatrol()
        {         
        }
        
        public string AlarmID
        { get; set; }
        public Patrol patrol
        {
            get;
            set;
        }

        public AlarmOperation Oper
        { get; set; }

        public AlarmPatrolOperItem CurOper
        { get; set; }

        public List<AlarmPatrolCmdItem> Cmds
        { get; set; }

        public void Init()
        {
            Cmds = new List<AlarmPatrolCmdItem>();
            ICommand icmd=new DelegateCommand<string>(RunCommand); 
            foreach (AlarmPatrolCmdItem one in CurOper.Cmds)
            {
                AlarmPatrolCmdItem item = new AlarmPatrolCmdItem();
                item.Key = one.Key;
                item.Name = one.Name;
                item.BindCommand = icmd;
                Cmds.Add(item);
            } 
            OnPropertyChanged("Cmds");
        }
        private void RunCommand(string cmdName)
        {
            if (patrol != null){
                LogHelper.WriteLog(cmdName + ":" + AlarmID);
                EventCenter.PublishAlarm(cmdName, AlarmID, patrol.GroupNo);
            }
            else {
                EventCenter.PublishAlarm(cmdName, AlarmID, "");
            }
        }
    }

    #region Base

    [XmlRoot("Root")] 
    public class BaseOperItems
    {
        [XmlArray("OperItems")]
        [XmlArrayItem("OperItem")]
        public List<AlarmPatrolOperItem> Items
        { get; set; }

        [XmlArray("MenuItems")]
        [XmlArrayItem("CmdItem")]
        public List<AlarmPatrolCmdItem> CmdItems
        { get; set; }

        [XmlArray("RunItems")]
        [XmlArrayItem("CmdItem")]
        public List<AlarmPatrolCmdItem> RunItems
        { get; set; }
    }

    public class AlarmPatrolOperItem
    {
        [XmlAttribute("Status")] 
        public int Status
        {get;set;}

        [XmlAttribute("Key")] 
        public string Key
        {get;set;}

        [XmlAttribute("Name")] 
        public string Name
        {get;set;}


        [XmlAttribute("Color")] 
        public string Color
        {get;set;}

        [XmlArray("CmdItems")]
        [XmlArrayItem("CmdItem")]
        public List<AlarmPatrolCmdItem> Cmds
        {
            get;
            set;
        }
    }

    public class AlarmPatrolCmdItem
    {
        [XmlAttribute("Key")]
        public string Key
        { get; set; }

        [XmlAttribute("Name")]
        public string Name
        { get; set; }

        [XmlIgnore]
        public ICommand BindCommand
        { get; set; }
    }

    #endregion

}
