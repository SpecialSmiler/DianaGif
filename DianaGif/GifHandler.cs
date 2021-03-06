using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using ImageMagick;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using SSmiler.FpsConverter;

namespace DianaGif
{
	class GifHandler
	{
		long fileSize = 0;
		int totalFrame = 0;
		int delay = 0;
		int colorCount = 0;
		int width = 0;
		int height = 0;
		FileInfo fi;
		private MagickImageCollection _collection;

		private Dictionary<(int, int), int[]> steps = new Dictionary<(int, int), int[]>()
		{
			//delay = 1 to (2,10)
			{(1,2), new int[]{ 2 } },
			{(1,3), new int[]{ 3 } },
			{(1,4), new int[]{ 4 } },
			{(1,5), new int[]{ 5 } },
			{(1,6), new int[]{ 6 } },
			{(1,7), new int[]{ 7 } },
			{(1,8), new int[]{ 8 } },
			{(1,9), new int[]{ 9 } },
			{(1,10), new int[]{ 10 } },
			//delay = 2 to (3,10)
			{(2,3), new int[]{ 1, 2 } },
			{(2,4), new int[]{ 2 } },
			{(2,5), new int[]{ 2, 3 } },
			{(2,6), new int[]{ 3 } },
			{(2,7), new int[]{ 3 , 4 } },
			{(2,8), new int[]{ 4 } },
			{(2,9), new int[]{ 4, 5 } },
			{(2,10), new int[] { 5 } },
			//delay = 3 to (4,10)
			{(3,4), new int[]{ 1, 1, 2} },
			{(3,5), new int[]{ 1, 2, 2} },
			{(3,6), new int[]{ 2 } },
			{(3,7), new int[]{ 2, 2, 3} },
			{(3,8), new int[]{ 2, 3, 3} },
			{(3,9), new int[]{ 3 } },
			{(3,10), new int[]{ 3, 3, 4 } },
			//delay = 4 to (5,10)
			{(4,5), new int[]{ 1, 1, 1, 2 } },
			{(4,6), new int[]{ 1, 2 } },
			{(4,7), new int[]{ 1, 2, 2, 2 } },
			{(4,8), new int[]{ 2 } },
			{(4,9), new int[]{ 2, 2, 2, 3 } },
			{(4,10), new int[]{ 2, 3 } },
			//delay = 5 to (6,10)
			{(5,6), new int[]{ 1, 1, 1, 1, 2 } },
			{(5,7), new int[]{ 1, 1, 2, 1, 2 } },
			{(5,8), new int[]{ 1, 2, 1, 2, 2 } },
			{(5,9), new int[]{ 1, 2, 2, 2, 2 } },
			{(5,10), new int[]{ 2 } },
			//delay = 6 to (7,10)
			{(6,7), new int[]{ 1, 1, 1, 1, 1, 2 } },
			{(6,8), new int[]{ 1, 1, 2, 1, 1, 2 } },
			{(6,9), new int[]{ 1, 2 } },
			{(6,10), new int[]{ 1, 2, 2 } },
			//delay = 7 to (8,10)
			{(7,8), new int[]{ 1, 1, 1, 1, 1, 1, 2 } },
			{(7,9), new int[]{ 1, 1, 1, 2, 1, 1, 2 } },
			{(7,10), new int[]{ 1, 1, 2, 1, 2, 1, 2 } },
			//delay = 8 to (9,10)
			{(8,9), new int[]{ 1, 1, 1, 1, 1, 1, 1, 2 } },
			{(8,10), new int[]{ 1, 1, 1, 2} },
			//delay = 9 to 10
			{(9,10), new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 2 } }
		};

		public int Delay { get => delay;}
		public int Width { get => width; }
		public int Height { get => height; }


		public MagickImageCollection Collection
		{
			get => _collection;
		}

