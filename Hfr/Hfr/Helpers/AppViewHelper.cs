using System;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Hfr.Helpers
{
    public static class AppViewHelper
    {
        public static void SetAppView()
        {
            DisplayInformation.GetForCurrentView().OrientationChanged += AppViewHelper_OrientationChanged;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = (Color?)App.Current.Resources["SystemAccentColor"];
                statusBar.BackgroundOpacity = 1;
            }
            else
            {
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;

                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonForegroundColor = Windows.UI.Colors.Black;
                titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;

            }
        }

        private static async void AppViewHelper_OrientationChanged(DisplayInformation sender, object args)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (sender.CurrentOrientation == DisplayOrientations.Landscape ||
                    sender.CurrentOrientation == DisplayOrientations.LandscapeFlipped)
                {
                    await statusBar.HideAsync();
                }
                else
                {
                    await statusBar.ShowAsync();
                }
            }
        }
    }
}