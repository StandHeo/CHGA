using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;

namespace Pvirtech.Framework.Map
{
	public class FunColor
	{
		public static List<Color> GetRandomColors(int count)
		{
			List<Color> list = new List<Color>();
			for (int i = 0; i < count; i++)
			{
				list.Add(FunColor.GetRandomColor(i));
			}
			return list;
		}

		public static Color GetRandomColor()
		{
			Random random = new Random();
			Random random2 = new Random((int)DateTime.Now.Ticks);
			Thread.Sleep(random2.Next(50));
			Random random3 = new Random((int)DateTime.Now.Ticks);
			int num = random2.Next(256);
			int num2 = random3.Next(256);
			int num3 = (num + num2 > 400) ? 0 : (400 - num - num2);
			num3 = ((num3 > 255) ? 255 : num3);
			return Color.FromArgb(255, (byte)num, (byte)num2, (byte)num3);
		}

		public static Color GetRandomColor(int index)
		{
			Random random = new Random((int)DateTime.Now.Ticks * (index + 1));
			int num = random.Next(256);
			int num2 = random.Next(256);
			int num3 = (num + num2 > 400) ? 0 : (400 - num - num2);
			num3 = ((num3 > 255) ? 255 : num3);
			return Color.FromArgb(255, (byte)num, (byte)num3, (byte)num2);
		}

		public static List<Color> GetStretchColors(int count, Color colorFrom, Color colorTo)
		{
			List<Color> list = new List<Color>();
			for (int i = 0; i < count; i++)
			{
				int num = (int)colorFrom.R + i * (int)(colorTo.R - colorFrom.R) / (count - 1);
				int num2 = (int)colorFrom.G + i * (int)(colorTo.G - colorFrom.G) / (count - 1);
				int num3 = (int)colorFrom.B + i * (int)(colorTo.B - colorFrom.B) / (count - 1);
				Color item = Color.FromArgb(colorFrom.A, (byte)num, (byte)num2, (byte)num3);
				list.Add(item);
			}
			return list;
		}

		public static List<Color> GetColors(Color colorMin, Color colorMax, int intBreakCount)
		{
			List<Color> result;
			if (colorMax == colorMin)
			{
				result = null;
			}
			else
			{
				List<Color> list = new List<Color>();
				if (intBreakCount == 1)
				{
					list.Add(colorMin);
					result = list;
				}
				else
				{
					for (int i = 0; i < intBreakCount; i++)
					{
						int num = (int)colorMin.R + i * (int)(colorMax.R - colorMin.R) / (intBreakCount - 1);
						int num2 = (int)colorMin.G + i * (int)(colorMax.G - colorMin.G) / (intBreakCount - 1);
						int num3 = (int)colorMin.B + i * (int)(colorMax.B - colorMin.B) / (intBreakCount - 1);
						Color item = Color.FromArgb(255, (byte)num, (byte)num2, (byte)num3);
						list.Add(item);
					}
					result = list;
				}
			}
			return result;
		}
	}
}
