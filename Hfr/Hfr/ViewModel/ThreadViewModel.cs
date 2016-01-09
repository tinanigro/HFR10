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
using Hfr.Models.Threads;

namespace Hfr.ViewModel
{
    public delegate void ThreadReadyToBeDisplayed(Thread thread);
    public class ThreadViewModel : ViewModelBase
    {
        #region private props
        private int _selectedThread;
        private bool _isThreadLoading;
        #endregion
        #region private fields
        #endregion
        #region public props      

        public bool IsThreadLoading
        {
            get { return _isThreadLoading; }
            set
            {
                Set(ref _isThreadLoading, value);
                RaisePropertyChanged(nameof(TopicLoadingVisibility));
            }
        }

        public Visibility TopicLoadingVisibility => IsThreadLoading ? Visibility.Visible : Visibility.Collapsed;

        public int SelectedThread
        {
            get { return _selectedThread; }
            set
            {
                Set(ref _selectedThread, value);
                if (ThreadVisible)
                {
                    Loc.NavigationService.ShowBackButtonIfCanGoBack();
                    RaisePropertyChanged(nameof(CurrentThread));
                }
                if (value == -1)
                {
                    // No topic displayed, reset webview
                    UpdateThreadWebView(null);
                }
            }
        }

        public Thread CurrentThread
        {
            get
            {
                if (IsInDesignMode)
                {
                    return new Topic()
                    {
                        ThreadName = "TU DesignTime"
                    };
                }
                if (!Loc.Main.Threads.Any() || SelectedThread == -1) return null;
                else if (SelectedThread > -1 && SelectedThread < Loc.Main.Threads.Count)
                    return Loc.Main.Threads[(int)SelectedThread];
                else
                {
                    SelectedThread = 0;
                    return Loc.Main.Threads[(int)SelectedThread];
                }
            }
        }

        public bool ThreadVisible
        {
            get
            {
                Loc.Main.TriggerUIAdapter();
                return CurrentThread != null;
            }
        }
        #endregion

        #region public fields
        #endregion

        #region commands
        public ChangeThreadPageCommand ChangeThreadPageCommand { get; } = new ChangeThreadPageCommand();
        public RefreshThreadCommand RefreshThreadCommand { get; } = new RefreshThreadCommand();
        public ShowEditorCommand ShowEditorCommand { get; } = new ShowEditorCommand();
        #endregion

        #region events
        public event ThreadReadyToBeDisplayed ThreadReadyToBeDisplayed;
        #endregion

        #region methods
        public void UpdateThreadWebView(Thread thread)
        {
            ThreadReadyToBeDisplayed?.Invoke(thread);
            if (thread != null && thread is Topic)
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
                        CurrentThread.ThreadCurrentPage = CurrentThread.ThreadNbPage;
                        break;
                    case EditorIntent.Edit:
                    case EditorIntent.Delete:
                        break;
                    default:
                        break;
                }
            });
            await Task.Run(async () => await ThreadFetcher.GetPosts(CurrentThread));
        }
        #endregion
    }
}