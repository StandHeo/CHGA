﻿using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.CommandSystem.ViewModels
{
	public class SystemInfoViewModel: BindableBase
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public InitializationMode InitMode { get; set; }
		public bool IsDefaultShow { get; set; }
		public bool IsReadOnly { get; set; }
		public ModuleState State { get; set; }
		public int MsgCount { get; set; } = 0;
	}
}
