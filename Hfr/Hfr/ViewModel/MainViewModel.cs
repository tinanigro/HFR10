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
using Hfr.Commands.Topics;
using Hfr.Commands.UI;
using Hfr.Models;
using Hfr.Utilities;

namespace Hfr.ViewModel
{
    public delegate void TopicReadyToBeDisplayed(Topic topic);
    public class MainViewModel : ViewModelBase
    {
        #region private properties 
        private AccountManager _accountManager;
        private bool _drapeauxLoading;
        private bool _drapeauxLoaded;
        private bool _privateChatsLoading;

        private MainColumn _mainColumn;
        private bool _firstColumnAndTopicVisible;
        private bool _allColumnsVisible;
        private bool _twoColumnsVisible;
        private bool _topicListColumnVisible;
        private bool _topicViewColumnVisible;
        private bool _categoriesListColumnVisible;
        private bool _privateChatsColumnVisible;
        private bool _displayPrivateChatsInsteadOfCategoriesVisible;

        public bool IsDrapeauxLoading
        {
            get { return _drapeauxLoading; }
            set
            {
                Set(ref _drapeauxLoading, value);
                RaisePropertyChanged(nameof(LoadingTopicsList));
            }
        }

        public bool IsDrapeauxLoaded
        {
            get { return _drapeauxLoaded; }
            set
            {
                Set(ref _drapeauxLoaded, value);
            }
        }


        public bool IsPrivateChatsLoading
        {
            get { return _privateChatsLoading; }
            set
            {
                Set(ref _privateChatsLoading, value);
                RaisePropertyChanged(nameof(LoadingPrivateChatsList));
            }
        }

        public Visibility LoadingTopicsList => IsDrapeauxLoading ? Visibility.Visible : Visibility.Collapsed;

        public Visibility LoadingPrivateChatsList => IsPrivateChatsLoading ? Visibility.Visible : Visibility.Collapsed;
        #endregion

        #region private fields
        private ObservableCollection<Topic> drapeaux;
        private List<PrivateChat> privateChats;
        private IEnumerable<IGrouping<string, Topic>> _favorisGrouped;

        private IEnumerable<IGrouping<string, PrivateChat>> _privateChatsGrouped;
        private ObservableCollection<Topic> _topics = new ObservableCollection<Topic>();

        #endregion
        #region public fields

        public ObservableCollection<Topic> Drapeaux
        {
            get
            {
                return drapeaux;
            }
            set
            {
                Set(ref drapeaux, value);
                IsDrapeauxLoaded = true;
                IsDrapeauxLoading = false;
            }
        }


