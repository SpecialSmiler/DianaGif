using System;
using System.Collections.Generic;
using System.IO;
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
using ImageMagick;
using System.Drawing;


namespace DianaGif
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DianaGifViewModel dianaGifViewModel = new DianaGifViewModel();
			DataContext = dianaGifViewModel;
		}

		//private void myMedia_MediaEnded(object sender, RoutedEventArgs e)
		//{
		//	MediaElement mediaElement = sender as MediaElement;
		//	mediaElement.Position = mediaElement.Position.Add(TimeSpan.FromMilliseconds(1));
		//	//((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
		//}

	}
}
