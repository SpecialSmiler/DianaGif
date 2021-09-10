using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;

namespace DianaGif
{
	class GifHandler
	{
		public GifBitmapDecoder decoder;
		public void OpenGif(string gifPath)
		{
			Stream imageStreamSource = new FileStream(gifPath, FileMode.Open, FileAccess.Read, FileShare.Read);
			//用微软的GifBitmapDecoder
			decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

			//用Magick.Net


		}

		public string GifInfo()
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append($"总帧数：{decoder.Frames.Count}\n");
			sb.Append($"帧率：\n");
			sb.Append($"颜色：\n");
			return sb.ToString();
		}


	}
}
