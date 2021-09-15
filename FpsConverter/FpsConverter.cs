using System;
using System.Collections.Generic;
using System.Text;

namespace FpsConverter
{
	class FpsConverter
	{
		static int[] GetStepsArrayFps(int inputFps, int outputFps)
		{
			//对inputFps和outputFps进行约分

			
			int partsCount = (inputFps - outputFps) % outputFps;
			int[][] tempArr = new int[partsCount][];




			int[] steps = new int[3];
			return steps;
		}


		static int[] GetStepsArrayGifDelay(int inputDelay, int outputDelay)
		{
			//inputFps和outputFps

			return GetStepsArrayFps(outputDelay, inputDelay);
		}
	}
}
