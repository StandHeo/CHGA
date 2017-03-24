using Prism.Modularity;
using Prism.Regions;
using Pvirtech.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.Modules.NormalAlarm
{
    [Roles("User")]
	public class NormalAlarmModule:IModule
	{
		private readonly IRegionManager _regionManager;
		public NormalAlarmModule(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));
		}
	}
}
