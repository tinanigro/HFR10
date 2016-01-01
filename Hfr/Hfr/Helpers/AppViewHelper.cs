using Windows.UI;

namespace Hfr.Helpers
{
    public static class AppViewHelper
    {
        public static void SetAppView()
        {
            var v = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
        }
    }
}