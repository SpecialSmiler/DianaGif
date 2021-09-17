using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ImageMagick;

namespace ImageConverter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<string> fileNames;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void OpenFileButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = true;
			if(openFileDialog.ShowDialog() == true)
			{
				fileNames = new List<string>(); 
				foreach(var file in openFileDialog.FileNames)
				{
					fileNames.Add(file);
					FileNameText.Text += (file + "\n");
				}
			}
		}

		private void ConvertButton_Click(object sender, RoutedEventArgs e)
		{
			//MagickSettings settings = new MagickReadSettings();
			//settings.Format = MagickFormat.WebP;
			foreach (var fileName in fileNames)
			{
				string outputFileName = Path.GetDirectoryName(fileName) + "\\output\\" + Path.GetFileNameWithoutExtension(fileName) + ".ico";
				using(var image = new MagickImage(fileName))
				{
					image.Format = MagickFormat.Ico;
					image.Write(outputFileName);
				}
			}
			fileNames.Clear();
			FileNameText.Text = "转换完了";
		}
	}
}