        public List<PrivateChat> PrivateChats
        {
            get { return privateChats; }
            set
            {
                Set(ref privateChats, value);
                IsPrivateChatsLoading = false;
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


        public IEnumerable<IGrouping<string, PrivateChat>> PrivateChatsGrouped
        {
            get
            {
                if (_privateChatsGrouped == null)
                {
                    RefreshPrivateChats();
                }
                return _privateChatsGrouped;
            }
            set { Set(ref _privateChatsGrouped, value); }
        }

        public ObservableCollection<Topic> Topics
        {
            get { return _topics; }
            set { Set(ref _topics, value); }
        }
        #endregion
        #region public properties

        public AccountManager AccountManager { get { return _accountManager; } set { Set(ref _accountManager, value); } }

        public bool FirstColumnAndTopicVisible
        {
            get { return _firstColumnAndTopicVisible; }
            set { Set(ref _firstColumnAndTopicVisible, value); }
        }

        public bool AllColumnsVisible
        {
            get { return _allColumnsVisible; }
            set { Set(ref _allColumnsVisible, value); }
        }

        public bool TwoColumnsVisible
        {
            get { return _twoColumnsVisible; }
            set { Set(ref _twoColumnsVisible, value); }
        }

        public MainColumn DefaultColumn
        {
            get { return _mainColumn; }
            set
            {
                Set(ref _mainColumn, value);
                TriggerUIAdapter();
            }
        }

        public bool TopicListColumnVisible
        {
            get { return _topicListColumnVisible; }
            set { Set(ref _topicListColumnVisible, value); }
        }

        public bool CategoriesPanelColumnVisible
        {
            get { return _categoriesListColumnVisible; }
            set
            {
                Set(ref _categoriesListColumnVisible, value);
            }
        }

        public bool PrivateChatsColumnVisible
        {
            get { return _privateChatsColumnVisible; }
            set { Set(ref _privateChatsColumnVisible, value); }
        }

        public bool TopicViewColumnVisible
        {
            get { return _topicViewColumnVisible; }
            set { Set(ref _topicViewColumnVisible, value); }
        }

        public bool DisplayPrivateChatsInsteadOfCategoriesVisible
        {
            get { return _displayPrivateChatsInsteadOfCategoriesVisible; }
            set
            {
                if (_displayPrivateChatsInsteadOfCategoriesVisible == value) return;
                Set(ref _displayPrivateChatsInsteadOfCategoriesVisible, value);
                TriggerUIAdapter();
            }
        }

        #endregion
        #region events
        #endregion
        #region methods
        public void TriggerUIAdapter()
        {
            TopicViewColumnVisible = false;
            FirstColumnAndTopicVisible = false;
            AllColumnsVisible = false;
            TwoColumnsVisible = false;

            TopicListColumnVisible = false;
            CategoriesPanelColumnVisible = false;
            PrivateChatsColumnVisible = false;

            // TODO : Use UI logic here instead of lazy loading it into properties.
            var width = Window.Current.Bounds.Width;
            if (Loc.Topic.CurrentTopic != null)
            {
                DisplayPrivateChatsInsteadOfCategoriesVisible = false;
                if (width < Strings.PortraitWidth)
                {
                    TopicViewColumnVisible = true;
                }
                else
                {
                    FirstColumnAndTopicVisible = true;
                }
            }
            else
            {
                if (width < Strings.PortraitWidth)
                {
                    switch (DefaultColumn)
                    {
                        case MainColumn.TopicsList:
                            TopicListColumnVisible = true;
                            break;
                        case MainColumn.CategoriesList:
                            CategoriesPanelColumnVisible = true;
                            break;
                        case MainColumn.PrivateChats:
                            PrivateChatsColumnVisible = true;
                            break;
                    }
                }
                else if (width < Strings.NormalWidth || !Loc.Settings.DisplayPrivateChatsByDefault)
                {
                    if (DisplayPrivateChatsInsteadOfCategoriesVisible)
                    {
                        TwoColumnsVisible = false;
                    }
                    else
                    {
                        TwoColumnsVisible = true;
                    }
                }
                else
                {
                    AllColumnsVisible = true;
                }
            }
        }

        public void RefreshDraps()
        {
            if (!IsDrapeauxLoading)
            {
                IsDrapeauxLoading = true;
                IsDrapeauxLoaded = false;
                DrapsGrouped = null;
                Task.Run(async () => await DrapFetcher.GetDraps(Loc.Settings.FollowedTopicType));
            }
        }

        public void RefreshPrivateChats()
        {
            if (!_privateChatsLoading)
            {
                IsPrivateChatsLoading = true;
                Task.Run(async () => await PrivateChatsFetcher.GetPrivateChats());
            }
        }

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
        #endregion

        #region commands
        public OpenTopicCommand OpenTopicCommand { get; } = new OpenTopicCommand();
        public RefreshDrapsCommand RefreshDrapsCommand { get; } = new RefreshDrapsCommand();
        public ContextMessageCommand ContextMessageCommand { get; } = new ContextMessageCommand();

#warning "for debugging purpose"

        public SetDefaultColumnViewCommand SetDefaultColumnViewCommand { get; } = new SetDefaultColumnViewCommand();

        public NavigateToSettings NavigateToSettings { get; } = new NavigateToSettings();
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
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            TriggerUIAdapter();
        }
    }
}