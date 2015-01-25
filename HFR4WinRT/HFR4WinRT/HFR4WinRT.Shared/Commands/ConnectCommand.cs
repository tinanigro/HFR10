using HFR4WinRT.Helpers;
using HFR4WinRT.Utilities;
using HFR4WinRT.ViewModel;

namespace HFR4WinRT.Commands
{
    public class ConnectCommand : Command
    {
        public override async void Execute(object parameter)
        {
            await Locator.Main.AccountManager.CurrentAccount.BeginAuthentication();
        }
    }
}
