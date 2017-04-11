using Prism.Mvvm;
using Pvirtech.Framework.Interactivity;
using System;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class UCHintWinViewModel : BindableBase,IConfirmation, IInteractionRequestAware
    {

        public UCHintWinViewModel(string title = null,string text = null)
        {
            okCommand = new DelegateCommand(OkClick);
            closeCommand = new DelegateCommand(OFFWinClick);
            if (string.IsNullOrEmpty(text))
                TextInfo = "是否进行此操作？";
            else
                TextInfo = text;
        }

        private void OFFWinClick(object obj)
        {
            this.Confirmed = false;
            this.FinishInteraction();
        }

        private void OkClick(object obj)
        {
            this.Confirmed = true;
            this.FinishInteraction();
        }

        #region 命令/字段

        private ICommand closeCommand;

        public ICommand CloseCommand
        {
            get { return closeCommand; }
        }

        private ICommand okCommand;

        public ICommand OkCommand
        {
            get { return okCommand; }
        }

        private string _TextInfo;

        public string TextInfo
        {
            get { return _TextInfo; }
            set
            {
                if (TextInfo != value)
                {
                    _TextInfo = value;
                    OnPropertyChanged(() => TextInfo);
                }
            }
        }

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

        public bool IsModal
        {
            get;
            set;
        }

        public bool Topmost
        {
            get;
            set;
        }

        public System.Windows.ResizeMode ResizeMode
        {
            get;
            set;
        }

        public System.Windows.WindowState WindowState
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

        public Action Activate
        {
            get;
            set;
        }

        #endregion

    }
}
