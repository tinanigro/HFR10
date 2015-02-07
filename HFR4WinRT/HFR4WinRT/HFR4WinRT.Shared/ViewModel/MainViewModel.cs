using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using HFR4WinRT.Commands;
using HFR4WinRT.Helpers;
using HFR4WinRT.Model;
using HFR4WinRT.Services;
using HFR4WinRT.Services.Classes;

namespace HFR4WinRT.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region private fields
        private AccountManager _accountManager;
        private ObservableCollection<Topic> drapeaux;
        private bool isDrapeauxLoaded;
        private IEnumerable<IGrouping<string, Topic>> _favorisGrouped;
        private ObservableCollection<Topic> _topics = new ObservableCollection<Topic>();
        private uint _selectedTopic;
        private OpenTopicCommand _openTopicCommand;

        #endregion

        #region public fields
        public AccountManager AccountManager { get { return _accountManager; } set { Set(ref _accountManager, value); } }

        public ObservableCollection<Topic> Drapeaux
        {
            get
            {
                if (drapeaux == null && !isDrapeauxLoaded)
                {
                    isDrapeauxLoaded = true;
                    Task.Run(async () => await DrapFetcher.GetDraps());
                }
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
                return _favorisGrouped;
            }
            set { Set(ref _favorisGrouped, value); }
        }

        #region posts

        public ObservableCollection<Topic> Topics
        {
            get { return _topics; }
            set { Set(ref _topics, value); }
        }

        public uint SelectedTopic
        {
            get { return _selectedTopic; }
            set
            {
                Set(ref _selectedTopic, value);
                RaisePropertyChanged("CurrentTopic");
                var topicCatid = CurrentTopic.TopicCatId;
                var topicId = CurrentTopic.TopicId;
                var nbPage = CurrentTopic.TopicNbPage;
                Task.Run(async () => await TopicFetcher.GetPosts(topicCatid, topicId, nbPage));
            }
        }

        public Topic CurrentTopic
        {
            get
            {
                if (SelectedTopic > -1 && SelectedTopic < Topics.Count)
                    return Topics[(int) SelectedTopic];
                else
                {
                    SelectedTopic = 0;
                    return Topics[(int)SelectedTopic];
                }
            }
        }

        public OpenTopicCommand OpenTopicCommand
        {
            get { return _openTopicCommand ?? (_openTopicCommand = new OpenTopicCommand()); }
        }
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