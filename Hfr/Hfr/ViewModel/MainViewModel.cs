using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using Hfr.Commands;
using Hfr.Helpers;
using Hfr.Model;
using Hfr.Services;
using Windows.UI.Xaml;

namespace Hfr.ViewModel
{
    public delegate void TopicReadyToBeDisplayed(Topic topic, string computedHtml);
    public class MainViewModel : ViewModelBase
    {
        #region private fields
        private AccountManager _accountManager;

        internal void ShowContextForMessage(object parameter)
        {
            Debug.WriteLine("VM ContextMessageCommand param=" + parameter);

            ShowPopup(parameter.ToString());
        }

        private async void ShowPopup(string parameter)
        {
            WwwFormUrlDecoder decoder = new WwwFormUrlDecoder(parameter);
            Debug.WriteLine(decoder.GetFirstValueByName("num"));
            Debug.WriteLine(decoder.GetFirstValueByName("offset"));
            var menu = new PopupMenu();
            menu.Commands.Add(new UICommand("Citer", (command) =>
            {
                Debug.WriteLine("VM Open param=" + parameter);
            }));

            var chosenCommand = await menu.ShowAsync(new Point(100, Convert.ToDouble(decoder.GetFirstValueByName("offset")) + 44.0));
            if (chosenCommand == null)
            {
                Debug.WriteLine("VM Dismiss param=" + parameter);
            }
        }

        private ObservableCollection<Topic> drapeaux;
        private bool _drapeauxLoading;
        private bool _drapeauxLoaded;
        private bool isDrapeauxLoading
        {
            get
            {
                return _drapeauxLoading;
            }
            set
            {
                Set(ref _drapeauxLoading, value);
                if (value)
                {
                    Set(ref _drapeauxLoaded, false);
                }
            }
        }
        private bool isDrapeauxLoaded
        {
            get
            {
                return _drapeauxLoaded;
            }
            set
            {
                Set(ref _drapeauxLoaded, value);
                if (value)
                {
                    Set(ref _drapeauxLoading, false);
                }
            }
        }

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
                isDrapeauxLoaded = true;
                RaisePropertyChanged(nameof(LoadingScreenDraps));
            }
        }

        public IEnumerable<IGrouping<string, Topic>> DrapsGrouped
        {
            get
            {
                if (_favorisGrouped == null)
                {
                    RefreshDraps();
                }
                return _favorisGrouped;
            }
            set
            {
                Set(ref _favorisGrouped, value);
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
        #endregion
        #region events
        public event TopicReadyToBeDisplayed TopicReadyToBeDisplayed;
        #endregion
        #region public properties
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
                    Task.Run(async () => await TopicFetcher.GetPosts(CurrentTopic));
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

        #region methods
        public void RefreshDraps()
        {
            if (!isDrapeauxLoading)
            {
                isDrapeauxLoading = true;
                Task.Run(async () => await DrapFetcher.GetDraps());
            }
        }
        #endregion

        #region commands
        public OpenTopicCommand OpenTopicCommand { get; } = new OpenTopicCommand();
        public RefreshDrapsCommand RefreshDrapsCommand { get; } = new RefreshDrapsCommand();
        public ContextMessageCommand ContextMessageCommand { get; } = new ContextMessageCommand();
        
#warning "for debugging purpose"
        public ShowEditorCommand ShowEditorCommand { get; } = new ShowEditorCommand();
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