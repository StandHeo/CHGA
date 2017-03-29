using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
using Pvirtech.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.Modules.NormalAlarm
{
    [Roles("User")]
	[ModuleInfo(Id = "NormalAlarmModule",Title ="常规处警",InitMode =InitializationMode.OnDemand)]
	public class NormalAlarmModule:IModule
	{
		private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        public NormalAlarmModule(IUnityContainer container, IRegionManager regionManager)
		{
			_regionManager = regionManager;
            _container = container;
        }

		public void Initialize()
		{
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));

            this._regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(MainWindow));

            // View discovery
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._container.Resolve<Pvirtech.Modules.NormalAlarm.Views.WorkingAlarms>());
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._container.Resolve<Pvirtech.Modules.NormalAlarm.Views.CompletedAlarms>());
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));
		}
	}
}
