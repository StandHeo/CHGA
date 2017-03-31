using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Pvirtech.Framework.Interactivity;
using System;
using System.Windows;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    public class SendMessageViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {   private readonly IEventAggregator _eventAggregator;
        private string message; 
        public SendMessageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            this.OKCommand = new DelegateCommand(this.SendMessage);
            this.CancelCommand = new DelegateCommand(this.Cancel);
        }
      
        public bool Confirmed { get; set; }
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public string Title { get; set; }

        public object Content { get; set; }

        public INotification Notification { get; set; }

        public Action FinishInteraction { get; set; }
        public Action Activate { get; set; }
        public ICommand OKCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }
        public WindowState WindowState { get; set; }
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                if (value!=message)
                {
                    message = value;
                    this.OnPropertyChanged(()=>Message);
                }             
            }
        }

        private void SendMessage()
        {
            this.Confirmed = true;
            //this.FinishInteraction();
        }

        private void Cancel()
        {
            this.Confirmed = false;
            this.FinishInteraction();
        } 
        public bool IsModal { get; set; }
       
    }
}
