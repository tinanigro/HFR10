using System.Diagnostics;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Editor
{
    public class ShowEditorCommand : Command
    {
        public override void Execute(object parameter)
        {
            Debug.WriteLine("ShowEditorCommand param=" + parameter);
            Loc.NavigationService.Navigate(View.Editor, parameter);
            Loc.NavigationService.ShowBackButtonIfCanGoBack();
        }
    }
}
