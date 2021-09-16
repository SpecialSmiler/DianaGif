using System;
using System.Collections.Generic;
using System.Text;

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

		private ResourceManager()
		{

		}
	}
}
