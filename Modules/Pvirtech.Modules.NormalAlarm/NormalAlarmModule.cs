using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
using Pvirtech.Framework.Domain;
using System;
using Prism.Unity;

namespace Pvirtech.Modules.NormalAlarm
{
    [Roles("User")]
	[ModuleInfo(Id = "NormalAlarmModule",Title ="常规处警",InitMode =InitializationMode.OnDemand)]
	public class NormalAlarmModule:IModule
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

            // View discovery
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._unityContainer.Resolve<Pvirtech.Modules.NormalAlarm.Views.WorkingAlarms>());
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._unityContainer.Resolve<Pvirtech.Modules.NormalAlarm.Views.CompletedAlarms>());
            
		}
	}
}
