
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml;

namespace Pvirtech.Framework.Map
{
	public class EzMapTiledLayer : TiledMapServiceLayer
	{
		private TileInfo ezTileInfo = null;

		private string _url = "";

		private MapCacheManager mapCacheManager = new MapCacheManager();

		private string _Cachepath;

		private string _mapConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd("\\".ToCharArray()), "config\\MapConfig.xml");

		private int maxLevel;

		private int minLevel;

		private int maxValidLevel;

		private int minValidLevel;

		private int tilesize = 256;

		private double scale = 2.0;

		private bool IsTDT = false;

		private string userId;

		private string authCode;

		private int maptype;

		public string Cachepath
		{
			get
			{
				return this._Cachepath;
			}
			set
			{
				this._Cachepath = value;
				this.mapCacheManager.Cachepath = this._Cachepath;
			}
		}

		public string Url
		{
			get
			{
				return this._url;
			}
			set
			{
				this._url = value;
			}
		}

		public int MaxLevel
		{
			get
			{
				return this.maxLevel;
			}
			set
			{
				this.maxLevel = value;
			}
		}

		public int MinLevel
		{
			get
			{
				return this.minLevel;
			}
			set
			{
				this.minLevel = value;
			}
		}

		public int MaxValidLevel
		{
			get
			{
				return this.maxValidLevel;
			}
			set
			{
				this.maxValidLevel = value;
			}
		}

		public int MinValidLevel
		{
			get
			{
				return this.minValidLevel;
			}
			set
			{
				this.minValidLevel = value;
			}
		}

		public string UserId
		{
			get
			{
				return this.userId;
			}
			set
			{
				this.userId = value;
			}
		}

		public string AuthCode
		{
			get
			{
				return this.authCode;
			}
			set
			{
				this.authCode = value;
			}
		}

		public int Maptype
		{
			get
			{
				return this.maptype;
			}
			set
			{
				this.maptype = value;
			}
		}

		public EzMapTiledLayer(string mapServer_NodeName)
		{
			this.InitLayer(mapServer_NodeName);
			base.TileLoaded += new EventHandler<TiledLayer.TileLoadEventArgs>(this.EzMapTiledLayer_TileLoaded);
			this.mapCacheManager.Cachepath = this.Cachepath;
		}

		private void EzMapTiledLayer_TileLoaded(object sender, TiledLayer.TileLoadEventArgs e)
		{
			this.mapCacheManager.SaveImage(e.ImageStream, e.Level, e.Row, e.Column);
		}

		protected override void GetTileSource(int level, int row, int col, Action<ImageSource> onComplete)
		{
			ImageSource image = this.mapCacheManager.GetImage(level, row, col);
			if (image != null)
			{
				if (onComplete != null)
				{
					onComplete(image);
				}
			}
			else
			{
				base.GetTileSource(level, row, col, onComplete);
			}
		}

		public override void Initialize()
		{
			base.SpatialReference = new SpatialReference(4326);
			base.Initialize();
		}

		private TileInfo GetTileInfo(bool isEzMap)
		{
			double num;
			if (this.IsTDT)
			{
				num = 1.40625;
			}
			else
			{
				num = 2.0;
			}
			TileInfo tileInfo = new TileInfo();
			tileInfo.Height = this.tilesize;
			tileInfo.Width = this.tilesize;
			tileInfo.Lods = new Lod[this.maxLevel - this.minLevel + 1];
			if (isEzMap)
			{
				tileInfo.Origin = new MapPoint(0.0, 0.0)
				{
					SpatialReference = new SpatialReference(4326)
				};
			}
			else
			{
				tileInfo.Origin = new MapPoint(-180.0, 90.0);
				base.SpatialReference = new SpatialReference(4326);
			}
			int i = 0;
			int num2 = tileInfo.Lods.Length;
			while (i < num2)
			{
				tileInfo.Lods[i] = new Lod
				{
					Resolution = num
				};
				num /= 2.0;
				i++;
			}
			return tileInfo;
		}

		public override string GetTileUrl(int level, int row, int col)
		{
			string result;
			if (this.maptype == 1)
			{
				StringBuilder stringBuilder = new StringBuilder(this._url);
				stringBuilder.Append("?requestString=<?xml version='1.0' encoding='UTF-8'?><Request><UserId>");
				stringBuilder.Append(this.UserId);
				stringBuilder.Append("</UserId><AuthCode>");
				stringBuilder.Append(this.AuthCode);
				stringBuilder.Append("</AuthCode>");
				stringBuilder.Append("<Params><Param name='Service'>getImage</Param><Param name='Type'>RGB</Param><Param name='ZoomOffset'>0</Param><Param name='Col'>");
				stringBuilder.Append(col);
				stringBuilder.Append("</Param><Param name='Row'>");
				stringBuilder.Append(row);
				stringBuilder.Append("</Param><Param name='Zoom'>");
				stringBuilder.Append(level);
				stringBuilder.Append("</Param><Param name='V'>0.3</Param></Params></Request>");
				result = stringBuilder.ToString();
			}
			else if (this.IsTDT)
			{
				string text = string.Concat(new object[]
				{
					this._url,
					"?Service=getImage&Type=RGB&ZoomOffset=0&V=1.0.0&Col=",
					col,
					"&Row=",
					row,
					"&Zoom=",
					level
				});
				result = text;
			}
			else
			{
				Point point = this.colrow2lonlat(level, row, col);
				Point point2 = this.lonlat2rowcol(level, point.X, point.Y);
				string text = string.Concat(new object[]
				{
					this._url,
					"?Service=getImage&Type=RGB&Col=",
					point2.X,
					"&Row=",
					point2.Y,
					"&Zoom=",
					level
				});
				result = text;
			}
			return result;
		}

