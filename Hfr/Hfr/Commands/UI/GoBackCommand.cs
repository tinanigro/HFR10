using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.UI
{
    public class GoBackCommand : Command
    {
        public override void Execute(object parameter)
        {
            Loc.NavigationService.GoBack();
        }
    }
}
