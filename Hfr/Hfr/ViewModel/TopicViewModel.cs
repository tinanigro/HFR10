﻿using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Hfr.Commands;
using Hfr.Commands.Editor;
using Hfr.Commands.Topic;
using Hfr.Helpers;
using Hfr.Model;

namespace Hfr.ViewModel
{
    public class TopicViewModel : ViewModelBase
    {
        #region private props
        private int _selectedTopic;
        #endregion
        #region private fields
        #endregion
        #region public props        
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
        public ShowEditorCommand ShowEditorCommand { get; } = new ShowEditorCommand();
        #endregion

        #region events
        public event TopicReadyToBeDisplayed TopicReadyToBeDisplayed;
        #endregion

        #region methods
        public void UpdateTopicWebView(Topic topic)
        {
            TopicReadyToBeDisplayed?.Invoke(topic);
        }

        public Task RefreshPage()
        {
            return Task.Run(async () => await TopicFetcher.GetPosts(CurrentTopic));
        }
        #endregion

    }
}