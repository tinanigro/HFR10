using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Diagnostics;

namespace Hfr.Commands
{
    

    public class SubmitEditorCommand : Command
    {
        public override void Execute(object parameter)
        {
            Loc.Editor.SubmitEditor();
        }
    }
    
}
