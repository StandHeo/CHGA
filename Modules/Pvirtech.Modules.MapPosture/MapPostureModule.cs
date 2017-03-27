using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.Modules.MapPosture
{ 
	
	[Roles("User")] 
	[ModuleInfo(Id = "MapPostureModule",Title ="成华态势",IsDefaultShow =true,InitMode =InitializationMode.OnDemand)]
	public class MapPostureModule : IModule
	{
		private readonly IRegionManager _regionManager;
		public MapPostureModule(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));
		}
	}
}
