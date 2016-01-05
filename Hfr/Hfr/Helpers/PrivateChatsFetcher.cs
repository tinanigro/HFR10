using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hfr.Models;
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
                    Loc.Main.PrivateChatsGrouped = msgs.GroupBy(x => x.DateTime.ToString("Y"));
                }
            });
        }

        public static async Task<List<PrivateChat>> Fetch()
        {
            var html = await HttpClientHelper.Get(HFRUrl.MessagesUrl + "&page=1"); // TODO : Implement multiple pages
            if (string.IsNullOrEmpty(html)) return null;

            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var messagesArray =
                htmlDoc.DocumentNode.Descendants("tr")
                    .Where(x => x.GetAttributeValue("class", "").Contains("sujet ligne_booleen cBackCouleurTab"))
                    .ToArray();
            if (messagesArray == null) return null;

            var messages = new List<PrivateChat>();
            foreach (var msg in messagesArray)
            {
                var isNewNode = msg.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase1"));
                var isMsgNew = false;
                if (isNewNode.GetAttributeValue("class", "").Contains("cBackCouleurTab2"))
                {
                    isMsgNew = true;
                }
                var subject = msg.Descendants("a").FirstOrDefault(x => x.GetAttributeValue("class", "") == "cCatTopic").InnerText.CleanFromWeb();
                var lastMsgNode = msg.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase9")).FirstChild;

                var dateInnerText = lastMsgNode.FirstChild.InnerText;
                var dateString = WebUtility.HtmlDecode(dateInnerText).Replace("à", "");
                var dateTime = DateTime.Parse(dateString, new CultureInfo("fr-FR"));

                var lastPoster = msg.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase6")).FirstChild.InnerText.CleanFromWeb();
                var message = new PrivateChat();
                message.NewMsg = isMsgNew;
                message.Subject = subject;
                message.DateTime = dateTime;
                message.Poster = lastPoster;
                messages.Add(message);
            }
            return messages;
        }
    }
}
