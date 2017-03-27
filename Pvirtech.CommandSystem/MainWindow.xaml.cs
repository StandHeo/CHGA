using Microsoft.Practices.Unity;
using Pvirtech.CommandSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pvirtech.CommandSystem
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow 
	{
		public MainWindow()
		{
			InitializeComponent();
		}
	
		[InjectionMethod]
		public void Load(MainWindowViewModel viewModel)
		{
			this.DataContext = viewModel;
		}
		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}

		private void LaunchMahAppsOnGitHub(object sender, RoutedEventArgs e)
		{
			 
		}

		private void LauchCleanDemo(object sender, RoutedEventArgs e)
		{

		}

		private void LstBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
