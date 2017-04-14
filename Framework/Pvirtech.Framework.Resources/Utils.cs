using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pvirtech.Framework.Resources
{
    public static class Utils
    {
        public static BitmapImage GetBitmapImage(string imgPath)
        {
            string path = string.Format("pack://application:,,,/Pvirtech.Framework.Resources;component/Images/{0}", imgPath);
            BitmapImage bImg = new BitmapImage();
            bImg.BeginInit();
            bImg.CacheOption = BitmapCacheOption.OnLoad;
            bImg.UriSource = new Uri(path);
            bImg.EndInit();
            bImg.Freeze(); 
            return bImg;
        }
        public static Uri GetImageUrl(string imgPath)
        {
            string path = string.Format("pack://application:,,,/Pvirtech.Framework.Resources;component/Images/{0}", imgPath);
            return new Uri(path, UriKind.RelativeOrAbsolute);
        }
        public static string GetImageRelativePath(string imgPath)
        {
            string path = string.Format("pack://application:,,,/Pvirtech.Framework.Resources;component/{0}", imgPath);
            return  path;
        }
    }
}
