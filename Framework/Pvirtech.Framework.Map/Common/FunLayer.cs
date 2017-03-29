using ESRI.ArcGIS.Client;
using System;
using System.Collections.Generic;

namespace FounderAMP.MapService
{
	public class FunLayer
	{
		public static void SetLayerVisbleScale(Map map, Layer layer, double maxScale, double minScale)
		{
			layer.MaximumResolution = FunMap.GetMapResolution(map, maxScale);
			layer.MinimumResolution = FunMap.GetMapResolution(map, minScale);
			layer.Visible = !layer.Visible;
			layer.Visible = !layer.Visible;
		}

		public static void MoveLayerTo(Map map, Layer layer, int index)
		{
			int num = map.Layers.IndexOf(layer);
			if (num != index && map.Layers.Count != 1)
			{
				if (index < 0)
				{
					if (num != 0)
					{
						map.Layers.RemoveAt(num);
						map.Layers.Insert(0, layer);
					}
				}
				else if (index >= map.Layers.Count)
				{
					if (num != map.Layers.Count - 1)
					{
						map.Layers.RemoveAt(num);
						map.Layers.Add(layer);
					}
				}
				else
				{
					map.Layers.RemoveAt(num);
					map.Layers.Insert(index, layer);
				}
			}
		}

		public static void MoveLayerExcursion(Map map, Layer layer, int moveOff)
		{
			if (moveOff != 0 && map.Layers.Count != 1)
			{
				int num = map.Layers.IndexOf(layer);
				int index = moveOff + num;
				FunLayer.MoveLayerTo(map, layer, index);
			}
		}

		public static List<string> GetLayerIDs(Map map)
		{
			List<string> list = new List<string>();
			foreach (Layer current in map.Layers)
			{
				list.Add(current.ID);
			}
			return list;
		}
	}
}
