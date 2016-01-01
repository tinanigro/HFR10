using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hfr.Commands;
using Hfr.Database;
using Hfr.Helpers;
using Hfr.Model;
using Hfr.ViewModel;

namespace Hfr.Services
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
            var accounts = accountDataRepository?.GetAccounts();
            if (accounts != null && accounts.Any())
            {
                Accounts = accounts;
                if (accounts.Count == 1)
                {
                    CurrentAccount = accounts[0];
#warning "Optimistic: any account/cookies is supposed valid, proper implementation needed (cookies timeout check)"
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
                    Loc.NavigationService.Navigate(View.Connect);
                });
            }
        }

        public void UpdateCurrentAccountInDB()
        {
            accountDataRepository?.Update(CurrentAccount);
        }

        internal void AddCurrentAccountInDB()
        {
            accountDataRepository?.Add(CurrentAccount);
        }
    }
}
