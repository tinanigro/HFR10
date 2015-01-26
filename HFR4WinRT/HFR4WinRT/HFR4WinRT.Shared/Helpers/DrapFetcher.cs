﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HFR4WinRT.Model;
using HFR4WinRT.Utilities;
using HFR4WinRT.ViewModel;
using HtmlAgilityPack;

namespace HFR4WinRT.Helpers
{
    public static class DrapFetcher
    {
        public static async Task GetDraps()
        {
            Debug.WriteLine("Fetching Drapeaux");
            var draps = await Fetch();
            Debug.WriteLine("Updating UI with new Drapeaux list");
            await ThreadUI.Invoke(() => Locator.Main.Drapeaux = draps);
        }

        public static async Task<ObservableCollection<Topic>> Fetch()
        {
            var html = await HttpClientHelper.Get(HFRUrl.DrapFavUrl, Locator.Main.AccountManager.CurrentAccount.CookieContainer);
            if (string.IsNullOrEmpty(html)) return null;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            string[] userIdArray = htmlDoc.DocumentNode.Descendants("a")
                                    .Where(x => x.GetAttributeValue("href", "").Contains("/user/allread.php?id_user="))
                                    .Select(y => y.GetAttributeValue("href", "")).ToArray();

            int userID = Convert.ToInt32(userIdArray[0].Split('=')[1].Split('&')[0]);
            await ThreadUI.Invoke(() => Locator.Main.AccountManager.CurrentAccount.UserId = userID);
            Locator.Main.AccountManager.UpdateCurrentAccountInDB();

            int i = 0;
            string[] favorisTopicNames = htmlDoc.DocumentNode.Descendants("a")
                .Where(x =>
                    x.GetAttributeValue("class", "") == "cCatTopic" &&
                    x.GetAttributeValue("title", "").Contains("Sujet"))
                    .Select(y => y.InnerText).ToArray();

            string[] favorisTopicNumberOfPages = htmlDoc.DocumentNode.Descendants("td")
                .Where(x => x.GetAttributeValue("class", "") == "sujetCase4")
                .Select(y => y.InnerText).ToArray();

            string[] favorisTopicUri = htmlDoc.DocumentNode.Descendants("a")
                .Where(x =>
                    x.GetAttributeValue("class", "") == "cCatTopic" &&
                    x.GetAttributeValue("title", "").Contains("Sujet"))
                    .Select(y => y.GetAttributeValue("href", "")).ToArray();

            string[] favorisLastPost = htmlDoc.DocumentNode.Descendants("td")
                .Where(x => x.GetAttributeValue("class", "").Contains("sujetCase9"))
                .Select(y => y.InnerText).ToArray();

            string[] favorisIsHot = htmlDoc.DocumentNode.Descendants("img")
                .Where(x => x.GetAttributeValue("alt", "") == "Off" ||
                    x.GetAttributeValue("alt", "") == "On")
                    .Select(y => y.GetAttributeValue("alt", "")).ToArray();

            string[] favorisBalise = htmlDoc.DocumentNode.Descendants("a")
                .Where(x => x.GetAttributeValue("href", "").Contains("#t"))
                .Select(y => y.GetAttributeValue("href", "")).ToArray();

            string[] mpArray =
                htmlDoc.DocumentNode.Descendants("a").Where(x => x.GetAttributeValue("class", "") == "red")
                .Select(y => y.InnerText).ToArray();

            int j = 0;
            var topics = new ObservableCollection<Topic>();
            foreach (string line in favorisTopicNames)
            {
                if (favorisIsHot[i] == "On")
                {
                    var numberOfPagesTopicLine = favorisTopicNumberOfPages[i] != "&nbsp;"
                        ? int.Parse(favorisTopicNumberOfPages[i])
                        : 1;

                    var firstTopicCatId =
                        WebUtility.HtmlDecode(favorisBalise[j]).IndexOf("&cat=", StringComparison.Ordinal) +
                        "&cat=".Length;
                    var lastTopicCatId = WebUtility.HtmlDecode(favorisBalise[j])
                        .IndexOf("&", firstTopicCatId, StringComparison.Ordinal);
                    int topicCatId;
                    int.TryParse(
                        WebUtility.HtmlDecode(favorisBalise[j])
                            .Substring(firstTopicCatId, lastTopicCatId - firstTopicCatId), out topicCatId);

                    var firstTopicSubCatId =
                        WebUtility.HtmlDecode(favorisBalise[j])
                            .IndexOf("&subcat=", StringComparison.Ordinal) + "&subcat=".Length;
                    var lastTopicSubCatId = WebUtility.HtmlDecode(favorisBalise[j])
                        .IndexOf("&", firstTopicSubCatId, StringComparison.Ordinal);
                    var topicSubCatId = WebUtility.HtmlDecode(favorisBalise[j])
                        .Substring(firstTopicSubCatId, lastTopicSubCatId - firstTopicSubCatId);

                    var firstTopicId =
                        WebUtility.HtmlDecode(favorisBalise[j]).IndexOf("&post=", StringComparison.Ordinal) +
                        "&post=".Length;
                    var lastTopicId = WebUtility.HtmlDecode(favorisBalise[j])
                        .LastIndexOf("&page", StringComparison.Ordinal);
                    var topicId = WebUtility.HtmlDecode(favorisBalise[j])
                        .Substring(firstTopicId, lastTopicId - firstTopicId);

                    var firstReponseId =
                        WebUtility.HtmlDecode(favorisBalise[j]).IndexOf("#t", StringComparison.Ordinal) +
                        "#t".Length;
                    var lastReponseId = WebUtility.HtmlDecode(favorisBalise[j]).Length;
                    var reponseId = "rep" +
                                       WebUtility.HtmlDecode(favorisBalise[j])
                                           .Substring(firstReponseId, lastReponseId - firstReponseId);

                    var firstPageNumber =
                        WebUtility.HtmlDecode(favorisBalise[j]).IndexOf("&page=", StringComparison.Ordinal) +
                        "&page=".Length;
                    var lastPageNumber = WebUtility.HtmlDecode(favorisBalise[j])
                        .LastIndexOf("&p=", StringComparison.Ordinal);
                    var pageNumber = int.Parse(WebUtility.HtmlDecode(favorisBalise[j])
                        .Substring(firstPageNumber, lastPageNumber - firstPageNumber));

                    // Formatage topic name
                    string topicNameFav = TopicNameHelper.Shorten(WebUtility.HtmlDecode(line));

                    // Conversion date
                    string favorisSingleLastPostTimeString =
                        Regex.Replace(
                            Regex.Replace(WebUtility.HtmlDecode(favorisLastPost[i].Substring(0, 28)), "à",
                                ""), "-", "/");
                    DateTime favorisSingleLastPostDt;
                    favorisSingleLastPostDt = DateTime.Parse(favorisSingleLastPostTimeString,
                        new CultureInfo("fr-FR"));
                    double favorisSingleLastPostTime;
                    favorisSingleLastPostTime = Convert.ToDouble(favorisSingleLastPostDt.ToFileTime());

                    // Nom du dernier posteur
                    string favorisLastPostUser =
                        WebUtility.HtmlDecode(favorisLastPost[i].Substring(28,
                            favorisLastPost[i].Length - 28));

                    // Temps depuis dernier post
                    TimeSpan timeSpent;
                    timeSpent = DateTime.Now.Subtract(favorisSingleLastPostDt);
                    string favorisLastPostText = TopicNameHelper.TimeSinceLastReadMsg(timeSpent, favorisLastPostUser);

                    topics.Add(new Topic()
                    {
                        TopicName = topicNameFav,
                        TopicCatId = topicCatId,
                        TopicSubCatId = topicSubCatId,
                        TopicId = topicId,
                        TopicCatName = HFRCats.PlainNameFromId(topicCatId),
                        TopicLastPostDate = favorisSingleLastPostTime,
                        TopicLastPost = favorisLastPostText,
                        TopicLastPostTimeSpan = timeSpent,
                        TopicNbPage = numberOfPagesTopicLine,
                        TopicCurrentPage = pageNumber,
                        TopicReponseId = reponseId,
                        TopicIndexCategory = HFRCats.GetHFRIndexFromId(topicCatId),
                    });
                    j++;
                }
                i++;
            }
            Debug.WriteLine("Drapeaux fetched");
            return topics;
        }
    }
}