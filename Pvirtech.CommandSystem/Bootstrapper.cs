﻿using Microsoft.Practices.Unity;
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
			Thread.CurrentPrincipal = principal;
            Application.Current.MainWindow.Show();
        } 
         
		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();
			Container.RegisterType<IModuleInitializer, RoleBasedModuleInitializer>(new ContainerControlledLifetimeManager());
		}
		protected override IModuleCatalog CreateModuleCatalog()
		{ 
			DynamicDirectoryModuleCatalog catalog = new DynamicDirectoryModuleCatalog(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Modules"));
			return catalog;
		}
	}
}