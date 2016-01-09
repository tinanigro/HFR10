using Windows.UI.Xaml.Controls;
using Hfr.Model;
using Hfr.Views.MainPages.MainPageControls.PrivateChatsViews;

namespace Hfr.Views.MainPages.MainPageControls
{
    public sealed partial class MessagesList : UserControl
    {
        public MessagesList()
        {
            this.InitializeComponent();
            Navigate(View.PrivateChatsList);
        }

        public void Navigate(View view)
        {
            switch (view)
            {
                case View.PrivateChatsList:
                    NavigationFrame.Navigate(typeof(PrivateChatsList));
                    break;
                case View.PrivateChat:
                    NavigationFrame.Navigate(typeof(PrivateChat));
                    break;
            }
        }
    }
}
