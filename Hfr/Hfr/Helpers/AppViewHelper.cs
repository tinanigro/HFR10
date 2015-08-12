using Windows.UI;

namespace Hfr.Helpers
{
    public static class AppViewHelper
    {
        public static void SetAppView()
        {
            var v = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            //v.TitleBar.ExtendViewIntoTitleBar = true;

            v.TitleBar.ButtonBackgroundColor = (Color)App.Current.Resources["MainColor"];
            v.TitleBar.ButtonForegroundColor = Colors.WhiteSmoke;
            v.TitleBar.InactiveBackgroundColor = (Color)App.Current.Resources["MainColor"];
            v.TitleBar.ButtonInactiveBackgroundColor = (Color)App.Current.Resources["InactiveDarkTitleBar"];
            v.TitleBar.ButtonInactiveForegroundColor = Colors.WhiteSmoke;
            v.TitleBar.InactiveForegroundColor = Colors.WhiteSmoke;
        }
    }
}