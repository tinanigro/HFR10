using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Settings.Accounts
{
    public class DeleteCurrentAccountCommand : Command
    {
        public override void Execute(object parameter)
        {
            Loc.Main.AccountManager.DeleteCurrentAccount();
            Task.Run(() => Loc.Main.AccountManager.Initialize());
        }
    }
}