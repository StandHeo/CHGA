using System.Windows;

namespace Pvirtech.CommandSystem.Views
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
	
		//[InjectionMethod]
		//public void Load(MainWindowViewModel viewModel)
		//{
		//	this.DataContext = viewModel;
		//}
		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}

		private void LaunchMahAppsOnGitHub(object sender, RoutedEventArgs e)
		{
			 
		}

		private void LauchCleanDemo(object sender, RoutedEventArgs e)
		{

		}
	
	}
}
