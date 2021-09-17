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
using System.Text.RegularExpressions;
using System.Globalization;

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
			DianaEyes.MouseDown += ShowDocument;
		}



		private void OpenImagePlayerButton_Click(object sender, RoutedEventArgs e)
		{
			if (!File.Exists(dianaGifViewModel.SrcPath))
			{
				DianaMessageBox.Show("啊笑死", "请打开一张确实存在的图片");
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

		private static readonly Regex _regex = new Regex("^[0-9]+$");
		private void NumericText_TextInputValidation(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !_regex.IsMatch(e.Text);//把这些不是数字的给Handle掉
		}
		private void NumericText_PastingValidation(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(String)))
			{
				String text = (String)e.DataObject.GetData(typeof(String));

				if (!_regex.IsMatch(text))
				{
					e.CancelCommand();
				}
			}
			else
			{
				e.CancelCommand();
			}
		}
		private void KeyDownValidation(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Space)
			{
				e.Handled = true;
			}
		}

		private void ShowDocument(object sender, MouseButtonEventArgs e)
		{
			DianaGifDocView docView = new DianaGifDocView();
			docView.Show();
		}
	}
}
