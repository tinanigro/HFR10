using GalaSoft.MvvmLight;
using Hfr.Commands.Settings;
using Hfr.Helpers;
using Windows.UI.Xaml;

namespace Hfr.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region private properties
        private ApplicationSettingsHelper ApplicationSettingsHelper = new ApplicationSettingsHelper();
        private bool _displayPrivateChatsByDefault;
        private bool _squareAvatarStylePreferred;
        #endregion
        #region public properties
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


        #region commands
        public GoToAppTopicCommand GoToAppTopicCommand { get; } = new GoToAppTopicCommand();
        #endregion
    }
}
