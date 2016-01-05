using GalaSoft.MvvmLight;
using Hfr.Commands.Settings;
using Hfr.Helpers;
using Hfr.Models;
using System.Collections.Generic;
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

        public Visibility SquareAvatarVisible { get { return _squareAvatarStylePreferred ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility CircleAvatarVisible { get { return _squareAvatarStylePreferred ? Visibility.Collapsed : Visibility.Visible; } }
        #endregion
        #region public props

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
        #endregion

        #region commands
        public GoToAppTopicCommand GoToAppTopicCommand { get; } = new GoToAppTopicCommand();
        #endregion
    }
}
