using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Hfr.Helpers;

namespace Hfr.CustomControls
{
    public class ScrollLockListView : ListView
    {
        private double verticalOffset;
        public ScrollLockListView()
        {
            this.Loaded += ScrollLockListView_Loaded;
        }

        private void ScrollLockListView_Loaded(object sender, RoutedEventArgs e)
        {
            var sV = this.GetFirstDescendantOfType<ScrollViewer>();
            sV.ViewChanging += SV_ViewChanging;
            sV.ViewChanged += SV_ViewChanged;
        }

        private void SV_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var sV = (ScrollViewer)sender;
            if (verticalOffset > 0 && sV.VerticalOffset == 0)
            {
                // offset hasn't moved
                sV.ChangeView(null, verticalOffset, null, true);
            }
            verticalOffset = sV.VerticalOffset;
        }

        private void SV_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var sV = (ScrollViewer) sender;
            verticalOffset = sV.VerticalOffset;
        }
    }
}
