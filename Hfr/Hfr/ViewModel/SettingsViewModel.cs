using GalaSoft.MvvmLight;
using Hfr.Commands.Settings;
using Hfr.Helpers;

namespace Hfr.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region private properties
        private ApplicationSettingsHelper ApplicationSettingsHelper = new ApplicationSettingsHelper();
        private bool _displayPrivateChatsByDefault;
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
        #endregion


        #region commands
        public GoToAppTopicCommand GoToAppTopicCommand { get; } = new GoToAppTopicCommand();
        #endregion
    }
}
