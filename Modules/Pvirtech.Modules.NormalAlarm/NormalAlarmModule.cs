using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
using Pvirtech.Framework.Domain;
using Pvirtech.Modules.NormalAlarm.Views;

namespace Pvirtech.Modules.NormalAlarm
{
    [Roles("User")]
	[ModuleInfo(Id = "NormalAlarmModule",Title ="常规处警")]
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

            //this._container.RegisterType<ICommonRepository, CommonRepository>(new ContainerControlledLifetimeManager());

            this._regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(MainWindow));

            // View discovery
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._container.Resolve<VAlarmList01>());
            this._regionManager.RegisterViewWithRegion(RegionNames.AlarmTabRegion, () => this._container.Resolve<VAlarmList02>());

        }
	}
}
