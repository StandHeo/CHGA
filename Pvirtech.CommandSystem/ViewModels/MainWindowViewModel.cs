using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using Pvirtech.CommandSystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.CommandSystem.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		private string _title = "成华分局指挥调度平台";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}
		private readonly IEventAggregator _eventAggregator;
		private readonly IUnityContainer _container;
		private readonly IModuleCatalog _moduleCatalog;
		private readonly IModuleManager _moduleManager;
		public MainWindowViewModel(IUnityContainer container, IEventAggregator eventAggregator, IModuleCatalog moduleCatalog, IModuleManager moduleManager)
		{
			_container = container;
			_eventAggregator = eventAggregator;
			_moduleCatalog = moduleCatalog;
			_moduleManager = moduleManager; 
		}
		/// <summary>
		/// 加载设置选项
		/// </summary>
		public void InitLoadSetting()
		{
			if (Settings.Default.IsBusy)
			{
				//ImgToolTip = "忙碌";
				//ImgSource = "pack://application:,,,/Pvirtech.Framework.Resources;component/Images/manglu.png";
			}
			else
			{
				//ImgToolTip = "空闲";
				//ImgSource = "pack://application:,,,/Pvirtech.Framework.Resources;component/Images/kongxian.png";
			}
		}
		 
		[InjectionMethod]
		public void Init()
		{
			int s = 0;
		}
	}
}
