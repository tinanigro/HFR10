using Windows.UI.Xaml.Controls;

namespace Hfr.Views.MainPages.MainPageControls.TopicsViews
{
    public sealed partial class FollowedTopicsList : UserControl
    {
        public FollowedTopicsList()
        {
            this.InitializeComponent();
        }

        private void SemanticZoom_OnViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            DrapsZoomeOutListView.ItemsSource = DrapsCvs.View.CollectionGroups;
        }
    }
}
