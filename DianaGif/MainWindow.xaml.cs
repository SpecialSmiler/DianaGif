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
using DianaGif.Views;


namespace DianaGif
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DianaGifViewModel dianaGifViewModel;
		public MainWindow()
		{
			InitializeComponent();
			dianaGifViewModel = new DianaGifViewModel();
			DataContext = dianaGifViewModel;
		}

		private void OpenImagePlayerButton_Click(object sender, RoutedEventArgs e)
		{
			if (!File.Exists(dianaGifViewModel.SrcPath))
			{
				MessageBox.Show("请打开一张确实存在的图片", "啊笑死", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			ImagePlayerView imagePlayerView = new ImagePlayerView();
			//imagePlayerView.PlayGif(gifHandler.collection);
			imagePlayerView.Grid_1.Width = dianaGifViewModel.GifHandler.collection[0].Width;
			imagePlayerView.Grid_1.Height = dianaGifViewModel.GifHandler.collection[0].Height;
			imagePlayerView.PlayGif(dianaGifViewModel.SrcPath);
			imagePlayerView.Owner = this;
			imagePlayerView.ShowDialog();
		}

	}
}
