using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pvirtech.Framework.Map
{
	public class FunGeometry
	{
		public static bool IsInSiChuanXY(double x, double y)
		{
			return x >= 102.5 && x <= 105.0 && y >= 30.0 && y <= 31.5;
		}

		public static bool PointInPolygon(MapPoint point, Polygon polygon)
		{
			PointCollection pointCollection = polygon.Rings[0];
			List<MapPoint> list = new List<MapPoint>();
			for (int i = 0; i < pointCollection.Count - 1; i++)
			{
				MapPoint mapPoint = pointCollection[i];
				list.Add(new MapPoint(mapPoint.X, mapPoint.Y));
			}
			return FunGeometry.PointInPolygon(point, list);
		}

		public static bool PointInPolygon(MapPoint point, List<MapPoint> polygonPoints)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < polygonPoints.Count; i++)
			{
				num = ((i == polygonPoints.Count - 1) ? 0 : (num + 1));
				if (polygonPoints[i].Y != polygonPoints[num].Y && ((point.Y >= polygonPoints[i].Y && point.Y < polygonPoints[num].Y) || (point.Y >= polygonPoints[num].Y && point.Y < polygonPoints[i].Y)) && point.X < (polygonPoints[num].X - polygonPoints[i].X) * (point.Y - polygonPoints[i].Y) / (polygonPoints[num].Y - polygonPoints[i].Y) + polygonPoints[i].X)
				{
					num2++;
				}
			}
			return num2 % 2 > 0;
		}

		public static Envelope GetEnvelope(double x, double y, double width)
		{
			double x2 = x - width / 2.0;
			double x3 = x + width / 2.0;
			double y2 = y - width / 2.0;
			double y3 = y + width / 2.0;
			return new Envelope(x2, y2, x3, y3);
		}

		public static List<MapPoint> GetMapPoints(Polyline polyline)
		{
			List<MapPoint> list = new List<MapPoint>();
			ObservableCollection<PointCollection> paths = polyline.Paths;
			for (int i = 0; i < paths.Count; i++)
			{
				PointCollection pointCollection = paths[i];
				for (int j = 0; j < pointCollection.Count; j++)
				{
					list.Add(pointCollection[j]);
				}
			}
			return list;
		}

		public static List<MapPoint> GetMapPoints(Polygon polygon)
		{
			List<MapPoint> list = new List<MapPoint>();
			ObservableCollection<PointCollection> rings = polygon.Rings;
			for (int i = 0; i < rings.Count; i++)
			{
				PointCollection pointCollection = rings[i];
				for (int j = 0; j < pointCollection.Count; j++)
				{
					list.Add(pointCollection[j]);
				}
			}
			return list;
		}

		public static MapPoint GetGravityCenter(Polygon polygon)
		{
			Envelope extent = polygon.Extent;
			MapPoint mapPoint = new MapPoint(extent.XMin, extent.YMin);
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			for (int i = 0; i < polygon.Rings.Count; i++)
			{
				ObservableCollection<PointCollection> rings = polygon.Rings;
				for (int j = 0; j < rings[0].Count; j++)
				{
					MapPoint mapPoint2 = rings[0][j];
					MapPoint mapPoint3;
					if (j == rings[0].Count - 1)
					{
						mapPoint3 = rings[0][0];
					}
					else
					{
						mapPoint3 = rings[0][j + 1];
					}
					double num4 = (mapPoint2.X - mapPoint.X) * (mapPoint3.Y - mapPoint2.Y) - (mapPoint2.X - mapPoint.X) * (mapPoint.Y - mapPoint2.Y) / 2.0 - (mapPoint3.X - mapPoint.X) * (mapPoint3.Y - mapPoint.Y) / 2.0 - (mapPoint2.X - mapPoint3.X) * (mapPoint3.Y - mapPoint2.Y) / 2.0;
					num3 += num4;
					MapPoint mapPoint4 = new MapPoint((mapPoint2.X + mapPoint3.X) / 2.0, (mapPoint2.Y + mapPoint3.Y) / 2.0);
					double num5 = mapPoint.X + (mapPoint4.X - mapPoint.X) * 2.0 / 3.0;
					double num6 = mapPoint.Y + (mapPoint4.Y - mapPoint.Y) * 2.0 / 3.0;
					num += num5 * num4;
					num2 += num6 * num4;
				}
			}
			return new MapPoint(num / num3, num2 / num3);
		}

		public static double CalPointToPointLength(double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt(Math.Pow(x1 - x2, 2.0) + Math.Pow(y1 - y2, 2.0));
		}

		public static double CalPointToPointSlope(double x1, double y1, double x2, double y2)
		{
			return (y2 - y1) / (x2 - x1);
		}

		public static MapPoint GetCenterPoint(Geometry geo)
		{
			MapPoint result;
			if (geo is Polygon)
			{
				result = FunGeometry.GetGravityCenter(geo as Polygon);
			}
			else if (geo is MapPoint)
			{
				result = (geo as MapPoint);
			}
			else
			{
				result = geo.Extent.GetCenter();
			}
			return result;
		}

		public static Envelope CreateEnvelop(MapPoint point, double pixles, double resolution)
		{
			double num = pixles / 2.0 * resolution;
			double x = point.X - num;
			double y = point.Y - num;
			double x2 = point.X + num;
			double y2 = point.Y + num;
			return new Envelope(x, y, x2, y2)
			{
				SpatialReference = point.SpatialReference
			};
		}

		public static Envelope GetEnvelope(FeatureSet featureSet)
		{
			Envelope result;
			if (featureSet != null && featureSet.Features.Count > 0)
			{
				result = FunGeometry.GetEnvelope(featureSet.Features);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static Envelope GetEnvelope(IEnumerable<Graphic> graphics)
		{
			Envelope envelope = null;
			foreach (Graphic current in graphics)
			{
				if (current.Geometry != null)
				{
					if (envelope != null)
					{
						envelope = envelope.Union(current.Geometry.Extent);
					}
					else
					{
						envelope = current.Geometry.Extent;
					}
				}
			}
			return envelope;
		}

		public static Polygon GetCellShape(double dblOriginX, double dblOriginY, double dblRadius, double dblStartAngle, double dblEndAngle, double stepAngle = 1.0)
		{
			dblStartAngle = FunCoordinateConvert.GetRadian(dblStartAngle);
			dblEndAngle = FunCoordinateConvert.GetRadian(dblEndAngle);
			stepAngle = FunCoordinateConvert.GetRadian(stepAngle);
			dblStartAngle = ((dblEndAngle > dblStartAngle) ? dblStartAngle : (dblStartAngle - 6.2831853071795862));
			Polygon polygon = new Polygon();
			PointCollection pointCollection = new PointCollection();
			pointCollection.Add(new MapPoint(dblOriginX, dblOriginY));
			int num = Convert.ToInt32(Math.Ceiling((dblEndAngle - dblStartAngle) / stepAngle));
			for (int i = 0; i <= num; i++)
			{
				double num2 = dblStartAngle + (double)i * stepAngle;
				double x = dblOriginX + dblRadius * Math.Cos(1.5707963267948966 - num2);
				double y = dblOriginY + dblRadius * Math.Sin(1.5707963267948966 - num2);
				pointCollection.Add(new MapPoint(x, y));
			}
			pointCollection.Add(new MapPoint(dblOriginX, dblOriginY));
			polygon.Rings.Add(pointCollection);
			return polygon;
		}

		public static PointCollection GetLine(double start_x, double start_y, double end_x, double end_y, double step_length)
		{
			PointCollection pointCollection = new PointCollection();
			PointCollection result;
			try
			{
				pointCollection.Add(new MapPoint(start_x, start_y));
				double num = FunGeometry.CalPointToPointLength(start_x, start_y, end_x, end_y);
				if (num <= step_length)
				{
					pointCollection.Add(new MapPoint(end_x, end_y));
					result = pointCollection;
					return result;
				}
				double angle = FunGeometry.GetAngle(start_x, start_y, end_x, end_y);
				for (double num2 = step_length; num2 < num; num2 += step_length)
				{
					double x = start_x + num2 * Math.Cos(angle);
					double y = start_y + num2 * Math.Sin(angle);
					pointCollection.Add(new MapPoint(x, y));
				}
				pointCollection.Add(new MapPoint(end_x, end_y));
			}
			catch
			{
			}
			result = pointCollection;
			return result;
		}

		public static double GetAngle(double start_x, double start_y, double end_x, double end_y)
		{
			double num = end_x - start_x;
			double num2 = end_y - start_y;
			double result;
			if (num2 == 0.0)
			{
				result = 0.0;
			}
			else if (num == 0.0)
			{
				result = 1.5707963267948966;
			}
			else
			{
				double d = FunGeometry.CalPointToPointSlope(start_x, start_y, end_x, end_y);
				double num3 = Math.Atan(d);
				if (num <= 0.0 || num2 <= 0.0)
				{
					if (num < 0.0 && num2 > 0.0)
					{
						num3 = 3.1415926535897931 + num3;
					}
					else if (num < 0.0 && num2 < 0.0)
					{
						num3 += 3.1415926535897931;
					}
					else if (num > 0.0 && num2 < 0.0)
					{
						num3 += 6.2831853071795862;
					}
				}
				result = num3;
			}
			return result;
		}
	}
}
