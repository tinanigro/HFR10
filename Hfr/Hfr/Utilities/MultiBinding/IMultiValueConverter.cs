using System;

namespace Huyn.MultiBinding
{
    public interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, String language);
        object[] ConvertBack(object value, Type[] targetTypes, object parameter, String language);
    }
}
