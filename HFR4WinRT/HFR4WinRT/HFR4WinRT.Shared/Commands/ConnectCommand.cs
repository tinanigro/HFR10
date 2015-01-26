using System;
using System.Diagnostics;
using System.Linq;
using HFR4WinRT.Helpers;
using HFR4WinRT.Model;
using HFR4WinRT.Utilities;
using HFR4WinRT.ViewModel;

namespace HFR4WinRT.Commands
{
    public class ConnectCommand : Command
    {
        public override async void Execute(object parameter)
        {
            if (Locator.Main.AccountManager.Accounts.FirstOrDefault(x => x.Pseudo == Locator.Main.AccountManager.CurrentAccount.Pseudo) != null) return;
            bool success = await Locator.Main.AccountManager.CurrentAccount.BeginAuthentication(true);
            if (success)
            {
                Locator.Main.AccountManager.Accounts.Add(Locator.Main.AccountManager.CurrentAccount);
                Locator.Main.AccountManager.AddCurrentAccountInDB();
                await ThreadUI.Invoke(() =>
                {
                    Locator.NavigationService.Navigate(Page.Main);
                });
            }
            else
            {
                Debug.WriteLine("Login failed");
            }
        }
    }
}
