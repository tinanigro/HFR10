using System;
using Hfr.Utilities;
using Hfr.ViewModel;
using Windows.UI.Popups;

namespace Hfr.Commands.Editor
{
    public class CancelMessageCommand : Command
    {
        public override async void Execute(object parameter)
        {
            var md = new MessageDialog("Voulez-vous vraiment abandonner le post en cours d'édition ?", "Attention");
            md.Commands.Add(new UICommand()
            {
                Id = 1,
                Label = "Oui",
                Invoked = (command) =>
                {
                    Loc.NavigationService.GoBack();
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
