using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Pvirtech.Framework.Common;
using Pvirtech.Framework.Domain;
using Pvirtech.Model;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    public class SelectPatrolViewModel:BindableBase
    {
        public List<SelectPatrolItem> AllPatrol
        { get; set; }

        #region Command
        private ICommand _SendCommand;
        public ICommand SendCommand
        {
            get
            {
                return _SendCommand;
            }
        }

        private void RunSendCommand(string nouse)
        {
            if (string.IsNullOrEmpty(SelectPatrolText))
            {              
                return;
            }
            var split = new char[] { ',', ' ', '+', '-', '=' };
            var items = SelectPatrolText.Split(split);
            List<SelectPatrolItem> lstKeys = new List<SelectPatrolItem>();
            foreach (string id in items)
            {
                if (string.IsNullOrEmpty(id)) continue;                 
                var item = AllPatrol.FirstOrDefault(aa => aa.ShortID == id);
                if (item != null)
                {
                    lstKeys.Add(item);
                }
            }
            var keys = string.Join(",", lstKeys.Select(o => o.UnitID));
            if (!string.IsNullOrEmpty(keys))
            {
                EventCenter.PublishAlarm("SendAlarm", jqlsh, keys);

                //todo：重大警情 调用信息流转中心发送消息的方法 派警后上报市局
                if (jqly == 3)
                { 
                    StringBuilder info = new StringBuilder();
                    info.Append("成华指挥室已派警给：");
                    foreach (var item in lstKeys)
                    {
                        info.Append(item.DisplayName).Append(",");
                    }
                    info.Remove(info.Length - 1, 1);
                    info.Append(DateTime.Now.ToString("【时间：yyyy-MM-dd HH:mm:ss】"));
                    OnSendMsgCmd(jjdbh, info.ToString());
                }
            }
            else
            {
                MessageBox.Show("未找到当前报备警力!");
            }
        }


        //两个参数 事件编号  信息内容
        private void OnSendMsgCmd(string bh, string info)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("jjdbh", bh);
            dic.Add("info", info);
            var eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.
                GetInstance<IEventAggregator>();
            if (eventAggregator !=null)
                eventAggregator.GetEvent<ZDJQSendAlarmEvent>().Publish(dic);
        }

        #endregion

        private string _SelectPatrolText;
        public string SelectPatrolText
        {
            get
            { return _SelectPatrolText; }
            set
            {
                if (_SelectPatrolText != value)
                {
                    _SelectPatrolText = value;
                    SetChecks(_SelectPatrolText);
                    OnPropertyChanged("SelectPatrolText");
                }
            }
        }

        public string SelectPatrolKeys
        { get; set; }

        private bool IsChecking;
        public void SetChecks(string selText)
        { 
            if(IsChecking)
                return ;
            if(string.IsNullOrEmpty(selText))
                return ;

            IsChecking=true;

            var Items = AllPatrol.Where(aa => aa.IsChecked && aa.IsEnabled);
            foreach (var item in Items)
            {
                item.IsChecked = false;
            }

            char[] split = new char[] { ',', ' ', '+', '-', '=' };
            string[] ss = selText.Split(split);

            foreach (string s in ss)
            {

                var item = AllPatrol.FirstOrDefault(aa => aa.ShortID == s);
                if (item != null && item.IsEnabled)
                {
                    item.IsChecked = true;
                }
            }
     
            SelectPatrolKeys = string.Join(",", GetKeys());
            IsChecking = false;
            
        }

        private string[] GetKeys()
        {
            return AllPatrol.Where(aa => aa.IsChecked && aa.IsEnabled).ToList().Select(aa => aa.UnitID).ToArray();
        }
        public void SetValues()
        {
            if (IsChecking)
                return;
            IsChecking = true;

            List<SelectPatrolItem> Items = AllPatrol.Where(aa => aa.IsChecked && aa.IsEnabled).ToList();
            string[] text=Items.Select(aa=>aa.ShortID).ToArray();
            string[] keys = Items.Select(aa => aa.UnitID).ToArray();
            _SelectPatrolText=string.Join(",",text);
            SelectPatrolKeys = string.Join(",", GetKeys());
            OnPropertyChanged("SelectPatrolText");
            IsChecking = false;

        }

        private string jqlsh
        { get; set; }

        private int jqly
        { get; set; }

        private string jjdbh
        { get; set; }

        public List<SelectPatrolGroup> Groups
        {
            get;
            set;
        }


        public List<SelectPatrolGroup> UseGroups
        {
            get
            {
                return Groups.Where(aa=>aa.Patrols.Count >0).ToList();
            }
        }


        public void Refresh()
        {
            OnPropertyChanged("UseGroups");
        }


        public void Init(MAlarmInfo alarm)
        {
            IsChecking = true;

            _SendCommand = new DelegateCommand<string>(RunSendCommand);
            jqlsh = alarm.Jqlsh;
            jqly = alarm.Jqly;
            jjdbh = alarm.Jjdbh;
            
            AllPatrol = new List<SelectPatrolItem>();
            List<Patrol> list= LocalDataCenter.GetPatrols();
            for (int i = 0; i < list.Count; i++)
            {
                SelectPatrolItem one = new SelectPatrolItem();
                one.GroupKey = (list[i].GroupType * 10 + list[i].PoliceGroupType).ToString();
                one.UnitID = list[i].GroupNo;
                one.ShortID=list[i].CallNo;
                one.IsEnabled=true;
                one.IsChecked=false;
                one.SetStatus(list[i].AppStatus, list[i].Status);
                one.model = this;
                one.DisplayName=list[i].GroupName;
                if (one.GroupKey == "10040")
                    one.DisplayName = one.DisplayName.Substring(0, 2);

                if(!string.IsNullOrEmpty(one.ShortID) && one.DisplayName!=one.ShortID)
                    one.DisplayName = one.DisplayName + "(" + one.ShortID + ")";
                
                
                string pNo = list[i].ParentNo;
                if (pNo == alarm.Gxdwbh)
                    one.GroupKey = "111";

                AllPatrol.Add(one);
            }
            CreatePoliceItems();

            Groups = new List<SelectPatrolGroup>();


            //确定已派
            foreach(MAlarmPatrol one in alarm.UsePatrols.Patrols)
            {
                if (one.patrol!=null)
                {
                    var item = AllPatrol.FirstOrDefault(aa => aa.UnitID == one.patrol.GroupNo);
                    if (item != null)
                    {

                        item.IsChecked = true;
                        item.GroupKey = "000";
                        item.IsEnabled = false;

                    }
                }
             
            }

            var items=AllPatrol.Where(aa=>aa.GroupKey=="000").ToList();
            Groups.Add(Create("已派", items));


            items = AllPatrol.Where(aa => aa.GroupKey=="111" ).ToList();
            Groups.Add(Create("归属", items));

            //items = AllPatrol.Where(aa => aa.ParentNo == alarm.Gxdwbh).ToList();
            //Groups.Add(createRandom("附近", 4, false));
            items = AllPatrol.Where(aa => aa.GroupKey == "20011").ToList();
            Groups.Add(Create("巡组", items));

            items = AllPatrol.Where(aa => aa.GroupKey =="10040").ToList();
            Groups.Add(Create("派出所", items));

            items = AllPatrol.Where(aa => aa.GroupKey == "999").ToList();
            Groups.Add(Create("科室队", items));

            items = AllPatrol.Where(aa => aa.GroupKey =="20012").ToList();
            Groups.Add(Create("快反", items));
           
            items = AllPatrol.Where(aa => aa.GroupKey =="20013").ToList();
            Groups.Add(Create("机动", items));

            IsChecking = false;
        }

        public void CreatePoliceItems()
        {
            XmlDocument xml = XmlHelper.GetXmlDoc("Alarm.SendAlarm.xml");
            XmlNode root = xml.SelectSingleNode("/Group");
            string gName = root.Attributes["Name"].InnerText;
            foreach (XmlNode one in root.ChildNodes)
            {
                SelectPatrolItem item = new SelectPatrolItem();
                item.DisplayName = one.Attributes["Name"].InnerText;
                item.UnitID = one.Attributes["UnitID"].InnerText;
                item.ShortID = one.Attributes["ShortID"].InnerText;
                item.IsChecked = false;
                item.IsEnabled = true;
                item.SetStatus(0,1);
                item.GroupKey = "999";
                item.model = this;
                AllPatrol.Add(item);
            }

        }

        public SelectPatrolGroup Create(string header, List<SelectPatrolItem> filterItems)
        {
            SelectPatrolGroup one = new SelectPatrolGroup();
            one.Name = header;
            one.Patrols = filterItems;
            one.IsChecked = true;

            return one;
        }
    }


    public class SelectPatrolGroup :BindableBase
    {
        public string Name
        { get; set; }

        public List<SelectPatrolItem> Patrols
        { get; set; }


        private bool _isChecked;
        public bool IsChecked
        {
            get
            { return _isChecked; }
            set
            {
                if (Patrols.Count == 0)
                    value = false;
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        public int OnlineCount
        {
            get {
                return Patrols.Count(aa => aa.Fore==Brushes.Green);
            }

        }
    }

    public class SelectPatrolItem:BindableBase
    {
        public SelectPatrolViewModel model;
        public string ShortID
        {
            get;set;
        }

        public string UnitID
        { get; set; }

        public string GroupKey
        { get; set; }

        public string DisplayName
        {
            get;
            set;
        }


        private bool _IsChecked;
        public bool IsChecked
        {
            get
            { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                    model.SetValues();
                }
            }
        }

        public bool IsEnabled
        {get;set;}

        public Brush Fore
        {
            get;
            set;
        }

        public void SetStatus(int appstatus,int groupstatus)
        { 
            Brush one = Brushes.Gray;
            if(appstatus==1)
                    one = Brushes.Green;
            else
                if(groupstatus!=0)
                    one = Brushes.Red;

            Fore=one;
            OnPropertyChanged(() => Fore);
        }

    }
}
