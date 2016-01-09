using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Hfr.Helpers;
using Hfr.Model;
using Hfr.Models.Threads;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.PrivateChats
{
    public class OpenPrivateChatCommand : Command
    {
        public override void Execute(object parameter)
        {
            PrivateChat privateChat = null;
            if (parameter is ItemClickEventArgs)
            {
                privateChat = ((ItemClickEventArgs) parameter).ClickedItem as PrivateChat;
            }

            if (privateChat == null) return;
            Loc.PrivateChat.CurrentPrivateChat = privateChat;

            if (!Loc.Main.Threads.Any())
                Loc.Main.Threads.Add(privateChat);
            else Loc.Main.Threads[0] = privateChat;
            Loc.Thread.SelectedThread = 0;

            Task.Run(async () => await ThreadFetcher.GetPosts(Loc.Thread.CurrentThread));

            if (Loc.NavigationService.CurrentView != View.Main)
                Loc.NavigationService.Navigate(View.Main);
        }
    }
}
