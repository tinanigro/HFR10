using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.UI
{
    public class NavigateToCategoriesList : Command
    {
        public override void Execute(object parameter)
        {
            if (Loc.NavigationService.CurrentView != View.Main)
            {
                Loc.NavigationService.Navigate(View.Main);
            }
            Loc.NavigationService.Navigate(View.CategoriesList);
        }
    }
}
