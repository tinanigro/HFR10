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
            if (parameter is Model.Topic)
            {
                Loc.NavigationService.Navigate(View.Editor, ((Model.Topic)parameter).TopicNewPostUriForm);
            }
            else if (parameter is string)
            {
                Loc.NavigationService.Navigate(View.Editor, parameter);
            }
            Loc.NavigationService.ShowBackButtonIfCanGoBack();
        }
    }
}
