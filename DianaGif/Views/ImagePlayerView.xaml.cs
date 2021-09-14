using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using ImageMagick;

namespace DianaGif.Views
{
	/// <summary>
	/// Interaction logic for ImageBrowserView.xaml
	/// </summary>
	public partial class ImagePlayerView : Window
	{

		public ImagePlayerView()
		{
			InitializeComponent();
		}

		//      public void PlayGif(MagickImageCollection collection)
		//{
		//	isPlay = true;

		//	Grid_1.Width = collection[0].Width;
		//	Grid_1.Height = collection[0].Height;
		//	Grid_1.MinWidth = collection[0].Width;
		//	Grid_1.MinHeight = collection[0].Height;

		//	List<(BitmapSource image, int delay)> images = new List<(BitmapSource image, int delay)>();
		//	List<BitmapSource> temp = new List<BitmapSource>();
		//	List<Bitmap> temp2 = new List<Bitmap>();

		//	foreach (MagickImage magickImage in collection)
		//	{

		//		//magickImage.AdaptiveResize(100, 100);
		//		Bitmap bitmap = magickImage.ToBitmap();
		//		images.Add((BitmapExtension.ConvertBitmap(bitmap), magickImage.AnimationDelay));
		//		//temp.Add(BitmapExtension.ConvertBitmap(bitmap));
		//		//temp2.Add(bitmap);
		//		bitmap.Dispose();

		//	}
		//	int n = 0;
		//	Task.Run(async () =>
		//	{
		//		while (isPlay)
		//		{
		//			await Dispatcher.InvokeAsync(() =>
		//			{
		//				GifImage.Source = images[n].image;

		//			});
		//			n++;
		//			if (n == images.Count)
		//			{
		//				n = 0;
		//			}
		//			await Task.Delay(20);
		//		}
		//		//images.Clear();
		//		//collection = null;
		//		//GifImage.Source = null;
		//		//GC.Collect();
		//	});

		//	//images.Clear();
		//	//foreach(var image in temp2)
		//	//{
		//	//	image.Dispose();
		//	//}
		//}

		public void PlayGif(string path)
		{
			Uri uri = new Uri(path);
			GifImage.Source = uri;
		}
		private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
		}

	}
}
