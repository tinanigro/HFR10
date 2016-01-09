using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hfr.Models;
using Hfr.Models.Threads;
using Hfr.Utilities;
using Hfr.ViewModel;
using HtmlAgilityPack;

namespace Hfr.Helpers
{
    public static class PrivateChatsFetcher
    {
        public static async Task GetPrivateChats()
        {
            Debug.WriteLine("Fetching Messages");
            var msgs = await Fetch();
            Debug.WriteLine("Updating UI with new Messages list");
            await ThreadUI.Invoke(() =>
            {
                Loc.Main.PrivateChats = msgs;
                if (msgs != null)
                {
                    Loc.Main.PrivateChatsGrouped = msgs.GroupBy(x => x.ThreadLastPostDate.ToString("Y"));
                }
            });
        }

        public static async Task<List<PrivateChat>> Fetch()
        {
            var html = await HttpClientHelper.Get(HFRUrl.MessagesUrl + "&page=1"); // TODO : Implement multiple pages
            if (string.IsNullOrEmpty(html)) return null;

            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var messagesArray = ThreadHelper.GetPostNodesFromHtmlDoc(htmlDoc);
            if (messagesArray == null) return null;

            var messages = new List<PrivateChat>();
            foreach (var msg in messagesArray)
            {
                var sujetCase10 = ThreadHelper.GetSujetCase10(msg);
                var id = ThreadHelper.GetIdFromSujetCase10Node(sujetCase10);

                var sujetCase9 = ThreadHelper.GetSujetCase9(msg);
                var threadLastPostDateTime = ThreadHelper.GetDateTimeLastPostFromNode(sujetCase9);
                var lastPoster = ThreadHelper.ThreadLastPostMemberPseudo(sujetCase9);

                var isNew = ThreadHelper.NewPost(msg);
                var subject = ThreadHelper.ThreadName(msg);
                var nbPage = ThreadHelper.GetNbPageFromNode(msg);
                var author = ThreadHelper.ThreadAuthor(msg);
                var threadUrl = msg.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "") == "sujetCase3")?.Descendants("a")?.First()?.GetAttributeValue("href", "")?.CleanFromWeb();

                var currentPage = ThreadHelper.GetCurrentPage(threadUrl);

                var message = new PrivateChat();
                message.ThreadId = id;
                message.ThreadName = subject;
                message.ThreadUri = threadUrl;
                message.ThreadAuthor = author;
                message.ThreadHasNewPost = isNew;
                message.ThreadLastPostDate = threadLastPostDateTime;
                message.ThreadLastPostMemberPseudo = lastPoster;
                message.ThreadNbPage = nbPage;
                message.ThreadCurrentPage = currentPage;

                messages.Add(message);
            }
            return messages;
        }
    }
}