		public double LevelToResolution(int level)
		{
			return this.ezTileInfo.Lods[level].Resolution;
		}

		public int ResolutionToLevel(double resolution)
		{
			int num = -1;
			int i = 0;
			int num2 = this.ezTileInfo.Lods.Length;
			while (i < num2)
			{
				if (Math.Round(this.ezTileInfo.Lods[i].Resolution, 9) == Math.Round(resolution, 9))
				{
					num = i;
					break;
				}
				i++;
			}
			if (num == -1)
			{
				i = 0;
				num2 = this.ezTileInfo.Lods.Length;
				while (i < num2)
				{
					if (i + 1 < num2)
					{
						double num3 = Math.Round(resolution, 9);
						double num4 = Math.Round(this.ezTileInfo.Lods[i + 1].Resolution, 9);
						double num5 = Math.Round(this.ezTileInfo.Lods[i].Resolution, 9);
						if (num3 > num4 && num3 < num5)
						{
							if (Math.Abs(num3 - num4) < Math.Abs(num3 - num5))
							{
								num = i + 1;
							}
							else
							{
								num = i;
							}
							break;
						}
					}
					i++;
				}
			}
			return num;
		}

		public void InitLayer(string mapServer_NodeName)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this._mapConfigPath);
			XmlAttribute xmlAttribute = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName).Attributes["mapservertype"];
			string text = "0";
			if (xmlAttribute != null)
			{
				text = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName).Attributes["mapservertype"].InnerText;
			}
			string innerText = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/scale").InnerText;
			double.TryParse(innerText, out this.scale);
			string innerText2 = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/tilesize").InnerText;
			int.TryParse(innerText2, out this.tilesize);
			this.Cachepath = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/cachepath").InnerXml + "\\" + text + "\\";
			string innerXml = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/levelinfo").Attributes["maxvalid"].InnerXml;
			int num;
			int.TryParse(innerXml, out num);
			this.maxValidLevel = num;
			innerXml = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/levelinfo").Attributes["minvalid"].InnerXml;
			int.TryParse(innerXml, out num);
			this.minValidLevel = num;
			string innerXml2 = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/levelinfo").Attributes["max"].InnerXml;
			int num2;
			int.TryParse(innerXml2, out num2);
			this.maxLevel = num2;
			innerXml2 = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/levelinfo").Attributes["min"].InnerXml;
			int.TryParse(innerXml2, out num2);
			this.minLevel = num2;
			XmlElement xmlElement = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/fullextent") as XmlElement;
			double x = Convert.ToDouble(xmlElement.GetAttribute("xmin"));
			double y = Convert.ToDouble(xmlElement.GetAttribute("ymin"));
			double x2 = Convert.ToDouble(xmlElement.GetAttribute("xmax"));
			double y2 = Convert.ToDouble(xmlElement.GetAttribute("ymax"));
			this.FullExtent = new Envelope(x, y, x2, y2);
			if (text == "1")
			{
				this.IsTDT = true;
			}
			else
			{
				this.IsTDT = false;
			}
			this.userId = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/userid").InnerText;
			this.authCode = xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/authcode").InnerText;
			this.maptype = Convert.ToInt32(xmlDocument.SelectSingleNode("config/" + mapServer_NodeName + "/maptype").InnerText);
			base.TileInfo = this.GetTileInfo(false);
			this.ezTileInfo = this.GetTileInfo(true);
			base.MaximumResolution = this.LevelToResolution(this.minValidLevel) * 2.0;
			base.MinimumResolution = this.LevelToResolution(this.MaxValidLevel) / 2.0;
		}

		private Point colrow2lonlat(int level, int row, int col)
		{
			double num = (double)base.TileInfo.Width * base.TileInfo.Lods[level].Resolution * (double)col + base.TileInfo.Origin.X;
			double num2 = (double)(-(double)base.TileInfo.Height) * base.TileInfo.Lods[level].Resolution * (double)row + base.TileInfo.Origin.Y;
			double x = num + (double)(base.TileInfo.Width / 2) * base.TileInfo.Lods[level].Resolution;
			double y = num2 - (double)(base.TileInfo.Height / 2) * base.TileInfo.Lods[level].Resolution;
			return new Point(x, y);
		}

		private Point lonlat2rowcol(int level, double lon, double lat)
		{
			double x = Math.Floor((lon - this.ezTileInfo.Origin.X) / ((double)this.ezTileInfo.Width * this.ezTileInfo.Lods[level].Resolution));
			double y = Math.Floor((lat - this.ezTileInfo.Origin.Y) / ((double)this.ezTileInfo.Height * this.ezTileInfo.Lods[level].Resolution));
			return new Point(x, y);
		}
	}
}
