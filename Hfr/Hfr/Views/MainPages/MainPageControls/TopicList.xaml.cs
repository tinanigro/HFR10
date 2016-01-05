using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hfr.Views.MainPages.MainPageControls
{
    public sealed partial class TopicList : UserControl
    {
        public TopicList()
        {
            this.InitializeComponent();
        }

        private void SemanticZoom_OnViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            DrapsZoomeOutListView.ItemsSource = DrapsCvs.View.CollectionGroups;
        }
    }
}
