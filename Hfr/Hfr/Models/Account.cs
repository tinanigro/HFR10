using System.Net;
using GalaSoft.MvvmLight;
using SQLite;

namespace Hfr.Model
{
    public class Account : ViewModelBase
    {
        private string pseudo;
        private string password;
        private int userId;
        private string _avatarId;
        private string cookieContainer;

        //TODO: Prevent two accounts with same pseudo in database
        [PrimaryKey]
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

        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                Set(ref userId, value);
            }
        }

        public string AvatarId
        {
            get { return _avatarId; }
            set { Set(ref _avatarId, value); }
        }

        public string CookieContainer { get; set; }
    }
}
