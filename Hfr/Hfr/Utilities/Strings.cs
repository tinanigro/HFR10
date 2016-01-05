using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Hfr.Utilities
{
    public class Strings
    {
        public Strings()
        {

        }

        public static GridLength DefaultMargin => (GridLength) App.Current.Resources["DefaultMarginGridLength"];
        public static double MinimumWidth => (double) App.Current.Resources[nameof(MinimumWidth)];
        public static double PortraitWidth => (double)App.Current.Resources[nameof(PortraitWidth)];
        public static double NormalWidth => (double)App.Current.Resources[nameof(NormalWidth)]; 
        public static double WideWidth => (double) App.Current.Resources[nameof(WideWidth)];

        public static string WebSiteCacheFolderName = "website-cache";
        public static string WebSiteCacheFileName = "webpage.html";
        public static string TopicPageCache = $"ms-appdata:///local/{WebSiteCacheFolderName}/{WebSiteCacheFileName}";
        public static Uri TopicPageCacheUri = new Uri(TopicPageCache);

        public static string LocalUriPrefix = "http://local/";


        public static string First => "First";
        public static string Last => "Last";
    }
}
