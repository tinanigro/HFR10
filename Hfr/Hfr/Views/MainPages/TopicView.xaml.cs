using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.Web;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Views.MainPages
{
    public sealed partial class TopicView : UserControl
    {
        public TopicView()
        {
            this.InitializeComponent();
            Loc.Topic.TopicReadyToBeDisplayed += CurrentTopic_TopicReadyToBeDisplayed;
        }

        private void CurrentTopic_TopicReadyToBeDisplayed(Model.Topic topic)
        {
            TopicWebView.NavigationCompleted += TopicWebViewOnNavigationCompleted;
            TopicWebView.Navigate(Strings.TopicPageCacheUri);
        }

        private async void TopicWebViewOnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            TopicWebView.NavigationCompleted -= TopicWebViewOnNavigationCompleted;
            if (!string.IsNullOrEmpty(Loc.Topic.CurrentTopic.TopicReponseId))
            {
                await TopicWebView.InvokeScriptAsync("scrollTo", new string[1]
                {
                    Loc.Topic.CurrentTopic.TopicReponseId
                });
            }
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
                        Loc.Topic.ShowEditorCommand.Execute(Loc.Topic.CurrentTopic.TopicNewPostUriForm + $"&numrep={postId}");
                }
                else
                {
                    Debug.WriteLine("WW " + args.Uri.Query + "-- " + args.Uri + " -- " + args.Uri.AbsoluteUri);
                    string param = args.Uri.Query.Replace("?", "");
                    if (Loc.Main.ContextMessageCommand.CanExecute(param))
                        Loc.Main.ContextMessageCommand.Execute(param);
                }
            }
            else
                Debug.WriteLine("WW initial =" + args);
        }
    }
}