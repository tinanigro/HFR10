using System;
using Windows.UI.Xaml.Data;

namespace Hfr.Converters
{
    public class AvatarIdToUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                // No avatar
                return "http://cache1.asset-cache.net/xc/483691632.jpg?v=2&c=IWSAsset&k=2&d=EMp_JPhMen2_gYwlQVvHyNzU4vF1kgQTl-RoD3x2Acp8nN6YliR4PMzRCzld1npg0";
            }
            return "http://forum-images.hardware.fr/images/mesdiscussions-" + value + ".jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
