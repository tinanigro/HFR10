using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HFR4WinRT.Commands;
using HFR4WinRT.Database;
using HFR4WinRT.Helpers;
using HFR4WinRT.Model;
using HFR4WinRT.ViewModel;

namespace HFR4WinRT.Services
{
    public class AccountManager
    {
        private AccountDataRepository accountDataRepository = new AccountDataRepository();
        private ConnectCommand connectCommand = new ConnectCommand();
        public List<Account> Accounts = new List<Account>();
        public Account CurrentAccount { get; set; }
        public ConnectCommand ConnectCommand { get { return connectCommand; } }
        
        public AccountManager()
        {
            Initialize();
        }

        async Task Initialize()
        {
            var accounts = accountDataRepository.GetAccounts();
            if (accounts != null && accounts.Any())
            {
                if (accounts.Count == 1)
                {
                    CurrentAccount = accounts[0];
                    await CurrentAccount.BeginAuthentication();
                }
                else
                {
                    // Handle seletion of one of the multi accounts
                }
            }
            else
            {
                // navigate to connectpage
                await ThreadUI.Invoke(() =>
                {
                    CurrentAccount = new Account();
                    Accounts.Add(CurrentAccount);
                    Locator.NavigationService.Navigate(Page.Connect);
                });
            }
        }
    }
}
