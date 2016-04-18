using System;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using Hfr.Models;
using System.Threading.Tasks;
using Hfr.Helpers;
using Hfr.Models.Threads;

namespace Hfr.Views.MainPages
{
    public sealed partial class ThreadView : UserControl
    {
        public ThreadView()
        {
            InitializeComponent();
            this.Loaded += ThreadView_Loaded;
            this.Unloaded += ThreadView_Unloaded;
        }

        private void ThreadView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ThreadWebView.NavigationCompleted += ThreadWebViewOnNavigationCompleted;
            Loc.Thread.ThreadReadyToBeDisplayed += CurrentThread_ThreadReadyToBeDisplayed;
            Loc.Editor.EditorCancelledMessage += Editor_EditorCancelledMessage;
            App.TelemetryClient.TrackPageView(nameof(ThreadView));
        }

        private async void Editor_EditorCancelledMessage()
        {
            try
            {
                await ThreadWebView.InvokeScriptAsync("resetAllMultiQuotes", new string[0]);
            }
            catch (Exception e)
            {
                App.TelemetryClient.TrackException(e);
            }
        }

        private void ThreadView_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ThreadWebView.NavigationCompleted -= ThreadWebViewOnNavigationCompleted;
            Loc.Thread.ThreadReadyToBeDisplayed -= CurrentThread_ThreadReadyToBeDisplayed;
            Loc.Editor.EditorCancelledMessage -= Editor_EditorCancelledMessage;
        }

        private void CurrentThread_ThreadReadyToBeDisplayed(Thread thread)
        {
            if (thread != null)
                ThreadWebView.Navigate(Strings.TopicPageCacheUri);
            else
                ThreadWebView.NavigateToString("");
        }

        private async void ThreadWebViewOnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.Uri == null) return;
            if (args.Uri.AbsoluteUri.Contains($"{Strings.WebSiteCacheFileName}#")) return;
            if (Loc.Editor.CurrentEditor != null)
            {
                var intent = Loc.Editor.CurrentEditor.Intent;
                switch (intent)
                {
                    case EditorIntent.New:
                    case EditorIntent.Quote:
                    case EditorIntent.MultiQuote:
                    case EditorIntent.Delete:
                        await ScrollTo("bas");
                        break;
                    case EditorIntent.Edit:
                        var anchor = "";
                        if (Loc.Editor.CurrentEditor.Data.TryGetValue("numreponse", out anchor))
                        {
                            if (string.IsNullOrEmpty(anchor)) return;
                            await ScrollTo(anchor);
                        }
                        break;
                    default:
                        break;
                }
                Loc.Editor.CurrentEditor = null;
            }
            else
            {
                if (Loc.Thread.CurrentThread?.ThreadBookmarkId > 0)
                {
                    await ScrollTo(Loc.Thread.CurrentThread.ThreadBookmarkId.ToString());
                }
            }
        }

        async Task ScrollTo(string anchor)
        {
            await ThreadWebView.InvokeScriptAsync("scrollTo", new string[1] { anchor });
        }

        private async void ThreadWebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null && args.Uri.AbsoluteUri.StartsWith(Strings.LocalUriPrefix))
            {
                args.Cancel = true;
                if (args.Uri.AbsoluteUri.Contains("quote"))
                {
                    var decoder = new WwwFormUrlDecoder(args.Uri.Query);
                    var postId = decoder.FirstOrDefault(x => x.Name == "postId")?.Value;
                    if (!string.IsNullOrEmpty(postId))
                    {
                        var package = new EditorPackage(EditorIntent.Quote, Loc.Thread.CurrentThread.ThreadNewPostUriForm + $"&numrep={postId}");
                        Loc.Thread.ShowEditorCommand.Execute(package);
                    }
                }
                else if (args.Uri.AbsoluteUri.Contains("deleteFromMultiQuote"))
                {
                    var decoder = new WwwFormUrlDecoder(args.Uri.Query);
                    var postId = decoder.FirstOrDefault(x => x.Name == "postId")?.Value;
                    if (!string.IsNullOrEmpty(postId))
                    {
                        var package = new EditorPackage(EditorIntent.MultiQuote, Loc.Thread.CurrentThread.ThreadNewPostUriForm + $"&numrep={postId}");
                        await Loc.Editor.RemoveQuoteFromMultiQuote(package);
                    }
                }
                else if (args.Uri.AbsoluteUri.Contains("multiQuote"))
                {
                    var decoder = new WwwFormUrlDecoder(args.Uri.Query);
                    var postId = decoder.FirstOrDefault(x => x.Name == "postId")?.Value;
                    if (!string.IsNullOrEmpty(postId))
                    {
                        var package = new EditorPackage(EditorIntent.MultiQuote, Loc.Thread.CurrentThread.ThreadNewPostUriForm + $"&numrep={postId}");
                        Loc.Thread.ShowEditorCommand.Execute(package);
                    }
                }
                else if (args.Uri.AbsoluteUri.Contains("edit"))
                {
                    var decoder = new WwwFormUrlDecoder(args.Uri.Query);
                    var postId = decoder.FirstOrDefault(x => x.Name == "postId")?.Value;
                    if (!string.IsNullOrEmpty(postId))
                    {
                        var package = new EditorPackage(EditorIntent.Edit, Loc.Thread.CurrentThread.ThreadNewPostUriForm + $"&numreponse={postId}");
                        Loc.Thread.ShowEditorCommand.Execute(package);
                    }
                }
                else
                {
                    Debug.WriteLine("WW " + args.Uri.Query + "-- " + args.Uri + " -- " + args.Uri.AbsoluteUri);
                    string param = args.Uri.Query.Replace("?", "");
                    if (Loc.Main.ContextMessageCommand.CanExecute(param))
                        Loc.Main.ContextMessageCommand.Execute(param);
                }
            }
        }
    }
}