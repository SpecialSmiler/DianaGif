using System;

namespace SSmiler.FpsConverter
{
	class Program
	{
		static void Main(string[] args)
		{

			//for (int i = 1; i <= 10; i++)
			//{
			//	for (int j = i; j <= 10; j++)
			//	{
			//		//Console.Write($"(InputFps:{j,3}, OutputFps:{i,3})");
			//		PrintArray(FpsConverter.GetStepsArrayFps(j, i));
			//	}
			//}
			PrintArray(FpsConverter.GetStepsArrayFps(10, 7), 10, 7);
			PrintArray(FpsConverter.GetStepsArrayFps(11, 7), 11, 7);
			PrintArray(FpsConverter.GetStepsArrayFps(12, 7), 12, 7);
		}

		static void PrintArray(int[] arr, int input = -1, int output = -1)
		{
			if(input >= 0 && output >= 0)
			{
				Console.Write($"(InputFps:{input,3}, OutputFps:{output,3})");
			}

			Console.Write("[ ");
			foreach (var a in arr)
			{
				Console.Write(a + " ");
			}
			Console.Write("]");
			Console.WriteLine();
		}
	}
}
