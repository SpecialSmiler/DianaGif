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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Media;

namespace DianaGif
{
	public class Delay
	{
		public string DelayName { get; set; }
		public int DelayValue { get; set; }

		public Delay(string _delayName, int _delayValue)
		{
			DelayName = _delayName;
			DelayValue = _delayValue;
		}
	}

	public class DianaGifViewModel : INotifyPropertyChanged
	{
		private GifHandler gifHandler = new GifHandler();
		private string _srcPath;
		private string _dstPath;
		private Uri _mediaElementSourcePath;
		private string _InfoText;
		private string _otherInfo;
		private Delay _selectedDelay;
		private bool _isIdle;
		private int _progressValue;
		private List<Delay> _delays;
		private ImageSource _currentBGImage;
		private bool _isCustomSize;
		private int _customWidth;
		private int _customHeight;
		private bool _isSettingWidth;
		private bool _isSettingHeight;

		public List<BitmapImage> bgImages = new List<BitmapImage>();
		public List<BitmapImage> warningImages = new List<BitmapImage>();
		public Random rand = new Random();

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		//private List<FpsDelay> _fpsDelayList = new List<FpsDelay>()
		//{   
		//	new FpsDelay(){Fps = "50", Delay = 2},
		//	new FpsDelay(){Fps = "25", Delay = 4},
		//	new FpsDelay(){Fps = "20", Delay = 5},
		//	new FpsDelay(){Fps = "12.5", Delay = 8},
		//	new FpsDelay(){Fps = "10", Delay = 10}
		//};

		//private List<(string,int)> delayList = new List<(string, int)>()
		//{
		//	("2", 2 ),
		//	("3", 3 ),
		//	("4", 4 ),
		//	("5", 5 ),
		//	("6", 6 ),
		//	("7", 7 ),
		//	("8", 8 ),
		//	("9", 9 ),
		//	("10", 10 )
		//};

		private int[] delayArr = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 };

		#region Properties
		public Delay SelectedDelay 
		{ 
			get { return _selectedDelay; }
			set
			{
				_selectedDelay = value;
				OnPropertyChanged("SelectedDelay");
			}
		}

