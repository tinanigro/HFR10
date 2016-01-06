using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using Hfr.Commands;
using Hfr.Commands.Editor;
using Hfr.Commands.Topic;
using Hfr.Helpers;
using Hfr.Model;
using Hfr.Models;

namespace Hfr.ViewModel
{
    public class TopicViewModel : ViewModelBase
    {
        #region private props
        private int _selectedTopic;
        private bool _isTopicLoading;
        #endregion
        #region private fields
        #endregion
        #region public props      

        public bool IsTopicLoading
        {
            get { return _isTopicLoading;}
            set
            {
                Set(ref _isTopicLoading, value); 
                RaisePropertyChanged(nameof(TopicLoadingVisibility));
            }
        }

        public Visibility TopicLoadingVisibility => IsTopicLoading ? Visibility.Visible : Visibility.Collapsed;

        public int SelectedTopic
        {
            get { return _selectedTopic; }
            set
            {
                Set(ref _selectedTopic, value);
                if (TopicVisible)
                {
                    Loc.NavigationService.ShowBackButtonIfCanGoBack();
                    RaisePropertyChanged(nameof(CurrentTopic));
                }
                if (value == -1)
                {
                    // No topic displayed, reset webview
                    UpdateTopicWebView(null);
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
                if (!Loc.Main.Topics.Any() || SelectedTopic == -1) return null;
                else if (SelectedTopic > -1 && SelectedTopic < Loc.Main.Topics.Count)
                    return Loc.Main.Topics[(int)SelectedTopic];
                else
                {
                    SelectedTopic = 0;
                    return Loc.Main.Topics[(int)SelectedTopic];
                }
            }
        }

        public bool TopicVisible
        {
            get
            {
                Loc.Main.TriggerUIAdapter();
                return CurrentTopic != null;
            }
        }
        #endregion

        #region public fields
        #endregion

        #region commands
        public ChangeTopicPageCommand ChangeTopicPageCommand { get; } = new ChangeTopicPageCommand();
        public RefreshTopicCommand RefreshTopicCommand { get; } = new RefreshTopicCommand();
        public ShowEditorCommand ShowEditorCommand { get; } = new ShowEditorCommand();
        #endregion

        #region events
        public event TopicReadyToBeDisplayed TopicReadyToBeDisplayed;
        #endregion

        #region methods
        public void UpdateTopicWebView(Topic topic)
        {
            TopicReadyToBeDisplayed?.Invoke(topic);
            if (topic != null)
            Task.Run(async () => await DrapFetcher.GetDraps(Loc.Settings.FollowedTopicType));
        }

        public async Task RefreshPage(EditorIntent editorIntent)
        {
            await ThreadUI.Invoke(() =>
            {
                switch (editorIntent)
                {
                    case EditorIntent.New:
                    case EditorIntent.Quote:
                    case EditorIntent.MultiQuote:
                        CurrentTopic.TopicCurrentPage = CurrentTopic.TopicNbPage;
                        break;
                    case EditorIntent.Edit:
                        break;
                    default:
                        break;
                }
            });
            await Task.Run(async () => await TopicFetcher.GetPosts(CurrentTopic));
        }
        #endregion

    }
}