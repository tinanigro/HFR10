using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Diagnostics;

namespace Hfr.Commands
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

    public class SubmitEditorCommand : Command
    {
        public override void Execute(object parameter)
        {
            Loc.Editor.SubmitEditor();
        }
    }
    
}
