using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.UI
{
    public class NavigateToPrivateChatsList : Command
    {
        public override void Execute(object parameter)
        {
            if (Loc.NavigationService.CurrentView != View.Main)
            {
                Loc.NavigationService.Navigate(View.Main);
            }
            Loc.NavigationService.Navigate(View.PrivateChatsList);
        }
    }
}
