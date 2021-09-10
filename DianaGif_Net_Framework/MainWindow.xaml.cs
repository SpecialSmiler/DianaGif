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
using ImageMagick;

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
		}

		private void PlayGif()
		{
			string src = "D:\\Pictures\\Asoul_pic\\dianagif\\50.gif";

			var image = new List<(BitmapSource image, int delay)>();
			using (var collection = new MagickImageCollection(src))
			{
				collection.Coalesce();
				foreach (MagickImage magickImage in collection)
				{
					magickImage.AdaptiveResize(100, 100);
					//image.Add((magickImage, magickImage.AnimationDelay * 10));
				}
			}
		}


	}

}
