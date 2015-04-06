using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hfr.Helpers
{
    public static class AppViewHelper
    {
        public static void SetAppView()
        {
            var v = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            v.TitleBar.BackgroundColor = (Color)App.Current.Resources["DarkTitleBar"];
            v.TitleBar.ForegroundColor = Colors.WhiteSmoke;

            v.TitleBar.ButtonBackgroundColor = (Color)App.Current.Resources["DarkTitleBar"];
            v.TitleBar.ButtonForegroundColor = Colors.WhiteSmoke;
            v.TitleBar.InactiveBackgroundColor = (Color)App.Current.Resources["InactiveDarkTitleBar"];
            v.TitleBar.ButtonInactiveBackgroundColor = (Color)App.Current.Resources["InactiveDarkTitleBar"];
            v.TitleBar.ButtonInactiveForegroundColor = Colors.WhiteSmoke;
            v.TitleBar.InactiveForegroundColor = Colors.WhiteSmoke;
        }
    }
}