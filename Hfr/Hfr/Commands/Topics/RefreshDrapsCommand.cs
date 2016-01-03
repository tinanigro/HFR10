using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Hfr.Commands
{
    public class RefreshDrapsCommand : Command
    {
        public override void Execute(object parameter)
        {
            Loc.Main.RefreshDraps();
        }
    }
}