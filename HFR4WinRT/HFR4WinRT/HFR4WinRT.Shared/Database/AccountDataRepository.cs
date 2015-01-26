using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HFR4WinRT.Model;
using SQLite;

namespace HFR4WinRT.Database
{
    public class AccountDataRepository : IDataRepository
    {
        private static readonly string _dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "accounts.sqlite");
        private SQLiteAsyncConnection connection;
        public AccountDataRepository()
        {
            Initialize();
        }


        public void Initialize()
        {
            connection = new SQLiteAsyncConnection(_dbPath);
            connection.CreateTableAsync<Account>();
        }

        public void Drop()
        {
            connection.DropTableAsync<Account>();
        }

        public void Clear()
        {
            connection.QueryAsync<Account>("DELETE * FROM ACCOUNT");
        }

        public Task<List<Account>> GetAccounts()
        {
            return connection.Table<Account>().ToListAsync();
        }

        public void Add(Account acc)
        {
            connection.InsertAsync(acc);
        }

        public void Update(Account currentAccount)
        {
            connection.UpdateAsync(currentAccount);
        }
    }
}
