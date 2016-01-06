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
                return "ms-appx:///Assets/HTML/UI/no_avatar.png";
            }

            return "http://forum-images.hardware.fr/images/mesdiscussions-" + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
