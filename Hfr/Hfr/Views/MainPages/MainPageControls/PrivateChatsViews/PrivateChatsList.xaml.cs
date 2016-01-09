using System;
using Windows.UI.Xaml.Controls;

namespace Hfr.Views.MainPages.MainPageControls.PrivateChatsViews
{
    public sealed partial class PrivateChatsList : Page
    {
        public PrivateChatsList()
        {
            this.InitializeComponent();
        }

        private void SemanticZoom_OnViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            ChatsZoomeOutListView.ItemsSource = PrivateChatsCvs.View.CollectionGroups;
        }
    }
}
