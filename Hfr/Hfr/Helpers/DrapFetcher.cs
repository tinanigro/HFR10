using Hfr.Model;
using Hfr.Models;
using Hfr.Utilities;
using Hfr.ViewModel;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hfr.Models.Threads;

namespace Hfr.Helpers
{
    public static class DrapFetcher
    {
        public static async Task GetDraps(FollowedTopicType topicType)
        {
            Debug.WriteLine("Fetching Drapeaux");
            var draps = await Fetch(topicType);
            Debug.WriteLine("Updating UI with new Drapeaux list");
            await ThreadUI.Invoke(() =>
            {
                Loc.Main.Drapeaux = draps;
                if (draps != null)
                {
                    Loc.Main.DrapsGrouped = draps.GroupBy(x => x.TopicCatName);
                }
            });
        }

        static async Task<ObservableCollection<Topic>> Fetch(FollowedTopicType topicType)
        {
            var url = "";
            switch (topicType)
            {
                case FollowedTopicType.Favoris:
                    url = HFRUrl.FavsUrl;
                    break;
                case FollowedTopicType.Drapeaux:
                    url = HFRUrl.DrapsUrl;
                    break;
                case FollowedTopicType.Lus:
                    url = HFRUrl.ReadsUrl;
                    break;
                default:
                    break;
            }
            var html = await HttpClientHelper.Get(url);
            if (string.IsNullOrEmpty(html)) return null;

            /* DG */
            Stopwatch stopwatch = new Stopwatch();
            Debug.WriteLine("Start Bench");
            stopwatch.Reset();
            stopwatch.Start();
            /* DG */

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            string[] userIdArray = htmlDoc.DocumentNode.Descendants("a")
                                    .Where(x => x.GetAttributeValue("href", "").Contains("/user/allread.php?id_user="))
                                    .Select(y => y.GetAttributeValue("href", "")).ToArray();

            int userID = Convert.ToInt32(userIdArray[0].Split('=')[1].Split('&')[0]);
            await ThreadUI.Invoke(() => Loc.Main.AccountManager.CurrentAccount.UserId = userID);
            Loc.Main.AccountManager.UpdateCurrentAccountInDB();

            /* DG */
            stopwatch.Stop();
            Debug.WriteLine("Bench Middle: " + stopwatch.ElapsedTicks +
            " mS: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Start();
            /* DG */

            var messagesArray = ThreadHelper.GetPostNodesFromHtmlDoc(htmlDoc);
            if (messagesArray == null) return null;

            var topics = new ObservableCollection<Topic>();
            foreach (var msg in messagesArray)
            {
                var sujetCase10 = ThreadHelper.GetSujetCase10(msg);
                var id = ThreadHelper.GetIdFromSujetCase10Node(sujetCase10);
                var catId = ThreadHelper.GetCatIdFromSujetCase10Node(sujetCase10);

                var sujetCase9 = ThreadHelper.GetSujetCase9(msg);
                var threadLastPostDateTime = ThreadHelper.GetDateTimeLastPostFromNode(sujetCase9);
                var lastPoster = ThreadHelper.ThreadLastPostMemberPseudo(sujetCase9);

                var isNew = ThreadHelper.NewPost(msg);

                var subject = ThreadHelper.ThreadName(msg);
                subject = ThreadNameHelper.Shorten(subject);

                var nbPage = ThreadHelper.GetNbPageFromNode(msg);
                var author = ThreadHelper.ThreadAuthor(msg);
                var threadUrl = msg.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "") == "sujetCase5")?.FirstChild?.GetAttributeValue("href", "")?.CleanFromWeb();

                var subCatId = ThreadHelper.GetSubCatId(threadUrl);
                var rep = ThreadHelper.GetBookmarkId(threadUrl);
                var currentPage = ThreadHelper.GetCurrentPage(threadUrl);
                
                var topic = new Topic();
                topic.ThreadId = id;
                topic.ThreadName = subject;
                topic.ThreadUri = threadUrl;
                topic.ThreadAuthor = author;
                topic.ThreadHasNewPost = isNew;
                topic.ThreadLastPostDate = threadLastPostDateTime;
                topic.ThreadLastPostMemberPseudo = lastPoster;
                topic.ThreadNbPage = nbPage;

                topic.ThreadCurrentPage = currentPage;
                topic.ThreadBookmarkId = rep;
                topic.TopicCatId = catId;
                topic.TopicSubCatId = subCatId;

                topics.Add(topic);
            }

            /* DG */
            stopwatch.Stop();
            Debug.WriteLine("Bench End: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);
            /* DG */

            Debug.WriteLine("Drapeaux fetched");
            return topics;
        }
    }
}
