using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Hfr.Utilities
{
    public static class Strings
    {
        public static GridLength DefaultMargin => (GridLength) App.Current.Resources["DefaultMarginGridLength"];
    }
}
