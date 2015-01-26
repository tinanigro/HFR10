using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace HFR4WinRT.Converters
{
    public class AvatarIdToUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "http://forum-images.hardware.fr/images/mesdiscussions-" + value + ".jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
