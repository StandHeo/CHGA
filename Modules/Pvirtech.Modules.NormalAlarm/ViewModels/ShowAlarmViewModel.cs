using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    //[Export(typeof(ShowAlarmViewModel))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public  class ShowAlarmViewModel: BindableBase, IShowAlarmViewModel
    {
        //编译警告注释 20151211 BINGLE
        //private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        //private readonly IUserRepository _userRepository;
        //private readonly ICommand showAlarmCommand;
       
        public ShowAlarmViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
           
            //mc = new MessageControl();
            //MessageInfo word = new MessageInfo();
            //word.txt_info = "警情目前正在总调试阶段，敬请期待...";
            //mc.MyNotifyIcon.ShowCustomBalloon(word, PopupAnimation.Slide, 12000);
        }

       

        public string HeaderInfo
        {
            get
            {
                return "警情处理";
            }
        }
    }
}
