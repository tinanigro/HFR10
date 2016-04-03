using GalaSoft.MvvmLight;
using Hfr.Commands.Settings;
using Hfr.Helpers;
using Hfr.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Hfr.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region private properties
        private ApplicationSettingsHelper ApplicationSettingsHelper = new ApplicationSettingsHelper();
        private FollowedTopicType followedTopicType;
        private bool _displayPrivateChatsByDefault;
        private bool _squareAvatarStylePreferred;
        private bool _isAppThemeDark;
        private double _fontSizePreferred;
        private double _postHeaderTransparencyPreferred;
        private string _ignoreListMembers;
        #endregion

        #region public properties        
        public FollowedTopicType FollowedTopicType
        {
            get
            {
                var followTopicType = ApplicationSettingsHelper.ReadSettingsValue(nameof(FollowedTopicType), false);
                if (followTopicType == null)
                {
                    followedTopicType = FollowedTopicType.Drapeaux;
                }
                else
                {
                    followedTopicType = (FollowedTopicType)followTopicType;
                }
                return followedTopicType;
            }
            set
            {
                if (followedTopicType == value) return;
                Set(ref followedTopicType, value);
                ApplicationSettingsHelper.SaveSettingsValue(nameof(FollowedTopicType), (int)value, false);
            }
        }

        public bool DisplayPrivateChatsByDefault
        {
            get
            {
                var displayChats = ApplicationSettingsHelper.ReadSettingsValue(nameof(DisplayPrivateChatsByDefault), false);
                if (displayChats == null)
                {
                    _displayPrivateChatsByDefault = true;
                }
                else
                {
                    _displayPrivateChatsByDefault = (bool)displayChats;
                }
                return _displayPrivateChatsByDefault;
            }
            set
            {
                if (_displayPrivateChatsByDefault == value) return;
                ApplicationSettingsHelper.SaveSettingsValue(nameof(DisplayPrivateChatsByDefault), value, false);
                Set(ref _displayPrivateChatsByDefault, value);
            }
        }

        public bool SquareAvatarStylePreferred
        {
            get
            {
                var square = ApplicationSettingsHelper.ReadSettingsValue(nameof(SquareAvatarStylePreferred), false);
                if (square == null)
                {
                    _squareAvatarStylePreferred = false;
                }
                else
                {
                    _squareAvatarStylePreferred = (bool)square;
                }
                return _squareAvatarStylePreferred;
            }
            set
            {
                if (_squareAvatarStylePreferred == value) return;
                ApplicationSettingsHelper.SaveSettingsValue(nameof(SquareAvatarStylePreferred), value, false);
                Set(ref _squareAvatarStylePreferred, value);
                RaisePropertyChanged(nameof(SquareAvatarVisible));
                RaisePropertyChanged(nameof(CircleAvatarVisible));
            }
        }

        public bool IsApplicationThemeDark
        {
            get
            {
                var themeDark = ApplicationSettingsHelper.ReadSettingsValue(nameof(IsApplicationThemeDark), false);
                if (themeDark == null)
                {
                    _isAppThemeDark = true;
                }
                else
                {
                    _isAppThemeDark = (bool)themeDark;
                }
                return _isAppThemeDark;
            }
            set
            {
                if (_isAppThemeDark == value) return;
                ApplicationSettingsHelper.SaveSettingsValue(nameof(IsApplicationThemeDark), value, false);
                Set(ref _isAppThemeDark, value);
                App.AppShell.GoToDarkTheme(value);
            }
        }

        public double MinimumFontSize { get { return 10; } }
        public double MaximumFontSize { get { return 20; } }

        /// <summary>
        /// This setting is not synced between devices by design
        /// </summary>
        public double FontSizePreferred
        {
            get
            {
                var fontSize = ApplicationSettingsHelper.ReadSettingsValue(nameof(FontSizePreferred), true);
                if (fontSize == null)
                {
                    _fontSizePreferred = 15;
                }
                else
                {
                    if ((double)fontSize < 10)
                    {
                        _fontSizePreferred = 15;
                    }
                    else _fontSizePreferred = (double)fontSize;
                }
                return _fontSizePreferred;
            }
            set
            {
                if (_fontSizePreferred == value) return;
                ApplicationSettingsHelper.SaveSettingsValue(nameof(FontSizePreferred), value, true);
                Set(ref _fontSizePreferred, value);
            }
        }

        public double PostHeaderTransparencyPreferred
        {
            get
            {
                var postHeaderTransparency = ApplicationSettingsHelper.ReadSettingsValue(nameof(PostHeaderTransparencyPreferred), false);
                if (postHeaderTransparency == null)
                {
                    _postHeaderTransparencyPreferred = 0.50;
                }
                else
                {
                    _postHeaderTransparencyPreferred = (double)postHeaderTransparency;
                }

                return _postHeaderTransparencyPreferred;
            }
            set
            {
                if (_postHeaderTransparencyPreferred == value) return;
                ApplicationSettingsHelper.SaveSettingsValue(nameof(PostHeaderTransparencyPreferred), value, false);
                Set(ref _postHeaderTransparencyPreferred, value);
            }
        }

        public Visibility SquareAvatarVisible { get { return _squareAvatarStylePreferred ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility CircleAvatarVisible { get { return _squareAvatarStylePreferred ? Visibility.Collapsed : Visibility.Visible; } }

        public string IgnoreListMembers
        {
            get
            {
                var list = ApplicationSettingsHelper.ReadSettingsValue(nameof(IgnoreListMembers), false);
                if (list == null)
                {
                    _ignoreListMembers = string.Empty;
                }
                else
                {
                    _ignoreListMembers = list.ToString();
                }
                return _ignoreListMembers;
            }
            set
            {
                if (_ignoreListMembers == value) return;
                Set(ref _ignoreListMembers, value);
                ApplicationSettingsHelper.SaveSettingsValue(nameof(IgnoreListMembers), value, false);
                RaisePropertyChanged(nameof(IgnoreListMembersList));
            }
        }
        #endregion
        #region public fields

        public IEnumerable<FollowedTopicType> FollowedTopicTypes
        {
            get
            {
                return new List<FollowedTopicType>()
                {
                    FollowedTopicType.Drapeaux,
                    FollowedTopicType.Favoris,
                    FollowedTopicType.Lus
                };
            }
        }

        public IEnumerable<string> IgnoreListMembersList
        {
            get { return _ignoreListMembers?.Split(';'); }
        }
        #endregion
        #region methods
        public void OnNavigatedTo()
        {
            Task.Run(() => Initialize());
        }

        async Task Initialize()
        {
            await ConnectHelper.GetAvatar(Loc.Main.AccountManager.CurrentAccount);
            Loc.Main.AccountManager.UpdateCurrentAccountInDB();
        }
        #endregion
        #region commands
        public GoToAppTopicCommand GoToAppTopicCommand { get; } = new GoToAppTopicCommand();
        #endregion
    }
}
