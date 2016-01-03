using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Hfr.Commands.Settings;

namespace Hfr.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region commands
        public GoToAppTopicCommand GoToAppTopicCommand { get; } = new GoToAppTopicCommand();
        #endregion
    }
}
