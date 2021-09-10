using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using ImageMagick;
using System.Drawing;

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
		MagickImageCollection collection;

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
			if(fileSize >= 1024 * 1024)
			{
				sb.Append($"File size: {((float)fileSize/(1024*1024)).ToString("0.00")} MB\n");
			}
			else if (fileSize >= 1024)
			{
				sb.Append($"File size: {((float)fileSize / 1024).ToString("0.00")} KB\n");
			}
			else
			{
				sb.Append($"File size: {fileSize} Bytes\n");
			}

			if(fi.Extension.ToLower().Equals(".gif"))
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

		public bool CreateGif(string dstFile, out string resStr)
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
			foreach (var image in collection)
			{
				image.Resize(200, 0);
			}

			collection.Write(imageStream);

			return true;
		}
	}
}
