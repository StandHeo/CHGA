using ESRI.ArcGIS.Client.Geometry;
using System;

namespace Pvirtech.Framework.Maps
{
	public class MapCoordinateConvert
	{
		private const double pi = 3.1415926535897931;

		private const double a = 6378245.0;

		private const double ee = 0.0066934216229659433;

		public static MapPoint LonLatToMercator(double x, double y)
		{
			double x2 = x * 20037508.34 / 180.0;
			double num = Math.Log(Math.Tan((90.0 + y) * 3.1415926535897931 / 360.0)) / 0.017453292519943295;
			num = num * 20037508.34 / 180.0;
			return new MapPoint
			{
				X = x2,
				Y = num
			};
		}

		public static MapPoint MercatorTolonLat(double x, double y)
		{
			double x2 = x / 20037508.34 * 180.0;
			double num = y / 20037508.34 * 180.0;
			num = 57.295779513082323 * (2.0 * Math.Atan(Math.Exp(num * 3.1415926535897931 / 180.0)) - 1.5707963267948966);
			return new MapPoint
			{
				X = x2,
				Y = num
			};
		}

		public static double GetGeographyLength(double length)
		{
			return length * 1E-05;
		}

		public static double GetRadian(double angle)
		{
			return angle * 3.1415926535897931 / 180.0;
		}

		public static double GetFlatAngle(double angle)
		{
			return (360.0 - angle + 90.0) % 360.0;
		}

		public static void bd_encrypt(double gg_lat, double gg_lon, out double bd_lat, out double bd_lon)
		{
			double num = Math.Sqrt(gg_lon * gg_lon + gg_lat * gg_lat) + 2E-05 * Math.Sin(gg_lat * 3.1415926535897931);
			double d = Math.Atan2(gg_lat, gg_lon) + 3E-06 * Math.Cos(gg_lon * 3.1415926535897931);
			bd_lon = num * Math.Cos(d) + 0.0065;
			bd_lat = num * Math.Sin(d) + 0.006;
		}

		public static void bd_decrypt(double bd_lat, double bd_lon, out double gg_lat, out double gg_lon)
		{
			double num = bd_lon - 0.0065;
			double num2 = bd_lat - 0.006;
			double num3 = Math.Sqrt(num * num + num2 * num2) - 2E-05 * Math.Sin(num2 * 3.1415926535897931);
			double d = Math.Atan2(num2, num) - 3E-06 * Math.Cos(num * 3.1415926535897931);
			gg_lon = num3 * Math.Cos(d);
			gg_lat = num3 * Math.Sin(d);
		}

		public static void transform(double wgLon, double wgLat, out double mgLon, out double mgLat)
		{
			if (outOfChina(wgLat, wgLon))
			{
				mgLat = wgLat;
				mgLon = wgLon;
			}
			else
			{
				double num = transformLat(wgLon - 105.0, wgLat - 35.0);
				double num2 = transformLon(wgLon - 105.0, wgLat - 35.0);
				double d = wgLat / 180.0 * 3.1415926535897931;
				double num3 = Math.Sin(d);
				num3 = 1.0 - 0.0066934216229659433 * num3 * num3;
				double num4 = Math.Sqrt(num3);
				num = num * 180.0 / (6335552.7170004258 / (num3 * num4) * 3.1415926535897931);
				num2 = num2 * 180.0 / (6378245.0 / num4 * Math.Cos(d) * 3.1415926535897931);
				mgLat = wgLat + num;
				mgLon = wgLon + num2;
			}
		}

		private static bool outOfChina(double lat, double lon)
		{
			return lon < 72.004 || lon > 137.8347 || (lat < 0.8293 || lat > 55.8271);
		}

		private static double transformLat(double x, double y)
		{
			double num = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
			num += (20.0 * Math.Sin(6.0 * x * 3.1415926535897931) + 20.0 * Math.Sin(2.0 * x * 3.1415926535897931)) * 2.0 / 3.0;
			num += (20.0 * Math.Sin(y * 3.1415926535897931) + 40.0 * Math.Sin(y / 3.0 * 3.1415926535897931)) * 2.0 / 3.0;
			return num + (160.0 * Math.Sin(y / 12.0 * 3.1415926535897931) + 320.0 * Math.Sin(y * 3.1415926535897931 / 30.0)) * 2.0 / 3.0;
		}

		private static double transformLon(double x, double y)
		{
			double num = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
			num += (20.0 * Math.Sin(6.0 * x * 3.1415926535897931) + 20.0 * Math.Sin(2.0 * x * 3.1415926535897931)) * 2.0 / 3.0;
			num += (20.0 * Math.Sin(x * 3.1415926535897931) + 40.0 * Math.Sin(x / 3.0 * 3.1415926535897931)) * 2.0 / 3.0;
			return num + (150.0 * Math.Sin(x / 12.0 * 3.1415926535897931) + 300.0 * Math.Sin(x / 30.0 * 3.1415926535897931)) * 2.0 / 3.0;
		}
	}
}
