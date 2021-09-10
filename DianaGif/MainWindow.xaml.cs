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
			DataContext = new DianaGifViewModel();
			//PlayGif();
		}

		private void myMedia_MediaEnded(object sender, RoutedEventArgs e)
		{
			MediaElement mediaElement = sender as MediaElement;
			mediaElement.Position = mediaElement.Position.Add(TimeSpan.FromMilliseconds(1));
			//((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		//private void PlayGif()
		//{
		//	string src = "D:\\Pictures\\Asoul_pic\\dianagif\\10.gif";

		//	var image = new List<(BitmapSource image, int delay)>();
		//	using (var collection = new MagickImageCollection(src))
		//	{
		//		collection.Coalesce();
		//		foreach(MagickImage magickImage in collection)
		//		{
		//			//magickImage.AdaptiveResize(100, 100);
		//			image.Add((BitmapExtension.ConvertBitmap(magickImage.ToBitmap()), magickImage.AnimationDelay * 10 * 4 / 5));
		//		}
		//	}
		//	int n = 0;
		//	Task.Run(async () =>
		//	{
		//		while (true)
		//		{
		//			await Dispatcher.InvokeAsync(() =>
		//			{
		//				GifImage.Source = image[n].image;

		//			});
		//			n++;
		//			if(n == image.Count)
		//			{
		//				n = 0;
		//			}
		//			await Task.Delay(image[n].delay);
		//		}
		//	});
		//}
	}
}
