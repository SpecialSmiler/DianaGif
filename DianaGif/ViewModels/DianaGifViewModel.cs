using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Juster.Common;
using Microsoft.Win32;
using DianaGif.Views;
using System.Collections.ObjectModel;
using System.Linq;

namespace DianaGif
{
	public class FpsDelay
	{
		public string Fps  { get; set; }
		public int Delay { get; set; }
		//public override string ToString()
		//{
		//	return Fps;
		//}
	}
	public class DianaGifViewModel : INotifyPropertyChanged
	{
		private GifHandler gifHandler = new GifHandler();
		private string _srcPath;
		private string _dstPath;
		private Uri _mediaElementSourcePath;
		private string _InfoText;
		private string _fileSizeText;
		private FpsDelay _selectedFpsDelay;


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private List<FpsDelay> _fpsDelayList = new List<FpsDelay>()
		{   
			new FpsDelay(){Fps = "50", Delay = 2},
			new FpsDelay(){Fps = "25", Delay = 4},
			new FpsDelay(){Fps = "20", Delay = 5},
			new FpsDelay(){Fps = "12.5", Delay = 8},
			new FpsDelay(){Fps = "10", Delay = 10}
		};
		public FpsDelay SelectedFpsDelay 
		{ 
			get { return _selectedFpsDelay; }
			set
			{
				_selectedFpsDelay = value;
				OnPropertyChanged("SelectedFpsDelay");
			}
		}

		public ObservableCollection<FpsDelay> FpsDelays
		{
			get{
				var fpsDelays = new ObservableCollection<FpsDelay>(_fpsDelayList);
				SelectedFpsDelay = fpsDelays.FirstOrDefault(fpsDelays => fpsDelays.Fps == "25");
				return fpsDelays;
			}

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
		public Uri MediaElementSourcePath
		{ 
			get => _mediaElementSourcePath; 
			set
			{
				_mediaElementSourcePath = value;
				OnPropertyChanged("MediaElementSourcePath");
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
		public ICommand OpenImagePlayerCommand { get; set; }
		public ICommand RunCommand { get; set; }
		public ICommand ChangeFpsCommand { get; set; }

		public DianaGifViewModel()
		{
			OpenSrcFileCommand = new RelayCommand(OpenSrcFileAction);
			OpenImagePlayerCommand = new RelayCommand(OpenImagePlayerAction);
			RunCommand = new RelayCommand(RunAction);

			//SrcPath = "Image/diana_1.gif";//默认路径
			SrcPath = "";
			InfoText = "";
			MediaElementSourcePath = null;
		}

		private void OpenSrcFileAction()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image files (*.gif)|*.gif|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
			{
				SrcPath = openFileDialog.FileName;
				DstPath = Path.GetDirectoryName(openFileDialog.FileName) + '\\' +Path.GetFileNameWithoutExtension(openFileDialog.FileName) + "_diana.gif";
			}

			gifHandler.OpenGif(SrcPath);
			InfoText = gifHandler.GifInfo();
			//MediaElementSourcePath = new Uri(SrcPath);
		}

		private void OpenImagePlayerAction()
		{
			if (!File.Exists(SrcPath))
			{
				MessageBox.Show("请打开一张确实存在的图片", "啊笑死", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			ImagePlayerView imagePlayerView = new ImagePlayerView(gifHandler.collection);
			imagePlayerView.ShowDialog();
		}

		private void RunAction()
		{
			if (!File.Exists(SrcPath))
			{
				MessageBox.Show("请打开一张确实存在的图片", "绷不住了", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			if (!gifHandler.CreateGif(SelectedFpsDelay.Delay, DstPath, out string resStr))
			{
				MessageBox.Show(resStr, "寄！");
			}
		}
	}
}
