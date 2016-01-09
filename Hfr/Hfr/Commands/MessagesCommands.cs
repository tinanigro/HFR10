using System.Diagnostics;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Linq;
using Windows.UI.Xaml.Controls;


namespace Hfr.Commands
{
    public class ContextMessageCommand : Command
    {
        public override void Execute(object parameter)
        {
            Debug.WriteLine("ContextMessageCommand param=" + parameter);

            Loc.Main.ShowContextForMessage(parameter);
        }
    }
}