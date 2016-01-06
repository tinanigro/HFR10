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

namespace Hfr.Views.MainPages
{
    public sealed partial class TopicView : UserControl
    {
        public TopicView()
        {
            InitializeComponent();
            Loc.Topic.TopicReadyToBeDisplayed += CurrentTopic_TopicReadyToBeDisplayed;
        }

        private void CurrentTopic_TopicReadyToBeDisplayed(Topic topic)
        {
            TopicWebView.NavigationCompleted += TopicWebViewOnNavigationCompleted;
            if (topic != null)
                TopicWebView.Navigate(Strings.TopicPageCacheUri);
            else
                TopicWebView.NavigateToString("");
        }

        private async void TopicWebViewOnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            TopicWebView.NavigationCompleted -= TopicWebViewOnNavigationCompleted;

            if (Loc.Editor.CurrentEditor != null)
            {
                var intent = Loc.Editor.CurrentEditor.Intent;
                switch (intent)
                {
                    case EditorIntent.New:
                    case EditorIntent.Quote:
                    case EditorIntent.MultiQuote:
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
                if (!string.IsNullOrEmpty(Loc.Topic.CurrentTopic?.TopicReponseId))
                {
                    await ScrollTo(Loc.Topic.CurrentTopic.TopicReponseId);
                }
            }
        }

        async Task ScrollTo(string anchor)
        {
            await TopicWebView.InvokeScriptAsync("scrollTo", new string[1] { anchor });
        }

        private void TopicWebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
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
                        var package = new EditorPackage(EditorIntent.Quote, Loc.Topic.CurrentTopic.TopicNewPostUriForm + $"&numrep={postId}");
                        Loc.Topic.ShowEditorCommand.Execute(package);
                    }
                }
                else if (args.Uri.AbsoluteUri.Contains("multiQuote"))
                {
                    var decoder = new WwwFormUrlDecoder(args.Uri.Query);
                    var postId = decoder.FirstOrDefault(x => x.Name == "postId")?.Value;
                    if (!string.IsNullOrEmpty(postId))
                    {
                        var package = new EditorPackage(EditorIntent.MultiQuote, Loc.Topic.CurrentTopic.TopicNewPostUriForm + $"&numrep={postId}");
                        Loc.Topic.ShowEditorCommand.Execute(package);
                    }
                }
                else if (args.Uri.AbsoluteUri.Contains("edit"))
                {
                    var decoder = new WwwFormUrlDecoder(args.Uri.Query);
                    var postId = decoder.FirstOrDefault(x => x.Name == "postId")?.Value;
                    if (!string.IsNullOrEmpty(postId))
                    {
                        var package = new EditorPackage(EditorIntent.Edit, Loc.Topic.CurrentTopic.TopicNewPostUriForm + $"&numreponse={postId}");
                        Loc.Topic.ShowEditorCommand.Execute(package);
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