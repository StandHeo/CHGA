using Microsoft.Practices.Unity;
using Prism.Unity; 
using System.Windows;
using Prism.Modularity;
using Pvirtech.Framework;
using System.Security.Principal;
using System.Threading;
using System.IO;
using System.Reflection;
using System;
using System.Linq;
using Pvirtech.CommandSystem.ViewModels;
using Prism.Events;
using Pvirtech.CommandSystem.Views;

namespace Pvirtech.CommandSystem
{
    class Bootstrapper : UnityBootstrapper
    { 
		protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
			var ident = WindowsIdentity.GetCurrent();
			var principal = new GenericPrincipal(ident, new string[] { "User" });
			//Thread.CurrentPrincipal = principal; 
			AppDomain.CurrentDomain.SetThreadPrincipal(principal); 
            Application.Current.MainWindow.Show();
        }
		protected override void ConfigureServiceLocator()
		{ 
			base.ConfigureServiceLocator();
		//	Container.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());
		}
		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();
			Container.RegisterType<IModuleInitializer, RoleBasedModuleInitializer>(new ContainerControlledLifetimeManager());
		}
		protected override IModuleCatalog CreateModuleCatalog()
		{ 
			DynamicDirectoryModuleCatalog catalog = new DynamicDirectoryModuleCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules"));
			return catalog;
		}
	}
}
