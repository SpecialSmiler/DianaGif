using System;
using System.Collections.Generic;
using System.Text;

namespace SSmiler.FpsConverter
{
	public class FpsConverter
	{
		static public int[] GetStepsArrayFps(int inputFps, int outputFps)
		{
			if(inputFps == outputFps)
			{
				return new int[1] { 1 };
			}

			int i;
			int j;
			//对inputFps和outputFps进行约分
			int gdc = GCD(inputFps, outputFps);
			inputFps /= gdc;
			outputFps /= gdc;

			int partsCount = (inputFps - outputFps) % outputFps;
			if(partsCount == 0)
			{
				return new int[1] { inputFps };
			}

			int[][] tempArr = new int[partsCount][];

			int[] subLen = new int[partsCount];
			int x = outputFps / partsCount;	//subLen中每个元素的初值
			int remain = outputFps % partsCount;
			
			for(i = 0; i <subLen.Length;i++)
			{
				subLen[i] = x;
			}

			i = 0;
			while(remain > 0)
			{
				subLen[i]++;
				remain--;
				i = (i + 1) % partsCount;
			}

			x = inputFps / outputFps;
			remain = inputFps % outputFps;
			for(i = 0; i < tempArr.Length; i++)
			{
				tempArr[i] = new int[subLen[i]];
				for(j = 0; j < tempArr[i].Length; j++)
				{
					tempArr[i][j] = x;
				}
			}

			
			i = tempArr.Length - 1;
			j = 0;
			while (remain > 0)
			{
				int col = tempArr[i].Length - 1 - j;
				tempArr[i][col]++;
				remain--;
				if(i==0)
				{
					j--;
					i = tempArr.Length - 1;
				}
				else
				{
					i--;
				}
			}


			int[] steps = new int[outputFps];
			j = 0;
			for(i = 0; i < tempArr.Length; i++)
			{
				tempArr[i].CopyTo(steps, j);
				j += tempArr[i].Length;
			}
			return steps;
		}

		static public int[] GetStepsArrayGifDelay(int inputDelay, int outputDelay)
		{
			return GetStepsArrayFps(outputDelay, inputDelay);
		}

		//辗转相除法，求最大公约数
		static private int GCD(int a, int b)
		{
			int temp;
			while (b >0)
			{
				/*利用辗除法，直到b为0为止*/
				temp = b;
				b = a % b;
				a = temp;
			}
			return a;
		}
	}
}
