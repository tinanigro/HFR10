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
    public class NavigateToSettings : Command
    {
        public override void Execute(object parameter)
        {
            Loc.NavigationService.Navigate(View.Settings);
        }
    }
}
