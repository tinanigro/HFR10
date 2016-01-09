using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Hfr.Model;
using Hfr.Models;
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
            Loc.NavigationService.Navigate(View.PrivateChat);
        }
    }
}
