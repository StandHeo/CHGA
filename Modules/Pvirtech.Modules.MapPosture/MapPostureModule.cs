using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
using System;
using Prism.Unity;

namespace Pvirtech.Modules.MapPosture
{ 
	
	[Roles("User")] 
	[ModuleInfo(Id = "MapPostureModule",Title ="成华态势",IsDefaultShow =true,InitMode =InitializationMode.OnDemand)]
	public class MapPostureModule : IModule
	{
		private readonly IRegionManager _regionManager;
		private readonly IUnityContainer _unityContainer;
		public MapPostureModule(IRegionManager regionManager, IUnityContainer unityContainer)
		{
			_regionManager = regionManager;
			_unityContainer = unityContainer;
		}

		public void Initialize()
		{
			_unityContainer.RegisterTypeForNavigation<MainWindow>("MapPostureModule");
		}
	}
}
