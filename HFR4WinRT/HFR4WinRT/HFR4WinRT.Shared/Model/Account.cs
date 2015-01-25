using System.Net;
using GalaSoft.MvvmLight;
using SQLite;

namespace HFR4WinRT.Model
{
    public class Account : ViewModelBase
    {
        private string pseudo;
        private string password;

        public string Pseudo
        {
            get { return pseudo; }
            set { Set(ref pseudo, value); }
        }

        public string Password
        {
            get { return password; }
            set { Set(ref password, value); }
        }

        [Ignore]
        public CookieContainer CookieContainer { get; set; }
    }
}
