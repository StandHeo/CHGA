using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Pvirtech.Framework.Interactivity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{

    //[Export(typeof(IMainWindowViewModel))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainWindowViewModel : BindableBase, IMainWindowViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICommand showAlarmCommand;
        private readonly IUnityContainer _container;
        public string HeaderInfo
        {
            get { return "常规处警"; }
        }

        public MainWindowViewModel(IRegionManager regionManager,  IUnityContainer container, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            this._container = container;
            _eventAggregator = eventAggregator;
            this.showAlarmCommand = new DelegateCommand(this.ShowAlarmView);
            this.tabSelectedCommand = new DelegateCommand<object>(this.OnTabSelected);
            
        }
        int index = 0;
        private void OnTabSelected(object obj)
        {
            index++;
            if (index==2)
            {
                 
            }
            else
            {
            }
            
        }
         

        public ICommand ShowAlarmCommand { get { return this.showAlarmCommand; } }

        private ICommand tabSelectedCommand;
        public ICommand TabSelectedCommand { get { return this.tabSelectedCommand; } }

        public ICollectionView Users
        {
            get
            {
                return null;
            }
        }

        private void ShowAlarmView()
        {
           
        }
        private bool sendingMessage;
        public bool SendingMessage
        {
            get
            {
                return this.sendingMessage;
            } 
            private set
            {
                if (value!=sendingMessage)
                {
                    sendingMessage = value;
                    this.OnPropertyChanged(()=>SendingMessage);
                }
              
            }
        }
       
    }
    
}
