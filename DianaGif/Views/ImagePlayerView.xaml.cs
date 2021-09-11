using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Media.Imaging;

using ImageMagick;

namespace DianaGif.Views
{
	/// <summary>
	/// Interaction logic for ImageBrowserView.xaml
	/// </summary>
	public partial class ImagePlayerView : Window
	{
        private MagickImageCollection collection;
		private List<(BitmapSource image, int delay)> images;
		public ImagePlayerView()
		{
			InitializeComponent();
		}

		public ImagePlayerView(MagickImageCollection _collection)
        {
            collection = _collection;
            InitializeComponent();
			Grid_1.Width  = collection[0].Width;
			Grid_1.Height = collection[0].Height;
			Grid_1.MinWidth = collection[0].Width;
			Grid_1.MinHeight = collection[0].Height;
			//ImagePlayerWindow.Width = collection[0].Width + 100;
			//ImagePlayerWindow.Height = collection[0].Height+ 100;
			//ImagePlayerWindow.MinWidth = collection[0].Width + SystemParameters.BorderWidth*2;
			//ImagePlayerWindow.MinHeight = collection[0].Height + SystemParameters.CaptionHeight + SystemParameters.BorderWidth * 2;
			//GifImage.Width = collection[0].Width;
			//GifImage.Height = collection[0].Height;
			PlayGif();
        }

        public void PlayGif()
		{
			images = new List<(BitmapSource image, int delay)>();
			foreach (MagickImage magickImage in collection)
			{
				//magickImage.AdaptiveResize(100, 100);
				images.Add((BitmapExtension.ConvertBitmap(magickImage.ToBitmap()), magickImage.AnimationDelay * 10));
			}
			int n = 0;
			Task.Run(async () =>
			{
				while (true)
				{
					await Dispatcher.InvokeAsync(() =>
					{
						GifImage.Source = images[n].image;

					});
					n++;
					if (n == images.Count)
					{
						n = 0;
					}
					await Task.Delay(images[n].delay);
				}
			});
		}
	}
}
