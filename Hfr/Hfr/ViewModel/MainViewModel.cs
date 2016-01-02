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
using Hfr.Commands.UI;
using Hfr.Models;
using Hfr.Utilities;

namespace Hfr.ViewModel
{
    public delegate void TopicReadyToBeDisplayed(Topic topic, string computedHtml);
    public class MainViewModel : ViewModelBase
    {
        #region private properties 
        private AccountManager _accountManager;
        private int _selectedTopic;
        private bool _drapeauxLoading;
        private bool _drapeauxLoaded;
        private bool _categoriesLoading;
        private bool _privateChatsLoading;

        private MainColumn _mainColumn;
        private bool _firstColumnAndTopicVisible;
        private bool _allColumnsVisible;
        private bool _twoColumnsVisible;
        private bool _topicListColumnVisible;
        private bool _topicViewColumnVisible;
        private bool _categoriesListColumnVisible;
        private bool _privateChatsColumnVisible;

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

        public bool IsCategoriesLoading
        {
            get { return _categoriesLoading;}
            set
            {
                Set(ref _categoriesLoading, value);
                RaisePropertyChanged(nameof(LoadingCategoriesList));
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
        public Visibility LoadingCategoriesList => IsCategoriesLoading ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LoadingPrivateChatsList => IsPrivateChatsLoading ? Visibility.Visible: Visibility.Collapsed;
        #endregion

        #region private fields
        private ObservableCollection<Topic> drapeaux;
        private List<SubCategory> _categories;
        private List<PrivateChat> privateChats;
        private IEnumerable<IGrouping<string, Topic>> _favorisGrouped;
        private IEnumerable<IGrouping<string, SubCategory>> _catsGrouped;
        private IEnumerable<IGrouping<string, PrivateChat>> _privateChatsGrouped;
        private ObservableCollection<Topic> _topics = new ObservableCollection<Topic>();

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
                IsDrapeauxLoaded = true;
                IsDrapeauxLoading = false;
            }
        }

        public List<SubCategory> Categories
        {
            get { return _categories; }
            set
            {
                Set(ref _categories, value);
                IsCategoriesLoading = false;
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

        public IEnumerable<IGrouping<string, SubCategory>> CategoriesGrouped
        {
            get
            {
                if (_catsGrouped == null)
                {
                    RefreshCats();
                }
                return _catsGrouped;
            }
            set
            {
                Set(ref _catsGrouped, value);
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
            get
            {
                TriggerUIAdapter();
                return CurrentTopic != null;
            }
        }

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

        public bool CategoriesListColumnVisible
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

        #endregion
        #region methods
        void TriggerUIAdapter()
        {
            TopicViewColumnVisible = false;
            FirstColumnAndTopicVisible = false;
            AllColumnsVisible = false;
            TwoColumnsVisible = false;

            TopicListColumnVisible = false;
            CategoriesListColumnVisible = false;
            PrivateChatsColumnVisible = false;

            // TODO : Use UI logic here instead of lazy loading it into properties.
            var width = Window.Current.Bounds.Width;
            if (CurrentTopic != null)
            {
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
                            CategoriesListColumnVisible = true;
                            break;
                        case MainColumn.PrivateChats:
                            PrivateChatsColumnVisible = true;
                            break;
                    }
                }
                else if (width < Strings.NormalWidth)
                {
                    TwoColumnsVisible = true;
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
                Task.Run(async () => await DrapFetcher.GetDraps());
            }
        }

        public void RefreshCats()
        {
            if (!_categoriesLoading)
            {
                IsCategoriesLoading = true;
                Task.Run(async () => await CatFetcher.GetCats());
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

        public void UpdateTopicWebView(Topic topic, string html)
        {
            TopicReadyToBeDisplayed?.Invoke(topic, html);
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
        public ShowEditorCommand ShowEditorCommand { get; } = new ShowEditorCommand();

        public OpenSplitViewPaneCommand OpenSplitViewPaneCommand { get; } = new OpenSplitViewPaneCommand();

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