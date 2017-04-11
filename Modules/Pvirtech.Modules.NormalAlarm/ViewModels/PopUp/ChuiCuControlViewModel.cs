using Prism.Commands;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class ChuiCuControlViewModel : WorkingAlarmInfoViewModel, IConfirmation, IInteractionRequestAware
    {
        public ChuiCuControlViewModel(WorkingAlarmInfoViewModel alarm)
            : base(alarm)
        {
            this.removePatrol = new DelegateCommand<MAlarmPatrol>(DeleteSltPatrol);
            this.selectedPatrol = new DelegateCommand<MAlarmPatrol>(AddSltPatrol);
            this.okCommand = new DelegateCommand(CuiCuClick);
        }

        private void AddSltPatrol(MAlarmPatrol obj)
        {
            MAlarmPatrol p = obj as MAlarmPatrol;
            if (this.UrgePatrol == null)
                UrgePatrol = new List<MAlarmPatrol>();
            this.UrgePatrol.Add(p);
        }

        private ICommand okCommand;

        public ICommand OkCommand
        {
            get { return okCommand; }
        }

        private void DeleteSltPatrol(MAlarmPatrol obj)
        {

            if (this.UrgePatrol == null)
                return;
            MAlarmPatrol p = obj as MAlarmPatrol;
            this.UrgePatrol.Remove(p);
        }

        private async void CuiCuClick()
        {
            if (this.UrgePatrol == null && this.UrgePatrol.Count < 0)
                return;
            string ErrorString = null; 
            foreach (var item in UrgePatrol)
            {
                Result<bool> result = await this.UrgeAlramByIdOrText(item.patrol.GroupNo,"0");
                if (!result.Model)
                {
                    ErrorString += item.patrol.GroupName + ",";
                }
            }
            if (string.IsNullOrEmpty(ErrorString))
            {
                //弹窗提示
            }
        }

        public string Title
        {
            get;
            set;
        }

        public WorkingAlarmInfoViewModel viewModel;
        public object Content
        {
            get;
            set;
        }
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
        public INotification Notification
        {
            get;
            set;
        }
        public Action Activate { get; set; }
        public Action FinishInteraction
        {
            get;
            set;
        }

        private ICommand selectedPatrol;

        public ICommand SelectedPatrol
        {
            get { return selectedPatrol; }
        }

        private ICommand removePatrol;

        public ICommand RemovePatrol
        {
            get { return removePatrol; }
        }

        public bool Confirmed
        {
            get;
            set;
        }
    }
}
