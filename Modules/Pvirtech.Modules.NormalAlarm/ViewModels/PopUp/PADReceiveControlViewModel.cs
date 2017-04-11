using Prism.Commands;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class PADReceiveControlViewModel : WorkingAlarmInfoViewModel, IConfirmation, IInteractionRequestAware
    {

        public PADReceiveControlViewModel(WorkingAlarmInfoViewModel alarm)
            : base(alarm)
        {
            this.removePatrol = new DelegateCommand<MAlarmPatrol>(RemoveSeletecdObj);
            this.addSelectPatrol = new DelegateCommand<MAlarmPatrol>(AddSeletecdObj);
            this.okCommand = new DelegateCommand(ArriveClick);
        }

        private void RemoveSeletecdObj(MAlarmPatrol obj)
        {

            if (this.ReceivePatrol == null)
                return;
            MAlarmPatrol p = obj as MAlarmPatrol;
            this.ReceivePatrol.Remove(p);
        }
        private void AddSeletecdObj(MAlarmPatrol obj)
        {
            MAlarmPatrol p = obj as MAlarmPatrol;
            if (this.ReceivePatrol == null)
                this.ReceivePatrol = new List<MAlarmPatrol>();
            this.ReceivePatrol.Add(p);

        }
        public async void ArriveClick()
        {
            string ErrorString = null;
            foreach (var item in ArrivedPatrol)
            {
                Result<bool> result = await this.AcceptMession(item.patrol.GroupNo);
                if (!result.Model)
                {//失败的巡组
                    ErrorString += item.patrol.GroupName + ",";
                }
            }
        }


        private ICommand okCommand;

        public ICommand OkCommand
        {
            get { return okCommand; }
            set { okCommand = value; }
        }

        private ICommand removePatrol;

        public ICommand RemovePatrol
        {
            get { return removePatrol; }
            set { removePatrol = value; }
        }

        private ICommand addSelectPatrol;

        public ICommand AddSelectPatrol
        {
            get { return addSelectPatrol; }
        }
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
        public bool Confirmed
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public object Content
        {
            get;
            set;
        }

        public INotification Notification
        {
            get; 
            set;
        }

        public Action FinishInteraction
        {
            get;
            set;
        }
        public Action Activate { get; set; }
    }
}
