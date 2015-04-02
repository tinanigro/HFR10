using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hfr.Model;
using SQLite;

namespace Hfr.Database
{
    public class AccountDataRepository : IDataRepository
    {
        private static readonly string _dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "accounts.sqlite");
        public AccountDataRepository()
        {
            Initialize();
        }

        public void Initialize()
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                connection.CreateTable<Account>();
            }
        }

        public void Drop()
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                connection.DropTable<Account>();
            }
        }

        public void Clear()
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                connection.Query<Account>("DELETE * FROM ACCOUNT");
            }
        }

        public List<Account> GetAccounts()
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                return connection.Table<Account>().ToList();
            }
        }

        public void Add(Account acc)
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                connection.Insert(acc);
            }
        }

        public void Update(Account currentAccount)
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                connection.Update(currentAccount);
            }
        }
    }
}
