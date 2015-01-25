using System.Collections.Generic;
using System.IO;
using System.Linq;
using HFR4WinRT.Model;
using SQLite;

namespace HFR4WinRT.Database
{
    public class AccountDataRepository : IDataRepository
    {
        private static readonly string _dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "accounts.sqlite");
        private SQLiteConnection connection;
        public AccountDataRepository()
        {
            Initialize();
        }


        public void Initialize()
        {
            connection = new SQLiteConnection(_dbPath);
            connection.CreateTable<Account>();
        }

        public void Drop()
        {
            using (connection)
            {
                connection.DropTable<Account>();
            }
        }

        public void Clear()
        {
            using (connection)
            {
                connection.DeleteAll<Account>();
            }
        }

        public List<Account> GetAccounts()
        {
            using (connection)
            {
                return connection.Table<Account>().ToList();
            }
        }

        public void Add(Account acc)
        {
            using (connection)
            {
                connection.Insert(acc);
            }
        }
    }
}
