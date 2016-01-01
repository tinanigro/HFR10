using Hfr.Utilities;

namespace Hfr.Commands.UI
{
    public class OpenSplitViewPaneCommand : Command
    {
        public override void Execute(object parameter)
        {
            App.AppShell.SplitView.IsPaneOpen = !App.AppShell.SplitView.IsPaneOpen;
        }
    }
}
