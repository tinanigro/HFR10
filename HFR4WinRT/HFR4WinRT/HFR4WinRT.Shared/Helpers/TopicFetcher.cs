using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Core;
using HFR4WinRT.Model;
using HFR4WinRT.Utilities;
using HFR4WinRT.ViewModel;
using HFR4WinRT.Views;
using HtmlAgilityPack;

namespace HFR4WinRT.Helpers
{
    public static class TopicFetcher
    {
        public static async Task GetPosts(int catId, string topicId, int topicNbPage)
        {
            Debug.WriteLine("Fetching Posts");
            await Fetch(catId, topicId, topicNbPage);
            Debug.WriteLine("Updating UI with new Posts list");
        }

        public static async Task Fetch(int catId, string topicId, int topicNbPage)
        {
            var html = await HttpClientHelper.Get("http://forum.hardware.fr/forum2.php?config=hfr.inc&cat=" + catId + "&post=" + topicId + "&page=" + topicNbPage + "&sondage=1&owntopic=1", Locator.Main.AccountManager.CurrentAccount.CookieContainer);
            if (string.IsNullOrEmpty(html)) return;
            
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html); await ThreadUI.Invoke(() =>
             {
                 Locator.Main.CurrentTopic.Html = html;

                 var topicText = htmlDoc.DocumentNode.Descendants("div")
                         .Where(x => x.GetAttributeValue("id", "").Contains("para"))
                         .
                         Select(y => y.InnerHtml).ToArray();
                 int i = 0;
                 string posts = "";
                 foreach (var post in topicText)
                 {
                     int lastPostText = topicText[i].IndexOf("<div style=\"clear: both;\"> </div>", StringComparison.Ordinal);
                     if (lastPostText == -1)
                     {
                         lastPostText = topicText[i].Length;
                     }
                     var postText = topicText[i].Substring(0, lastPostText);
                     postText = Regex.Replace(WebUtility.HtmlDecode(postText), " target=\"_blank\"", "");
                     Debug.WriteLine(postText);
                     posts += postText;
                     i++;
                 }

                 (Locator.NavigationService.CurrentPage as MainPage).WebView.NavigateToString(posts);
             });
        }
    }
}
