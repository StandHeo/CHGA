using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
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
		public NormalAlarmModule(IRegionManager regionManager, IUnityContainer unityContainer)
		{
			_regionManager = regionManager;
			_unityContainer = unityContainer;
		}

		public void Initialize()
		{
			//_regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));
		  //	_regionManager.RequestNavigate("MainRegion", navigatePath);
			_unityContainer.RegisterTypeForNavigation<MainWindow>(name: "NormalAlarmModule");
		}
	}
}
