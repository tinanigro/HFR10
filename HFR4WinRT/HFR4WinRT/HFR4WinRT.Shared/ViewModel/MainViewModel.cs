using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using HFR4WinRT.Services;
using HFR4WinRT.Services.Classes;

namespace HFR4WinRT.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private AccountManager _accountManager;
        public AccountManager AccountManager { get { return _accountManager; } set { Set(ref _accountManager, value); } }

        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
    }
}