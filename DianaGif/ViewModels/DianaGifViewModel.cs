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
using System.Threading;
using System.Threading.Tasks;

namespace DianaGif
{
	public class FpsDelay
	{
		public string Fps  { get; set; }
		public int Delay { get; set; }

	}
	public class DianaGifViewModel : INotifyPropertyChanged
	{
		private GifHandler gifHandler = new GifHandler();
		private string _srcPath;
		private string _dstPath;
		private Uri _mediaElementSourcePath;
		private string _InfoText;
		private string _otherInfo;
		private FpsDelay _selectedFpsDelay;
		private bool _isImageLoaded;
		private int _progressValue;

		//private ImagePlayerView imagePlayerView;

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

		public bool IsImageLoaded
		{
			get => _isImageLoaded;
			set
			{
				_isImageLoaded = value;
				OnPropertyChanged("IsImageLoaded");
			}
		}

		public int ProgressValue 
		{ 
			get => _progressValue; 
			set
			{
				_progressValue = value;
				OnPropertyChanged("ProgressValue");
			}
		}

		public string OtherInfo 
		{
			get => _otherInfo; 
			set
			{
				_otherInfo = value;
				OnPropertyChanged("OtherInfo");
			}
		}


		public ICommand OpenSrcFileCommand { get; set; }
		public ICommand OpenImagePlayerCommand { get; set; }
		public ICommand RunCommand { get; set; }
		public ICommand ChangeFpsCommand { get; set; }


		public DianaGifViewModel()
		{
			OpenSrcFileCommand = new RelayCommand(OpenSrcFileAction);
			OpenImagePlayerCommand = new RelayCommand(OpenPlayerAction);
			RunCommand = new RelayCommand(RunAction);

			SrcPath = "";
			InfoText = "";
			MediaElementSourcePath = null;
			IsImageLoaded = false;
			OtherInfo = "";
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

			Thread thread = new Thread(new ThreadStart(() =>
			{
				ProgressValue = 30;
				gifHandler.OpenGif(SrcPath);
				ProgressValue = 90;
				InfoText = gifHandler.GifInfo();
				ProgressValue = 100;
				Thread.Sleep(100);
				ProgressValue = 0;
				IsImageLoaded = true;
				OtherInfo = "图片解析完成";
			}));
			thread.Start();

			//Task task = Task.Run(async () =>
			//{
			//	await gifHandler.OpenGif(SrcPath);
			//	InfoText = gifHandler.GifInfo();
			//});

		}

		private void OpenPlayerAction()
		{
			if (!File.Exists(SrcPath))
			{
				MessageBox.Show("请打开一张确实存在的图片", "啊笑死", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			ImagePlayerView imagePlayerView = new ImagePlayerView();
			//imagePlayerView.PlayGif(gifHandler.collection);
			imagePlayerView.Grid_1.Width = gifHandler.collection[0].Width;
			imagePlayerView.Grid_1.Height = gifHandler.collection[0].Height;
			imagePlayerView.PlayGif(SrcPath);
			imagePlayerView.ShowDialog();
			//GC.Collect();
			//imagePlayerView = null;
			
		}


		private void RunAction()
		{
			if (!File.Exists(SrcPath))
			{
				MessageBox.Show("请打开一张确实存在的图片", "绷不住了", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			Thread thread = new Thread(new ThreadStart(() =>
			{
				if (!gifHandler.CreateGif(SelectedFpsDelay.Delay, DstPath, out string resStr))
				{
					MessageBox.Show(resStr, "寄！");
				}
			}));

			thread.Start();
			
		}

	}
}
