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
        public static double MinimumWidth => (double) App.Current.Resources[nameof(MinimumWidth)];
        public static double PortraitWidth => (double)App.Current.Resources[nameof(PortraitWidth)];
        public static double NormalWidth => (double)App.Current.Resources[nameof(NormalWidth)]; 
        public static double WideWidth => (double) App.Current.Resources[nameof(WideWidth)];
    }
}
