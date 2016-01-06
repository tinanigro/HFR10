using System;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Hfr.Commands.Editor
{
    public class DeleteMessageCommand : Command
    {
        public override async void Execute(object parameter)
        {
            string deleteEntry;
            if (Loc.Editor.CurrentEditor.Data.TryGetValue("delete", out deleteEntry))
            {
                var md = new MessageDialog("Voulez-vous vraiment supprimer le post en cours d'édition ?", "Attention");
                md.Commands.Add(new UICommand()
                {
                    Id = 1,
                    Label = "Oui",
                    Invoked = (command) =>
                    {
                        Loc.Editor.CurrentEditor.Data["delete"] = "1";
                        Task.Run(() => Loc.Editor.SubmitEditor());
                    },
                });
                md.Commands.Add(new UICommand()
                {
                    Id = 0,
                    Label = "Non"
                });
                md.CancelCommandIndex = 0;
                md.DefaultCommandIndex = 1;
                md.Options = MessageDialogOptions.AcceptUserInputAfterDelay;
                await md.ShowAsync();
            }
        }
    }
}
