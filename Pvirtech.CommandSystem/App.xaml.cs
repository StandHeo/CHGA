using Pvirtech.CommandSystem.Properties;
using Pvirtech.Framework.Domain;
using Pvirtech.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pvirtech.CommandSystem
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

            GlobalConfig.GetUrl = Settings.Default.QureyMapUrl;
            string url = Settings.Default.QueryUrl;

            string defaultStr = GlobalConfig.InitLoadConfig(url);
            if (!string.IsNullOrEmpty(defaultStr))//加载配置文件
            {
                System.Windows.MessageBox.Show(defaultStr);
                Application.Current.Shutdown(-1);
                return;
            }

            //初始化HTTP命令中心 替换HTTP命令中心中的HOST地址 BINGLE
            CommandCenter.Start(GlobalConfig.GetInstance().BusinessCenter1, GlobalConfig.GetInstance().BusinessCenter2);

            var bootstrapper = new Bootstrapper();
			bootstrapper.Run();
		}
	}
}
