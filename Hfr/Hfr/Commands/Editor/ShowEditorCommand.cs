using System.Diagnostics;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using Hfr.Models;

namespace Hfr.Commands.Editor
{
    public class ShowEditorCommand : Command
    {
        public override void Execute(object parameter)
        {
            Debug.WriteLine("ShowEditorCommand param=" + parameter);
            if (parameter == null)
            {
                // We assume it's a New Answer in the Current Topic
                Loc.NavigationService.Navigate(View.Editor, new EditorPackage(EditorIntent.New, Loc.Topic.CurrentTopic.TopicNewPostUriForm));
            }
            else if (parameter is EditorPackage)
            {
                var package = (EditorPackage)parameter;
                Loc.NavigationService.Navigate(View.Editor, package);
            }
            Loc.NavigationService.ShowBackButtonIfCanGoBack();
        }
    }
}
