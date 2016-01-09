using Windows.UI.Xaml.Controls;

namespace Hfr.Views.MainPages.MainPageControls.ThreadsViews
{
    public sealed partial class FollowedThreadsList : UserControl
    {
        public FollowedThreadsList()
        {
            this.InitializeComponent();
        }

        private void SemanticZoom_OnViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            DrapsZoomeOutListView.ItemsSource = DrapsCvs.View.CollectionGroups;
        }
    }
}
