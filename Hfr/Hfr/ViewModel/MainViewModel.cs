using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Hfr.Commands;
using Hfr.Helpers;
using Hfr.Model;
using Hfr.Services;
using Hfr.Services.Classes;
using Windows.UI.Xaml;

namespace Hfr.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region private fields
        private AccountManager _accountManager;
        private ObservableCollection<Topic> drapeaux;
        private bool isDrapeauxLoading;
        private bool isDrapeauxLoaded;
        private IEnumerable<IGrouping<string, Topic>> _favorisGrouped;
        private ObservableCollection<Topic> _topics = new ObservableCollection<Topic>();
        private int _selectedTopic;
        #endregion

        #region public fields
        public AccountManager AccountManager { get { return _accountManager; } set { Set(ref _accountManager, value); } }

        public ObservableCollection<Topic> Drapeaux
        {
            get
            {
                return drapeaux;
            }
            set
            {
                Set(ref drapeaux, value);
            }
        }

        public IEnumerable<IGrouping<string, Topic>> DrapsGrouped
        {
            get
            {
                if (_favorisGrouped == null || !isDrapeauxLoading)
                {
                    isDrapeauxLoading = true;
                    Task.Run(async () => await DrapFetcher.GetDraps());
                }
                return _favorisGrouped;
            }
            set
            {
                Set(ref _favorisGrouped, value);
                isDrapeauxLoaded = true;
                RaisePropertyChanged(nameof(LoadingScreenDraps));
            }
        }

        public Visibility LoadingScreenDraps
        {
            get { return isDrapeauxLoaded ? Visibility.Collapsed : Visibility.Visible; }
        }
        #region posts

        public ObservableCollection<Topic> Topics
        {
            get { return _topics; }
            set { Set(ref _topics, value); }
        }

        public int SelectedTopic
        {
            get { return _selectedTopic; }
            set
            {
                Set(ref _selectedTopic, value);
                RaisePropertyChanged(nameof(CurrentTopic));
                RaisePropertyChanged(nameof(TopicVisible));
                Loc.NavigationService.ShowBackButtonIfCanGoBack();
                if (CurrentTopic != null)
                {
                    var topicCatid = CurrentTopic.TopicCatId;
                    var topicId = CurrentTopic.TopicId;
                    var nbPage = CurrentTopic.TopicNbPage;
                    Task.Run(async () => await TopicFetcher.GetPosts(topicCatid, topicId, nbPage));
                }
            }
        }

        public Topic CurrentTopic
        {
            get
            {
                if (IsInDesignMode)
                {
                    return new Topic()
                    {
                        TopicName = "TU DesignTime"
                    };
                }
                if (!Topics.Any() || SelectedTopic == -1) return null;
                else if (SelectedTopic > -1 && SelectedTopic < Topics.Count)
                    return Topics[(int)SelectedTopic];
                else
                {
                    SelectedTopic = 0;
                    return Topics[(int)SelectedTopic];
                }
            }
        }

        public bool TopicVisible
        {
            get { return CurrentTopic != null; }
        }

        #endregion
        #region commands
        public OpenTopicCommand OpenTopicCommand { get; } = new OpenTopicCommand();
        #endregion
        #endregion
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