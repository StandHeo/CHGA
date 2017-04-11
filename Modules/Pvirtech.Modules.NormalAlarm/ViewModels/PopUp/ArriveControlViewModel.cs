using Prism.Commands;
using Pvirtech.Framework.Interactivity;
using System;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class ArriveControlViewModel:IConfirmation, IInteractionRequestAware
    {
        //编译警告注释 20151211 BINGLE
        //private readonly IUserRepository _userRepository;
        public ArriveControlViewModel(WorkingAlarmInfoViewModel alarm)
        {
            this.okCommand = new DelegateCommand(ArriveClick);
        }

        public void ArriveClick()
        {
        }

        //编译警告注释 20151211 BINGLE
        //private WorkingAlarmInfoViewModel _ViewModel;

        public WorkingAlarmInfoViewModel ViewModel
        {
            get;
            set;
        }

        public string Title { get; set; }

        public object Content { get; set; }

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

        public bool Confirmed
        {
            get;
            set;
        }

        private ICommand okCommand;

        public ICommand OkCommand
        {
            get { return okCommand; }
            set { okCommand = value; }
        }
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
    }
}
