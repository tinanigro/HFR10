using System.Net;
using GalaSoft.MvvmLight;
using SQLite;
using Windows.UI.Xaml;

namespace Hfr.Model
{
    public class Account : ViewModelBase
    {
        private string pseudo;
        private string password;
        private int userId;
        private string _avatarId;
        private string cookieContainer;
        private bool isConnecting;
        private string connectionErrorStatus;

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
            get { return userId; }
            set { Set(ref userId, value); }
        }

        public string AvatarId
        {
            get { return _avatarId; }
            set { Set(ref _avatarId, value); }
        }

        public string ConnectionErrorStatus
        {
            get { return connectionErrorStatus; }
            set
            {
                Set(ref connectionErrorStatus, value);
                RaisePropertyChanged(nameof(ConnectionErrorTextVisible));
            }
        }

        public Visibility ConnectionErrorTextVisible
        {
            get { return string.IsNullOrEmpty(connectionErrorStatus) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                Set(ref isConnecting, value);
                RaisePropertyChanged(nameof(ConnectingVisible));
            }
        }

        public Visibility ConnectingVisible
        {
            get { return IsConnecting ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string CookieContainer { get; set; }
    }
}
