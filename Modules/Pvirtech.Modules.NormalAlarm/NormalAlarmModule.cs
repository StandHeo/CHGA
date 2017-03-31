using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
using Pvirtech.Framework.Domain;
using System;
using Prism.Unity;
using Pvirtech.Modules.NormalAlarm.ViewModels;
using Pvirtech.Modules.NormalAlarm.Views;

namespace Pvirtech.Modules.NormalAlarm
{
    [Roles("User")]
    [ModuleInfo(Id = "NormalAlarmModule", Title = "常规处警", InitMode = InitializationMode.OnDemand)]
    public class NormalAlarmModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;
        public NormalAlarmModule(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _unityContainer = container;
        }

        public void Initialize()
        {


            _unityContainer.RegisterTypeForNavigation<MainWindow>("NormalAlarmModule");
            _unityContainer.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<WorkingAlarmsViewModel>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<CompletedAlarmsViewModel>(new ContainerControlledLifetimeManager());

            _unityContainer.RegisterType<WorkingAlarms>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<CompletedAlarms>(new ContainerControlledLifetimeManager());
            // View discovery
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._unityContainer.Resolve<WorkingAlarms>());
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._unityContainer.Resolve<CompletedAlarms>());

        }
    }
}
