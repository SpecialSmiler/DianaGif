using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Juster.Common;
using Microsoft.Win32;

namespace DianaGif
{
	public class DianaGifModel : INotifyPropertyChanged
	{
		private GifHandler gifHandler = new GifHandler();
		private string _srcPath;
		private string _dstPath;
		private string _InfoText;
		private string _fileSizeText;

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public string SrcPath
		{
			get { return _srcPath; }
			set 
			{ 
				_srcPath = value;
				OnPropertyChanged("SrcPath");
			}
		}
		public string DstPath 
		{
			get { return _dstPath; }
			set 
			{ 
				_dstPath = value;
				OnPropertyChanged("DstPath");
			} 
		}
		public string InfoText 
		{ 
			get { return _InfoText;  }
			set 
			{
				_InfoText = value;
				OnPropertyChanged("InfoText");
			}
		}
		public string FileSizeText { get => _fileSizeText; set => _fileSizeText = value; }

		public ICommand OpenSrcFileCommand { get; set; }
		public ICommand AnalyzeCommand { get; set; }
		public ICommand RunCommand { get; set; }

		public DianaGifModel()
		{
			OpenSrcFileCommand = new RelayCommand(OpenSrcFileAction);
			AnalyzeCommand = new RelayCommand(AnalyzeAction);
			RunCommand = new RelayCommand(RunAction);

			SrcPath = "Image/diana_1.gif";//默认路径
			InfoText = "";
		}

		private void OpenSrcFileAction()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image files (*.gif)|*.gif|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
			{
				SrcPath = openFileDialog.FileName;
			}
		}

		private void AnalyzeAction()
		{
			if (!File.Exists(SrcPath))
			{
				MessageBox.Show("文件不存在", "啊笑死", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			gifHandler.OpenGif(SrcPath);
			InfoText = gifHandler.GifInfo();
		}

		private void RunAction()
		{

		}

	}
}
