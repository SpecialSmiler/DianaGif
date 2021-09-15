using System;

namespace SSmiler.FpsConverter
{
	class Program
	{
		static void Main(string[] args)
		{

			for (int i = 1; i <= 10; i++)
			{
				for (int j = i; j <= 10; j++)
				{
					Console.Write($"({j},{i}): ");
					PrintArray(FpsConverter.GetStepsArrayFps(j, i));
				}
			}

			//PrintArray(fpsConverter.GetStepsArrayFps(6, 3));
			//PrintArray(fpsConverter.GetStepsArrayFps(10, 9));
			//PrintArray(fpsConverter.GetStepsArrayFps(7, 4));
			//PrintArray(fpsConverter.GetStepsArrayFps(3, 2));
		}

		static void PrintArray(int[] arr)
		{
			foreach (var a in arr)
			{
				Console.Write(a + " ");
			}
			Console.WriteLine();
		}
	}
}
