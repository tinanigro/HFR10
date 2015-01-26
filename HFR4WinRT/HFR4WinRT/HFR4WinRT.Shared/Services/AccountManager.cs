using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var accounts = await accountDataRepository.GetAccounts();
            if (accounts != null && accounts.Any())
            {
                Accounts = accounts;
                if (accounts.Count == 1)
                {
                    CurrentAccount = accounts[0];
                    bool success = await CurrentAccount.BeginAuthentication(false);
                    if (success)
                    {
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
                    Locator.NavigationService.Navigate(Page.Connect);
                });
            }
        }

        public void UpdateCurrentAccountInDB()
        {
            accountDataRepository.Update(CurrentAccount);
        }

        internal void AddCurrentAccountInDB()
        {
            accountDataRepository.Add(CurrentAccount);
        }
    }
}