		public void OpenGif(string gifFilePath)
		{
			//用微软的GifBitmapDecoder
			//Stream imageStreamSource = new FileStream(gifPath, FileMode.Open, FileAccess.Read, FileShare.Read);
			//decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

			//用Magick.Net
			fi = new FileInfo(gifFilePath);
			fileSize = fi.Length;

			if(!fi.Extension.ToLower().Equals(".gif"))//如果打开的不是gif文件就不要往下解析了
			{
				return;
			}

			_collection = new MagickImageCollection(gifFilePath);
			_collection.Coalesce();
			totalFrame = _collection.Count;
			if(totalFrame > 0)
			{
				colorCount = _collection[0].ColormapSize;
				width = _collection[0].Width;
				height = _collection[0].Height;
				delay = _collection[0].AnimationDelay;
			}
		}

		public string GifInfo()
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append($"File size: {FormatBytes(fileSize)}\n");
			if (fi.Extension.ToLower().Equals(".gif"))
			{
				sb.Append($"Total frame: {totalFrame}\n");
				sb.Append($"AnimationDelay：{delay}\n");
				sb.Append($"Fps：{(100f/delay).ToString("0.00")}\n");
				sb.Append($"Color: {colorCount}\n");
				sb.Append($"Width: {width}\n");
				sb.Append($"Height: {height}\n");
			}
			else
			{
				sb.Append("打开的不是gif文件\n");
			}

			return sb.ToString();
		}

		//public void CompressGif(int outputDelay, string outputFileName)
		//{
		//	using (Stream imageStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Write))
		//	{
		//		int curDelay = delay;
		//		MagickImageCollection dstImages = DelayConverter(curDelay, outputDelay);
		//		dstImages.Write(imageStream);
		//	}
		//}

		public void CompressGif(int outputDelay, string outputFileName, int width = 0, int height = 0)
		{
			using (Stream imageStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Write))
			{
				int inputDelay = delay;
				MagickImageCollection outputImages = DelayConverter(inputDelay, outputDelay);
				if (width > 0 || height > 0)
				{
					foreach (var image in outputImages)
					{
						image.Resize(width, height);
					}
				}
				outputImages.Optimize();
				outputImages.Write(imageStream);
			}
		}

		public void SeperateAndSaveGif(string dstPath)
		{
			if(dstPath.Substring(dstPath.Length-4, 4).ToLower().Equals(".png"))
			{
				dstPath = dstPath.Substring(0, dstPath.Length - 4);
			}
			StringBuilder sb = new StringBuilder();
			sb.Capacity = 200;
			sb.Append(dstPath);
			
			int originLength = sb.Length;
			for (int i = 0; i < Collection.Count; i++)
			{
				sb.AppendFormat($"_{i}.png");
				Collection[i].Write(sb.ToString());
				sb.Remove(originLength, sb.Length - originLength);
			}
		}

		private MagickImageCollection DelayConverter(int inputDelay, int outputDelay)
		{
			if(outputDelay > inputDelay)//若帧率降低，为了保证速度不变，需要抽帧
			{
				//float ratio = (float)dstDelay / curDelay;
				int[] steps = FpsConverter.GetStepsArrayGifDelay(inputDelay, outputDelay);

				MagickImageCollection images = new MagickImageCollection();
				int n = Collection.Count;
				int i = 0;
				int tablePos = 0;
				while(i < n)
				{
					MagickImage image = new MagickImage(Collection[i]);
					image.AnimationDelay = outputDelay;
					images.Add(image);
					i += steps[tablePos];
					tablePos = (tablePos + 1) % steps.Length;
				}

				foreach (var image in images)
				{
					image.AnimationDelay = outputDelay;
				}

				return images;
			}
			else//若不需要抽帧
			{
				MagickImageCollection tempImages = new MagickImageCollection();
				foreach(var img in Collection)
				{
					tempImages.Add(new MagickImage(img));
				}
				return tempImages;
			}
		}
		private string FormatBytes(long bytes)
		{
			string[] magnitudes = new string[] { "GB", "MB", "KB", "Bytes" };
			long max = (long)Math.Pow(1024, magnitudes.Length);
			return string.Format("{1:##.##} {0}",
				magnitudes.FirstOrDefault(
					magnitude => bytes > (max /= 1024)) ?? "0 Bytes",
				(decimal)bytes / (decimal)max);
		}
	}


}
