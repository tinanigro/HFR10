using Hfr.Models;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Hfr.Commands.Editor
{
    public class SmileyChosenCommand : Command
    {
        public override void Execute(object parameter)
        {
            Smiley smiley = null;
            if (parameter is ItemClickEventArgs)
            {
                smiley = ((ItemClickEventArgs)parameter).ClickedItem as Smiley;
            }
            else if (parameter is AutoSuggestBoxSuggestionChosenEventArgs)
            {
                smiley = ((AutoSuggestBoxSuggestionChosenEventArgs)parameter).SelectedItem as Smiley;
            }
            if (smiley == null) return;
            Loc.Editor.InsertSmiley(smiley);
        }
    }
}
