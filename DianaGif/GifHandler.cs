﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using ImageMagick;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DianaGif
{
	class GifHandler
	{
		long fileSize = 0;
		int totalFrame = 0;
		int delay = 0;
		float fps = 0f;
		int colorCount = 0;
		int width = 0;
		int height = 0;
		FileInfo fi;
		//List<(Bitmap image, int delay)> images;
		public MagickImageCollection collection;

		int[] ratio2to1 = { 2 };
		int[] ratio4to1 = { 4 };
		int[] ratio5to1 = { 5 };
		int[] ratio5to2 = { 2, 3 };
		int[] ratio5to4 = { 1, 1, 1, 2 };
		int[] ratio8to5 = { 2, 2, 1, 2, 1 };

		int progressValue = 0;

		//public GifBitmapDecoder decoder;
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

			//images = new List<(Bitmap image, int delay)>();
			collection = new MagickImageCollection(gifFilePath);
			collection.Coalesce();
			totalFrame = collection.Count;
			if(totalFrame > 0)
			{
				delay = collection[0].AnimationDelay;
				fps = 100f / delay;
				colorCount = collection[0].ColormapSize;
				width = collection[0].Width;
				height = collection[0].Height;
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
				sb.Append($"Fps：{fps.ToString("0.00")}\n");
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

		public bool CreateGif(int dstDelay, string dstFile, out string resStr)
		{
			resStr = "";
			if(collection.Count==0)
			{
				resStr = "图像序列为空！";
				return false;
			}
			Stream imageStream = new FileStream(dstFile, FileMode.Create, FileAccess.Write, FileShare.Write);

			// Resize each image in the collection to a width of 200. When zero is specified for the height
			// the height will be calculated with the aspect ratio.

			int curDelay = delay;

			MagickImageCollection dstImages = FpsConverter(curDelay, dstDelay);
			//dstImages.Write(imageStream);
			dstImages.WriteAsync(imageStream);

			return true;
		}

		private MagickImageCollection FpsConverter(int curDelay, int dstDelay)
		{
			if(curDelay == dstDelay)
			{
				return collection;
			}
			else if(dstDelay > curDelay)//若帧率降低，为了保证速度不变，需要抽帧
			{
				float ratio = (float)dstDelay / curDelay;
				int[] stepTable;
				if (Math.Abs(ratio - 2f) < 0.01f)
				{
					stepTable = ratio2to1;
				}
				else if(Math.Abs(ratio - 4f) < 0.01f)
				{
					stepTable = ratio4to1;
				}
				else if (Math.Abs(ratio - 5f) < 0.01f)
				{
					stepTable = ratio5to1;
				}
				else if (Math.Abs(ratio - 2.5f) < 0.01f)
				{
					stepTable = ratio5to2;
				}
				else if (Math.Abs(ratio - 1.25f) < 0.01f)
				{
					stepTable = ratio5to4;
				}
				else if (Math.Abs(ratio - 1.6f) < 0.01f)
				{
					stepTable = ratio8to5;
				}
				else
				{
					//其余情况不做处理
					return collection;
				}

				MagickImageCollection images = new MagickImageCollection();
				int n = collection.Count;
				int i = 0;
				int tablePos = 0;
				while(i < n)
				{
					MagickImage image = new MagickImage(collection[i]);
					image.AnimationDelay = dstDelay;
					images.Add(image);
					i += stepTable[tablePos];
					tablePos = (tablePos + 1) % stepTable.Length;
				}

				foreach (var image in images)
				{
					image.AnimationDelay = dstDelay;
				}

				return images;
			}
			else	//若Delay比原来小，最终结果是速度加快了，也可以加帧，但还是算了，因为没有意义
			{
				foreach(var image in collection)
				{
					image.AnimationDelay = dstDelay;
				}
				return collection;
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
