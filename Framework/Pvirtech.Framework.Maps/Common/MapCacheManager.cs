 using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text; 
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pvirtech.Framework.Maps
{
	public class MapCacheManager
	{
		private object locker = new object();
		public string Cachepath
		{
			get;
			set;
		}
		public object Locker { get => locker; set => locker = value; }

		public MapCacheManager()
		{
		}

		public MapCacheManager(string cachepath)
		{
			this.Cachepath = cachepath;
		}

		public string GetImageFile(int level, int row, int col)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Cachepath);
			stringBuilder.Append(level);
			stringBuilder.Append("\\");
			stringBuilder.Append(row);
			stringBuilder.Append("\\");
			stringBuilder.Append(col);
			stringBuilder.Append(".png");
			return stringBuilder.ToString();
		}

		public ImageSource GetImage(int level, int row, int col)
		{
			string imageFile = this.GetImageFile(level, row, col);
			ImageSource result;
			if (File.Exists(imageFile))
			{
				Uri uriSource = new Uri(imageFile);
				ImageSource imageSource = new BitmapImage(uriSource);
				result = imageSource;
				//imageSource.Freeze();
			}
			else
			{
				result = null;
			}
			return result;
		}

		private byte[] GetNoTiledImage()
		{
			string path = "";
			byte[] result;
			if (File.Exists(path))
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					result = array;
					return result;
				}
			}
			result = null;
			return result;
		}

		public void SaveImage(Stream stream, int level, int row, int col)
		{
			string imageFile = this.GetImageFile(level, row, col);
			this.SaveImage(stream, imageFile);
		}

		public bool SaveImage(Stream stream, string imageFile)
		{
			bool result;
			if (stream == null)
			{
				result = false;
			}
			else
			{
				lock (Locker)
				{
					if (File.Exists(imageFile))
					{
						result = true;
						return result;
					}
					try
					{
						string directoryName = Path.GetDirectoryName(imageFile);
						string pathRoot = Path.GetPathRoot(directoryName);
						if (!Directory.Exists(pathRoot))
						{
							result = false;
							return result;
						}
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						using (Image image = Image.FromStream(stream))
						{
							image.Save(imageFile, ImageFormat.Png);
						}
					}
					catch (Exception e)
					{
						//LogManager.Instance.WriteLog(e, "MapCacheManager.SaveImage", LogLevel.Error);
						result = false;
						return result;
					}
				}
				result = true;
			}
			return result;
		}
	}
}