		public List<Delay> Delays
		{
			//get{
			//	var oc = new ObservableCollection<(string, int)>();
			//	oc.Add(("-", -1));
			//	foreach(var d in delayList)
			//	{
			//		if(d.Item2 > gifHandler.Delay)
			//		{
			//			oc.Add(d);
			//		}
			//	}
			//	SelectedDelay = oc.First();
			//	return oc;
			//}
			get{ return _delays; }
			set
			{
				_delays = value;
				if(_delays.Count>0)
				{
					SelectedDelay = _delays[0];
				}
				OnPropertyChanged("Delays");
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

		public bool IsIdle
		{
			get => _isIdle;
			set
			{
				_isIdle = value;
				OnPropertyChanged("IsIdle");
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

		public ImageSource CurrentBGImage
		{
			get => _currentBGImage;
			set
			{
				_currentBGImage = value;
				OnPropertyChanged("CurrentBGImage");
			}
		}

		public bool IsCustomSize 
		{ 
			get => _isCustomSize; 
			set
			{
				_isCustomSize = value;
				OnPropertyChanged("IsCustomSize");
			}
		}

		public int CustomWidth
		{
			get =>  _customWidth;
			set
			{
				_customWidth = value;
				OnPropertyChanged("CustomWidth");
			}
		}
		public int CustomHeight
{
			get =>  _customHeight;
			set
			{
				_customHeight = value;
				OnPropertyChanged("CustomHeight");
			}
		}
		public bool IsSettingWidth 
		{ 
			get => _isSettingWidth; 
			set
			{
				_isSettingWidth = value;
				if (_isSettingWidth)
				{
					CustomWidth = gifHandler.Width;
				}
				else
				{
					CustomWidth = 0;
				}
				OnPropertyChanged("IsSettingWidth");
			}
		}
		public bool IsSettingHeight 
		{ 
			get => _isSettingHeight; 
			set
			{
				_isSettingHeight = value;
				if(_isSettingHeight)
				{
					CustomHeight = gifHandler.Height;
				}
				else
				{
					CustomHeight = 0;
				}
				OnPropertyChanged("IsSettingHeight");
			}
		}
		#endregion

		public ICommand OpenSrcFileCommand { get; set; }
		//public ICommand SetDstPathCommand { get; set; }
		public ICommand RunCommand { get; set; }
		internal GifHandler GifHandler { get => gifHandler; }


		public DianaGifViewModel()
		{
			OpenSrcFileCommand = new RelayCommand(OpenSrcFileAction);
			//SetDstPathCommand = new RelayCommand(SetDstPathAction);
			RunCommand = new RelayCommand(RunAction);

			SrcPath = "";
			InfoText = "";
			MediaElementSourcePath = null;
			IsIdle = true;
			OtherInfo = "";
			Delays = new List<Delay>() { new Delay("-",-1) };
			IsCustomSize = false;

			bgImages = LoadFiles("./Image/BG/");
			warningImages = LoadFiles("./Image/Warning/");
			CurrentBGImage = GetItemRandomly(bgImages);
			IsSettingWidth = true;
			IsSettingHeight = false;
		}

		//打开文件
		private async void OpenSrcFileAction()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image files (*.gif)|*.gif|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
			{
				SrcPath = openFileDialog.FileName;
			}
			else
			{
				return;
			}

			IsIdle = false;
			OtherInfo = "正在解析图片...";
			ProgressValue = 100;
			int preDelay = gifHandler.Delay;
			await Task.Run(()=> { gifHandler.OpenGif(SrcPath); });
			InfoText = gifHandler.GifInfo();
			OtherInfo = "图片解析完成";
			ProgressValue = 0;
			if(preDelay!=gifHandler.Delay)//更新ComboBox
			{
				List<Delay> NewDelays = new List<Delay>();
				NewDelays.Add(new Delay("-", -1));
				foreach (var num in delayArr)
				{
					if (num >= gifHandler.Delay)
					{
						NewDelays.Add(new Delay(num.ToString(), num));
					}
				}
				Delays = NewDelays;
			}
			IsIdle = true;
			CurrentBGImage = GetItemRandomly(bgImages);
			if(IsSettingWidth)
			{
				CustomWidth = gifHandler.Width;
			}
			if(_isSettingHeight)
			{
				CustomHeight = gifHandler.Height;
			}
		}


		//创建图片，润！
		private async void RunAction()
		{
			if (!File.Exists(SrcPath))
			{
				//MessageBox.Show("请打开一张确实存在的图片", "绷不住了", MessageBoxButton.OK, MessageBoxImage.Warning);
				DianaMessageBox.Show("绷不住了","请打开一张确实存在的图片",GetItemRandomly(warningImages));
				return;
			}
			if (gifHandler.Delay >=10)
			{
				DianaMessageBox.Show("救不了", "当前图片帧数已经很低了（delay>=10)", GetItemRandomly(warningImages));
				return;
			}
			if (SelectedDelay.DelayValue < 0)
			{
				DianaMessageBox.Show("快快快快快快", "请在“延迟”下拉菜单中选择一个值", GetItemRandomly(warningImages));
				return;
			}
			if (gifHandler.collection.Count == 0)
			{
				DianaMessageBox.Show("寄", "图像序列为空", GetItemRandomly(warningImages));
				return;
			}

			int width = 0;
			int height = 0;
			if (IsCustomSize)
			{

				width = CustomWidth;
				height = CustomHeight;
				
				DstPath = Path.GetDirectoryName(SrcPath) +
							'\\' + Path.GetFileNameWithoutExtension(SrcPath) +
							$"_diana_{gifHandler.Delay}_to_{SelectedDelay.DelayName}_resize_";
				if(width > 0)
				{
					DstPath += $"w_{width}.gif";
				}
				if(height > 0)
				{
					DstPath += $"h_{height}.gif";
				}
			}
			else
			{
				DstPath = Path.GetDirectoryName(SrcPath) +
							'\\' + Path.GetFileNameWithoutExtension(SrcPath) +
							$"_diana_{gifHandler.Delay}_to_{SelectedDelay.DelayName}.gif";
			}

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Image files (*.gif)|*.gif|All files (*.*)|*.*";
			saveFileDialog.InitialDirectory = DstPath;
			saveFileDialog.FileName = Path.GetFileName(DstPath);
			if (saveFileDialog.ShowDialog() == true)
			{
				DstPath = saveFileDialog.FileName;
			}
			else
			{
				return;
			}

			IsIdle = false;
			OtherInfo = "正在输出GIF...";
			ProgressValue = 100;

			await Task.Run(() =>
			{
				gifHandler.CompressGif(SelectedDelay.DelayValue, DstPath, width, height);
			});

			OtherInfo = "GIF文件输出完成";
			ProgressValue = 0;
			IsIdle = true;
		}

		private List<BitmapImage> LoadFiles(string directory)
		{
			List<BitmapImage> imgs = new List<BitmapImage>();
			foreach(string filename in Directory.GetFiles(directory))
			{
				imgs.Add(new BitmapImage(new Uri("pack://application:,,," + filename)));
			}
			return imgs;
		}

		public T GetItemRandomly<T>(List<T> items)
		{
			return items[rand.Next(items.Count)];
		}
	}
}
