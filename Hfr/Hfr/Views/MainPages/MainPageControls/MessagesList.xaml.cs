using Windows.UI.Xaml.Controls;

namespace Hfr.Views.MainPages.MainPageControls
{
    public sealed partial class MessagesList : UserControl
    {
        public MessagesList()
        {
            this.InitializeComponent();
        }
        private void SemanticZoom_OnViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            ChatsZoomeOutListView.ItemsSource = PrivateChatsCvs.View.CollectionGroups;
        }
    }
}
