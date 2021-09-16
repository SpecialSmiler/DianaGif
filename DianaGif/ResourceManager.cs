using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Windows.Media.Imaging;

namespace DianaGif
{
	public class ResourceManager
	{
		private static ResourceManager instance;
		public static ResourceManager Instance 
		{
			get
			{
				if (instance == null)
				{
					instance = new ResourceManager();
				}
				return instance;
			}
		}

		private Random rand = new Random();
		private List<BitmapImage> bgImages = new List<BitmapImage>();
		private List<BitmapImage> warningImages = new List<BitmapImage>();
		private List<SoundPlayer> sounds = new List<SoundPlayer>();

		public bool isPlaySound = false;

		private ResourceManager()
		{
			bgImages = LoadImages("./Image/BG/");
			warningImages = LoadImages("./Image/Warning/");
			sounds = LoadSounds("./Sound/");
		}

		public BitmapSource GetBGImageRandomly()
		{
			return GetItemRandomly(bgImages);
		}

		public BitmapSource GetWarningImageRandomly()
		{
			return GetItemRandomly(warningImages);
		}

		public void PlaySoundRandomly()
		{
			if(isPlaySound)
			{
				GetItemRandomly(sounds).Play();
			}
		}


		private List<BitmapImage> LoadImages(string directory)
		{
			List<BitmapImage> imgs = new List<BitmapImage>();
			foreach (string filename in Directory.GetFiles(directory))
			{
				imgs.Add(new BitmapImage(new Uri("pack://application:,,," + filename)));
			}
			return imgs;
		}

		private List<SoundPlayer> LoadSounds(string directory)
		{
			List<SoundPlayer> sounds = new List<SoundPlayer>();
			foreach (string filename in Directory.GetFiles(directory))
			{
				sounds.Add(new SoundPlayer(filename));
			}
			return sounds;
		}

		private T GetItemRandomly<T>(List<T> items)
		{
			return items[rand.Next(items.Count)];
		}
	}
}
