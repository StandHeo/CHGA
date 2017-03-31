using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
namespace Pvirtech.Framework.Maps
{
	public class PvirtechMap
	{
		public static void ZoomToCenter(ESRI.ArcGIS.Client.Map map, double x, double y)
		{
			map.PanTo(new MapPoint(x, y)
			{
				SpatialReference = map.SpatialReference
			});
		}

		public static void ZoomToDistance(ESRI.ArcGIS.Client.Map map, double width)
		{
			Envelope extent = map.Extent;
			double factor = width / extent.Width;
			Envelope extent2 = extent.Expand(factor);
			map.Extent = extent2;
		}

		public static void ZoomToCentreAndWidth(ESRI.ArcGIS.Client.Map map, double x, double y, double width)
		{
			map.Extent = MapGeometry.GetEnvelope(x, y, width);
		}

		public static void LocateMapByScale(ESRI.ArcGIS.Client.Map map, double x, double y, double scale)
		{
			double mapResolution = GetMapResolution(map, scale);
			map.ZoomToResolution(mapResolution);
		}

		public static Envelope LocateMap(ESRI.ArcGIS.Client.Map map, IEnumerable<Graphic> graphics)
		{
			Envelope envelope = MapGeometry.GetEnvelope(graphics);
			if (envelope != null && envelope.Width > 0.0 && envelope.Height > 0.0)
			{
				map.Extent = envelope;
			}
			return envelope;
		}

		public static void LocateMap(ESRI.ArcGIS.Client.Map map, IEnumerable<Point> points)
		{
			if (points == null || points.Count<Point>() == 0)
			{
				throw new Exception("points不能为空");
			}
			if (points.Count<Point>() == 1)
			{
				MapPoint geometry = new MapPoint(points.FirstOrDefault<Point>().X, points.FirstOrDefault<Point>().Y);
				map.PanTo(geometry);
			}
			else if (points.Count<Point>() == 2)
			{
				double x = (points.ElementAt(0).X + points.ElementAt(1).X) / 2.0;
				double y = (points.ElementAt(0).Y + points.ElementAt(1).Y) / 2.0;
				map.PanTo(new MapPoint(x, y));
			}
			else
			{
				MultiPoint multiPoint = new MultiPoint();
				foreach (Point current in points)
				{
					multiPoint.Points.Add(new MapPoint(current.X, current.Y));
				}
				map.Extent = multiPoint.Extent.Expand(1.1);
			}
		}

		public static MapPoint GetPointsCenterPoint(IEnumerable<MapPoint> points)
		{
			if (points == null || points.Count<MapPoint>() == 0)
			{
				throw new Exception("points不能为空");
			}
			MapPoint result;
			if (points.Count<MapPoint>() == 1)
			{
				result = new MapPoint(points.FirstOrDefault<MapPoint>().X, points.FirstOrDefault<MapPoint>().Y);
			}
			else if (points.Count<MapPoint>() == 2)
			{
				double x = (points.ElementAt(0).X + points.ElementAt(1).X) / 2.0;
				double y = (points.ElementAt(0).Y + points.ElementAt(1).Y) / 2.0;
				result = new MapPoint(x, y);
			}
			else
			{
				MultiPoint multiPoint = new MultiPoint();
				foreach (MapPoint current in points)
				{
					multiPoint.Points.Add(new MapPoint(current.X, current.Y));
				}
				result = multiPoint.Extent.GetCenter();
			}
			return result;
		}

		public static void RefreshMap(ESRI.ArcGIS.Client.Map map)
		{
			foreach (Layer current in map.Layers)
			{
				if (current is ArcGISDynamicMapServiceLayer)
				{
					(current as ArcGISDynamicMapServiceLayer).Refresh();
				}
				else if (current is ArcGISTiledMapServiceLayer)
				{
					(current as ArcGISTiledMapServiceLayer).Refresh();
				}
				if (current is GraphicsLayer)
				{
					(current as GraphicsLayer).Refresh();
				}
			}
		}

		public static double GetMapResolution(ESRI.ArcGIS.Client.Map map, double scale)
		{
			return scale / (map.Scale / map.Resolution);
		}
	}
}
