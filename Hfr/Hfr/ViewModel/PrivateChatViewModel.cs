using System;
using GalaSoft.MvvmLight;
using Hfr.Commands.PrivateChats;
using Hfr.Models;
using Hfr.Models.Threads;

namespace Hfr.ViewModel
{
    public class PrivateChatViewModel : ViewModelBase
    {
        #region private props

        private PrivateChat _privateChat;

        #endregion

        #region publics props

        public PrivateChat CurrentPrivateChat
        {
            get { return _privateChat;}
            set { Set(ref _privateChat, value); }
        }
        #endregion

        #region commands
        public OpenPrivateChatCommand OpenPrivateChatCommand { get; } = new OpenPrivateChatCommand();

        #endregion
    }
}
