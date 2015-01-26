using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using HFR4WinRT.Helpers;
using HFR4WinRT.Model;
using HFR4WinRT.Services;
using HFR4WinRT.Services.Classes;

namespace HFR4WinRT.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private AccountManager _accountManager;
        private ObservableCollection<Topic> drapeaux;
        private bool isDrapeauxLoaded;
        private IEnumerable<IGrouping<string, Topic>> _favorisGrouped; 
        public AccountManager AccountManager { get { return _accountManager; } set { Set(ref _accountManager, value); } }

        public ObservableCollection<Topic> Drapeaux
        {
            get
            {
                if (drapeaux == null && !isDrapeauxLoaded)
                {
                    isDrapeauxLoaded = true;
                    Task.Run(async ()=> await DrapFetcher.GetDraps());
                }
                return drapeaux;
            }
            set { Set(ref drapeaux, value); }
        }

        public IEnumerable<IGrouping<string, Topic>> DrapsGrouped
        {
            get
            {
                return _favorisGrouped;
            }
            set { Set(ref _favorisGrouped, value); }
        }

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