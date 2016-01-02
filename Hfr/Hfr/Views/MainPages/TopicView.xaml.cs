using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Hfr.ViewModel;
namespace Hfr.Views.MainPages
{
    public sealed partial class TopicView : UserControl
    {
        public TopicView()
        {
            this.InitializeComponent();
            Loc.Main.TopicReadyToBeDisplayed += CurrentTopic_TopicReadyToBeDisplayed;
        }

        private void CurrentTopic_TopicReadyToBeDisplayed(Model.Topic topic, string computedHtml)
        {
            TopicWebView.NavigateToString(computedHtml);
        }

        private void TopicWebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null)
            {
                args.Cancel = true;
                Debug.WriteLine("WW " + args.Uri.Query + "-- " + args.Uri + " -- " + args.Uri.AbsoluteUri);
                string param = args.Uri.Query.Replace("?", "");
                var viewModel = (MainViewModel)DataContext;
                if (viewModel.ContextMessageCommand.CanExecute(param))
                    viewModel.ContextMessageCommand.Execute(param);

            }
            else
                Debug.WriteLine("WW initial =" + args);
        }
    }
}
